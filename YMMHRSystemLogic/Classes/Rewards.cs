using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using YMMHRSystemLogic.DTO;

namespace YMMHRSystemLogic
{
    public class Rewards
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        EmailNotification emailNotification = new EmailNotification();
        SendEmail sendEmail = new SendEmail();
        #region
        /// <summary>
        /// Loads the Rewards DTO
        /// </summary>
        /// <param name=""></param>
        /// <returns>DtoWorkerRewards</returns>
        /// 
        public List<DtoWorkerRewards> GetWorkerRewards() { 
            var result = new List<DtoWorkerRewards>();
            string query = "";
            try 
            {
                query = "SELECT w.WorkerFileId, w.Names, w.WorkerId, w.Process, w.Job, ic.Score FROM ImprovementCredits ic JOIN WorkerFiles w ON ic.WorkerId = w.WorkerId WHERE w.Status = 1 ORDER BY w.WorkerFileId";
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoWorkerRewards> dtoListWorkerRewards = new List<DtoWorkerRewards>();
                mapping.Load<DtoWorkerRewards>(query, "ImprovementCredits", new DtoWorkerRewards());
                dtoListWorkerRewards.AddRange(mapping.dtoList.Select(renglon => (DtoWorkerRewards)renglon.Dto));

                return dtoListWorkerRewards;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load WorkerRewards", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadWorkerRewads");
                throw ex;
            }
        }

        public List<DtoImprovements> GetImprovementsbySelection(string[] selectedWorkerIds)
        {
            try
            {
                if (selectedWorkerIds == null || selectedWorkerIds.Length == 0)
                    return new List<DtoImprovements>();

                // 1) Sanitizar IDs (WorkerId es VARCHAR en DB)
                var ids = selectedWorkerIds
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim().Replace("'", "''")) // escapar comillas simples
                    .Distinct()
                    .ToList();

                if (ids.Count == 0)
                    return new List<DtoImprovements>();

                string inWorkerIds = string.Join(",", ids.Select(x => $"'{x}'"));

                // 2) Consultar tipos en WorkerFiles (solo Type)
                // Alias "Type" para que el mapper lo llene en DtoWorkerFile.Type
                string qTypes = $@"
                SELECT DISTINCT wf.[Type] AS [Type]
                FROM WorkerFiles wf
                WHERE wf.Status = 1
                  AND wf.WorkerId IN ({inWorkerIds});";

                var mappingTypes = new DBFrameworkMapping();
                mappingTypes.Load<DtoWorkerFile>(qTypes, "WorkerFiles", new DtoWorkerFile());

                var types = mappingTypes.dtoList
                    .Select(r => ((DtoWorkerFile)r.Dto).Type ?? "")
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                bool hasUnion = types.Any(t => t.Equals("Union Worker", StringComparison.OrdinalIgnoreCase));
                bool hasStaff = types.Any(t => t.Equals("Staff", StringComparison.OrdinalIgnoreCase));

                // 3) Resolver categorías WorkerType de Improvements
                var categories = new List<string>();
                if (hasUnion) categories.Add("'A'");
                if (hasStaff) categories.Add("'B'");
                categories.Add("'C'"); // C aplica a todos (según tu definición)

                string inWorkerTypes = string.Join(",", categories.Distinct());

                // 4) Traer catálogo filtrado
                string qImprovements = $@"
                SELECT ImprovementId, ImprovemenName, ScoreValue, WorkerType, UserCreated, DateAdded
                FROM Improvements
                WHERE WorkerType IN ({inWorkerTypes})
                ORDER BY WorkerType, ImprovemenName;";

                var mapping = new DBFrameworkMapping();
                mapping.Load<DtoImprovements>(qImprovements, "Improvements", new DtoImprovements());

                return mapping.dtoList.Select(r => (DtoImprovements)r.Dto).ToList();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Improvement", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "GetImprovementsbySelection");
                throw; // mantiene stacktrace
            }
        }

        public void ApplyScores(
                            string[] trustedWorkerIds, string[] associateWorkerIds,
                            long[] improvementIdsTrusted,       // B
                            long[] improvementIdsAssociates,    // A
                            long[] improvementIdsContingencies, // C
                            long userId)
        {
            try
            {
                var trusted = (trustedWorkerIds ?? Array.Empty<string>())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var associates = (associateWorkerIds ?? Array.Empty<string>())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var impB = (improvementIdsTrusted ?? Array.Empty<long>()).Distinct().ToList();
                var impA = (improvementIdsAssociates ?? Array.Empty<long>()).Distinct().ToList();
                var impC = (improvementIdsContingencies ?? Array.Empty<long>()).Distinct().ToList();

                var applyToTrusted = impB.Concat(impC).Distinct().ToList();       // Trusted: B + C
                var applyToAssociates = impA.Concat(impC).Distinct().ToList();    // Associates: A + C

                if (!applyToTrusted.Any() && !applyToAssociates.Any())
                    return;

                // 1) Traer ScoreValue de improvements una sola vez
                var allImpIds = applyToTrusted.Concat(applyToAssociates).Distinct().ToList();
                string inImp = string.Join(",", allImpIds); // numeric safe

                string qImp = $@"
                    SELECT ImprovementId, ScoreValue
                    FROM Improvements
                    WHERE ImprovementId IN ({inImp});";

                var mapImp = new Dictionary<long, int>();
                var mImp = new DBFrameworkMapping();
                mImp.Load<DtoImprovements>(qImp, "Improvements", new DtoImprovements());
                foreach (var row in mImp.dtoList.Select(x => (DtoImprovements)x.Dto))
                    mapImp[row.ImprovementId] = row.ScoreValue;

                // 2) Función local para aplicar movimientos (sin helper externo)
                void ApplyFor(string workerIdRaw, long improvementId)
                {
                    if (!mapImp.TryGetValue(improvementId, out var delta)) return;

                    // WorkerId es VARCHAR -> escapar comillas
                    string workerId = workerIdRaw.Replace("'", "''");

                    // Batch SQL: update + insert history cumpliendo CHECKs
                    string q = $@"
                        DECLARE @t TABLE (BeforeScore INT, AfterScore INT);

                        IF NOT EXISTS (SELECT 1 FROM ImprovementCredits WITH (UPDLOCK, HOLDLOCK) WHERE WorkerId = '{workerId}')
                        BEGIN
                            INSERT INTO ImprovementCredits (WorkerId, Score) VALUES ('{workerId}', 0);
                        END

                        UPDATE ImprovementCredits WITH (UPDLOCK, HOLDLOCK)
                        SET Score = Score + {delta}
                        OUTPUT deleted.Score, inserted.Score INTO @t
                        WHERE WorkerId = '{workerId}';

                        INSERT INTO RewardsHistory
                        (
                            WorkerId, TransactionType, ImprovementId, RedeemId,
                            ScoreBefore, ScoreDelta, ScoreAfter, UserCreated
                        )
                        OUTPUT
                            inserted.RewardsHistoryId,
                            inserted.WorkerId,
                            inserted.TransactionType,
                            inserted.ImprovementId,
                            inserted.RedeemId,
                            inserted.ScoreBefore,
                            inserted.ScoreDelta,
                            inserted.ScoreAfter,
                            inserted.TransactionDate,
                            inserted.UserCreated
                        SELECT
                            '{workerId}', 1, {improvementId}, NULL,
                            BeforeScore, {delta}, AfterScore, {userId}
                        FROM @t;";

                    var m = new DBFrameworkMapping();
                    // Ejecuta y “mapea” el OUTPUT
                    m.Load<DtoRewardsHistory>(q, "RewardsHistory", new DtoRewardsHistory());
                }

                // 3) Aplicar a Trusted
                foreach (var workerId in trusted)
                    foreach (var impId in applyToTrusted)
                        ApplyFor(workerId, impId);

                // 4) Aplicar a Associates
                foreach (var workerId in associates)
                    foreach (var impId in applyToAssociates)
                        ApplyFor(workerId, impId);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Apply Scores", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "ApplyScores");
                throw;
            }
        }

        public List<DtoRewardsHistoryLog> GetHistorybyWorkerId(string workerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(workerId))
                    return new List<DtoRewardsHistoryLog>();

                string safeWorkerId = workerId.Trim().Replace("'", "''");

                string query = $@"
                    SELECT
                        rwh.TransactionType,
                        ISNULL(i.ImprovemenName, '') AS ImprovemenName,
                        ISNULL(rds.RedeemName, '') AS RedeemName,
                        rwh.ScoreBefore,
                        rwh.ScoreDelta,
                        rwh.ScoreAfter,
                        CONVERT(VARCHAR(19), rwh.TransactionDate, 120) AS TransactionDate,
                        u.Name AS TransactionName
                    FROM RewardsHistory rwh
                    LEFT JOIN Improvements i ON rwh.ImprovementId = i.ImprovementId
                    LEFT JOIN Users u ON rwh.UserCreated = u.UserId
                    LEFT JOIN (
                        SELECT
                            rh.RedeemId,
                            CONCAT(
                                ' (', rh.TotalPoints, ' pts)',
                                CASE 
                                    WHEN COUNT(rl.RedeemLineId) > 0 THEN ' - ' + STRING_AGG(CONCAT(ri.ItemName, ' x', rl.ItemRedeemQuantity), ', ')
                                    ELSE ''
                                END
                            ) AS RedeemName
                        FROM RedeemHeader rh
                        LEFT JOIN RedeemLines rl ON rh.RedeemId = rl.RedeemId
                        LEFT JOIN RewardsItems ri ON rl.ItemId = ri.ItemId
                        GROUP BY rh.RedeemId, rh.TotalPoints
                    ) rds ON rwh.RedeemId = rds.RedeemId
                    WHERE rwh.WorkerId = '{safeWorkerId}'
                    ORDER BY rwh.TransactionDate DESC, rwh.RewardsHistoryId DESC;";

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.Load<DtoRewardsHistoryLog>(query, "RewardsHistory", new DtoRewardsHistoryLog());

                return mapping.dtoList.Select(r => (DtoRewardsHistoryLog)r.Dto).ToList();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load RewardsHistory by WorkerId", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "GetHistorybyWorkerId");
                throw;
            }
        }

        public int GetScoreByWorkerId(string workerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(workerId)) return 0;

                var safe = workerId.Trim().Replace("'", "''");

                string query = $@"
                    SELECT ISNULL(Score, 0) AS Score
                    FROM ImprovementCredits
                    WHERE WorkerId = '{safe}';";

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.Load<DtoScoreRow>(query, "ImprovementCredits", new DtoScoreRow());

                if (mapping.dtoList == null || mapping.dtoList.Count == 0)
                    return 0;

                return ((DtoScoreRow)mapping.dtoList.First().Dto).Score;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Score By WorkerId", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "GetScoreByWorkerId");
                throw;
            }
        }

        public List<DtoRewardsItem> GetRedeemableItems(int score)
        {
            try
            {
                if (score <= 0) return new List<DtoRewardsItem>();

                string query = $@"
                    SELECT ItemId, ItemName, ValuePoints, Stock, StockMin, StockMax, IsActive
                    FROM RewardsItems
                    WHERE IsActive = 1
                      AND Stock > 0
                      AND ValuePoints <= {score}
                    ORDER BY ValuePoints ASC, ItemName ASC;";

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.Load<DtoRewardsItem>(query, "RewardsItems", new DtoRewardsItem());

                return mapping.dtoList.Select(r => (DtoRewardsItem)r.Dto).ToList();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Redeemable Items", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "GetRedeemableItems");
                throw;
            }
        }

        public DtoRedeemResult SaveRedeemRewards(string workerId, long[] itemIds, int[] quantities, long userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(workerId))
                    return new DtoRedeemResult { Success = 0, Message = "WorkerId is required." };

                if (itemIds == null || quantities == null || itemIds.Length == 0 || itemIds.Length != quantities.Length)
                    return new DtoRedeemResult { Success = 0, Message = "Invalid items payload." };

                // Sanitizar WorkerId (varchar)
                string safeWorkerId = workerId.Trim().Replace("'", "''");

                // Construir VALUES para tabla variable @Lines (ItemId, Qty)
                // (IDs numéricos: no requieren comillas)
                var values = new List<string>();
                for (int i = 0; i < itemIds.Length; i++)
                {
                    long id = itemIds[i];
                    int qty = quantities[i];

                    if (id > 0 && qty > 0)
                        values.Add($"({id},{qty})");
                }

                if (values.Count == 0)
                    return new DtoRedeemResult { Success = 0, Message = "Select at least one item." };

                string valuesSql = string.Join(",", values);

                // Batch SQL: valida saldo, valida stock, inserta header/lines, descuenta stock, descuenta puntos, inserta history
                string query = $@"
                        SET NOCOUNT ON;
                        SET XACT_ABORT ON;

                        BEGIN TRY
                            BEGIN TRAN;

                            DECLARE @WorkerId VARCHAR(250) = '{safeWorkerId}';
                            DECLARE @UserCreated BIGINT = {userId};

                            DECLARE @Lines TABLE(ItemId BIGINT, Qty INT);
                            INSERT INTO @Lines(ItemId, Qty)
                            VALUES {valuesSql};

                            -- Validar que exista saldo
                            IF NOT EXISTS (SELECT 1 FROM ImprovementCredits WITH (UPDLOCK, HOLDLOCK) WHERE WorkerId = @WorkerId)
                            BEGIN
                                -- Si no existe, se considera saldo 0
                                INSERT INTO ImprovementCredits (WorkerId, Score) VALUES (@WorkerId, 0);
                            END

                            DECLARE @ScoreBefore INT;
                            SELECT @ScoreBefore = Score
                            FROM ImprovementCredits WITH (UPDLOCK, HOLDLOCK)
                            WHERE WorkerId = @WorkerId;

                            -- Calcular total usando el costo real del catálogo (no confiamos en el front)
                            DECLARE @TotalPoints INT;
                            SELECT @TotalPoints = SUM(l.Qty * ri.ValuePoints)
                            FROM @Lines l
                            JOIN RewardsItems ri WITH (UPDLOCK, HOLDLOCK) ON ri.ItemId = l.ItemId
                            WHERE ri.IsActive = 1;

                            IF @TotalPoints IS NULL OR @TotalPoints <= 0
                                THROW 50001, 'Invalid redeem total.', 1;

                            -- Validar stock y activos
                            IF EXISTS (
                                SELECT 1
                                FROM @Lines l
                                JOIN RewardsItems ri WITH (UPDLOCK, HOLDLOCK) ON ri.ItemId = l.ItemId
                                WHERE ri.IsActive = 0 OR ri.Stock < l.Qty
                            )
                                THROW 50002, 'Insufficient stock for one or more items.', 1;

                            -- Validar saldo
                            IF @ScoreBefore < @TotalPoints
                                THROW 50003, 'Not enough points to redeem selected items.', 1;

                            -- Insertar header
                            INSERT INTO RedeemHeader (WorkerId, TotalPoints, UserCreated)
                            VALUES (@WorkerId, @TotalPoints, @UserCreated);

                            DECLARE @RedeemId BIGINT = SCOPE_IDENTITY();

                            -- Insertar líneas (PointsPerItem real y LineTotalPoints calculado)
                            INSERT INTO RedeemLines (RedeemId, ItemId, ItemRedeemQuantity, PointsPerItem, LineTotalPoints)
                            SELECT
                                @RedeemId,
                                l.ItemId,
                                l.Qty,
                                ri.ValuePoints,
                                l.Qty * ri.ValuePoints
                            FROM @Lines l
                            JOIN RewardsItems ri ON ri.ItemId = l.ItemId;

                            -- Descontar stock
                            UPDATE ri
                            SET ri.Stock = ri.Stock - l.Qty
                            FROM RewardsItems ri
                            JOIN @Lines l ON l.ItemId = ri.ItemId;

                            -- Descontar puntos
                            UPDATE ImprovementCredits
                            SET Score = Score - @TotalPoints
                            WHERE WorkerId = @WorkerId;

                            DECLARE @ScoreAfter INT = @ScoreBefore - @TotalPoints;

                            -- Insertar historial (TransactionType=2, delta negativo, ImprovementId NULL, RedeemId NOT NULL)
                            INSERT INTO RewardsHistory
                            (WorkerId, TransactionType, ImprovementId, RedeemId, ScoreBefore, ScoreDelta, ScoreAfter, UserCreated)
                            VALUES
                            (@WorkerId, 2, NULL, @RedeemId, @ScoreBefore, -@TotalPoints, @ScoreAfter, @UserCreated);

                            COMMIT;

                            SELECT
                                1 AS Success,
                                'Redeem saved successfully.' AS Message,
                                @RedeemId AS RedeemId,
                                @TotalPoints AS TotalPoints,
                                @ScoreAfter AS NewBalance;

                        END TRY
                        BEGIN CATCH
                            IF @@TRANCOUNT > 0 ROLLBACK;

                            SELECT
                                0 AS Success,
                                ERROR_MESSAGE() AS Message,
                                0 AS RedeemId,
                                0 AS TotalPoints,
                                0 AS NewBalance;
                        END CATCH;
                        ";

                var mapping = new DBFrameworkMapping();
                mapping.Load<DtoRedeemResult>(query, "RedeemHeader", new DtoRedeemResult());

                if (mapping.dtoList == null || mapping.dtoList.Count == 0)
                    return new DtoRedeemResult { Success = 0, Message = "No response from DB." };

                var result = (DtoRedeemResult)mapping.dtoList.First().Dto;

                if (result.Success == 1 && result.RedeemId > 0)
                {
                    // workerId original (no el safe) para el correo
                    NotifyLowStockAfterRedeem(result.RedeemId, workerId, userId);
                }

                return result;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Redeem Rewards", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "SaveRedeemRewards");
                throw;
            }
        }

        public bool ResetPoints(string[] workerIds, long userid)
        {
            try
            {
                if (workerIds == null || workerIds.Length == 0)
                    return false;

                // Sanitizar + eliminar vacíos/duplicados
                var ids = workerIds
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim().Replace("'", "''"))
                    .Distinct()
                    .ToList();

                if (ids.Count == 0)
                    return false;

                // Generar batch por cada WorkerId (para capturar ScoreBefore individual)
                // Nota: usamos transacción para que sea consistente
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("SET NOCOUNT ON;");
                sb.AppendLine("SET XACT_ABORT ON;");
                sb.AppendLine("BEGIN TRY");
                sb.AppendLine("BEGIN TRAN;");

                foreach (var wid in ids)
                {
                    sb.AppendLine($@"
                        DECLARE @WorkerId VARCHAR(250) = '{wid}';
                        DECLARE @Before INT;

                        -- Asegurar que exista el registro en ImprovementCredits
                        IF NOT EXISTS (SELECT 1 FROM ImprovementCredits WITH (UPDLOCK, HOLDLOCK) WHERE WorkerId = @WorkerId)
                        BEGIN
                            INSERT INTO ImprovementCredits (WorkerId, Score) VALUES (@WorkerId, 0);
                        END

                        SELECT @Before = Score
                        FROM ImprovementCredits WITH (UPDLOCK, HOLDLOCK)
                        WHERE WorkerId = @WorkerId;

                        -- Solo registrar si realmente había puntos
                        IF (@Before > 0)
                        BEGIN
                            UPDATE ImprovementCredits
                            SET Score = 0
                            WHERE WorkerId = @WorkerId;

                            INSERT INTO RewardsHistory
                            (
                                WorkerId, TransactionType, ImprovementId, RedeemId,
                                ScoreBefore, ScoreDelta, ScoreAfter, UserCreated
                            )
                            VALUES
                            (
                                @WorkerId, 3, NULL, NULL,
                                @Before, -@Before, 0, {userid}
                            );
                        END
                        ");
                }

                sb.AppendLine("COMMIT;");
                sb.AppendLine("END TRY");
                sb.AppendLine("BEGIN CATCH");
                sb.AppendLine("IF @@TRANCOUNT > 0 ROLLBACK;");
                sb.AppendLine("THROW;");
                sb.AppendLine("END CATCH");

                oDatabase.ExecuteNonQuery(sb.ToString(), "Reset Points + RewardsHistory");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Reset Points (with History)", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "ResetPoints");
                return false;
            }
        }

        public List<DtoRewardsItem> GetRewardsItems()
        {
            var result = new List<DtoRewardsItem>();
            string query = "";
            try
            {
                query = "SELECT * FROM RewardsItems";
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoRewardsItem> dtoListRewardsItems = new List<DtoRewardsItem>();
                mapping.Load<DtoRewardsItem>(query, "RewardsItems", new DtoRewardsItem());
                dtoListRewardsItems.AddRange(mapping.dtoList.Select(renglon => (DtoRewardsItem)renglon.Dto));

                return dtoListRewardsItems;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load RewardsItems", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadRewardsItems");
                throw ex;
            }
        }

        public bool SaveRewardsItem(DtoRewardsItem item, long userId)
        {
            try
            {
                // Normalizar / sanitizar
                string name = (item.ItemName ?? "").Trim().Replace("'", "''");
                int valuePoints = item.ValuePoints;
                int stock = item.Stock;
                int stockMin = item.StockMin;
                int stockMax = item.StockMax;
                int isActive = item.IsActive ? 1 : 0;

                // INSERT (ItemId es identity)
                string query = $@"
                    INSERT INTO RewardsItems (ItemName, ValuePoints, Stock, StockMin, StockMax, IsActive)
                    VALUES ('{name}', {valuePoints}, {stock}, {stockMin}, {stockMax}, {isActive});";

                oDatabase.ExecuteNonQuery(query, "Insert RewardsItems");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Rewards Item", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "SaveRewardsItem");
                return false;
            }
        }

        public DtoRewardsItem GetRewardsItembyId(long itemId)
        {
            try
            {
                string query = "SELECT ItemId, ItemName, ValuePoints, Stock, Cost, StockMin, StockMax, IsActive " +
                               "FROM RewardsItems WHERE ItemId = " + itemId.ToString();

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.Load<DtoRewardsItem>(query, "RewardsItems", new DtoRewardsItem());

                if (mapping.dtoList == null || mapping.dtoList.Count == 0)
                    return new DtoRewardsItem();

                return (DtoRewardsItem)mapping.dtoList.First().Dto;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Rewards Item", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "GetRewardsItembyId");
                return new DtoRewardsItem();
            }
        }

        public bool UpdateRewardsItem(DtoRewardsItem item)
        {
            try
            {
                // Sanitizar strings
                string name = (item.ItemName ?? "").Trim().Replace("'", "''");
                int isActive = item.IsActive ? 1 : 0;

                // Update completo (reemplaza valores)
                string query = $@"
                    UPDATE RewardsItems
                    SET
                        ItemName = '{name}',
                        ValuePoints = {item.ValuePoints},
                        Stock = {item.Stock},
                        Cost = {item.Cost},
                        StockMin = {item.StockMin},
                        StockMax = {item.StockMax},
                        IsActive = {isActive}
                    WHERE ItemId = {item.ItemId};";

                oDatabase.ExecuteNonQuery(query, "Update RewardsItems");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Rewards Item", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "UpdateRewardsItem");
                return false;
            }
        }

        public bool DeleteRewardsItem(long itemId)
        {
            try
            {
                if (itemId <= 0) return false;

                string query = $@"
                    UPDATE RewardsItems
                    SET IsActive = 0
                    WHERE ItemId = {itemId};";

                oDatabase.ExecuteNonQuery(query, "Soft delete RewardsItems (IsActive=0)");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Rewards Item", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "DeleteRewardsItem");
                return false;
            }
        }

        private void NotifyLowStockAfterRedeem(long redeemId, string workerId, long userId)
        {
            try
            {
                // 1) Obtener destinatarios desde EmailNotifications.RewardInventory
                var contactsStr = emailNotification.GetRewardInventoryContacts(1);
                if (string.IsNullOrWhiteSpace(contactsStr)) return;

                var contacts = contactsStr
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => x.Length > 0)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (contacts.Count == 0) return;

                // 2) Consultar SOLO items de este redeem cuyo stock quedó <= stockmin
                string q = $@"
                    SELECT
                        ri.ItemId,
                        ri.ItemName,
                        ri.Stock,
                        ri.StockMin,
                        SUM(rl.ItemRedeemQuantity) AS RedeemedQty
                    FROM RedeemLines rl
                    JOIN RewardsItems ri ON rl.ItemId = ri.ItemId
                    WHERE rl.RedeemId = {redeemId}
                    GROUP BY ri.ItemId, ri.ItemName, ri.Stock, ri.StockMin
                    HAVING ri.Stock <= ri.StockMin
                    ORDER BY ri.Stock ASC;";

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.Load<DtoLowStockRow>(q, "RewardsItems", new DtoLowStockRow());

                var lowItems = mapping.dtoList.Select(r => (DtoLowStockRow)r.Dto).ToList();
                if (lowItems.Count == 0) return;

                // 3) Construir tabla HTML (se la pasamos al template)
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("<table style='border-collapse:collapse;width:100%'>");
                sb.AppendLine("<thead><tr>");
                sb.AppendLine("<th style='border:1px solid #ddd;padding:6px;text-align:left'>Item</th>");
                sb.AppendLine("<th style='border:1px solid #ddd;padding:6px;text-align:right'>Redeemed</th>");
                sb.AppendLine("<th style='border:1px solid #ddd;padding:6px;text-align:right'>Stock</th>");
                sb.AppendLine("<th style='border:1px solid #ddd;padding:6px;text-align:right'>Min</th>");
                sb.AppendLine("</tr></thead><tbody>");
                foreach (var it in lowItems)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td style='color: #D71A2F;border:1px solid #ddd;padding:6px'><b>{System.Net.WebUtility.HtmlEncode(it.ItemName)}</b></td>");
                    sb.AppendLine($"<td style='color: #D71A2F;border:1px solid #ddd;padding:6px;text-align:right'><b>{it.RedeemedQty}</b></td>");
                    sb.AppendLine($"<td style='color: #D71A2F;border:1px solid #ddd;padding:6px;text-align:right'><b>{it.Stock}</b></td>");
                    sb.AppendLine($"<td style='color: #D71A2F;border:1px solid #ddd;padding:6px;text-align:right'><b>{it.StockMin}</b></td>");
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</tbody></table>");

                // 4) Enviar correo usando template HTML
                string subject = "YMM HR System: Rewards Inventory Low Stock Alert";
                string templateName = "TemplateRewardInventoryLowStock"; // <- crear archivo en /Templates

                sendEmail.SendEmailTemplate(subject, templateName, new[,]
                {
            {"$WORKERID$", workerId},
            {"$REDEEMID$", redeemId.ToString()},
            {"$DATE$", DateTime.Now.ToString("dd/MM/yyyy HH:mm")},
            {"$ITEMS_TABLE$", sb.ToString()}
        }, sendEmail.GetAdminEmail(), contacts);
            }
            catch (Exception ex)
            {
                // Muy importante: NO romper el canje si falla el correo
                log.WriteToErrorLog("HR System", "Notify Low Stock After Redeem", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "NotifyLowStockAfterRedeem");
            }
        }

        public bool EnsureImprovementCredits(string workerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(workerId)) return false;

                string safeWorkerId = workerId.Trim().Replace("'", "''");

                // Idempotente: inserta solo si no existe
                string query = $@"
                    IF NOT EXISTS (SELECT 1 FROM ImprovementCredits WHERE WorkerId = '{safeWorkerId}')
                    BEGIN
                        INSERT INTO ImprovementCredits (WorkerId, Score)
                        VALUES ('{safeWorkerId}', 0);
                    END";

                oDatabase.ExecuteNonQuery(query, "Ensure ImprovementCredits");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Ensure ImprovementCredits", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "EnsureImprovementCredits");
                return false;
            }
        }
        #endregion
    }
}
