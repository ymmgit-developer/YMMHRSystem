using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMMHRSystemLogic;

namespace YMMHRSystemLogic
{
    public class GiftAssociate
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Save multiple Gift Associates
        /// </summary>
        /// <param name="giftAssociates"></param>
        public void SaveMultiple(List<DtoGiftAssociate> giftAssociates)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                giftAssociates.ForEach(item =>
                {
                    mapping.dtoList.Add(new DBFrameworkDto() { Dto = item, TableName = "GiftAssociates" });
                });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Multiple Gift Associates", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple GiftAssociate with fields.
        /// </summary>
        /// <returns>Load GiftAssociate Dto</returns>
        public List<DtoGiftAssociate> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGiftAssociate> giftAssociateList = new List<DtoGiftAssociate>();

                mapping.Load<DtoGiftAssociate>("SELECT GiftAssociateId, WorkerFileId, Associate, Process, Month, Day, EmailSent FROM GiftAssociates ORDER BY GiftAssociateId", "GiftAssociate", new DtoGiftAssociate());
                giftAssociateList.AddRange(mapping.dtoList.Select(renglon => (DtoGiftAssociate)renglon.Dto));

                return giftAssociateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple GiftAssociate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }      
        #endregion

        #region General Methods   
        /// <summary>
        /// Get GiftAssociate Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetGiftAssociateId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT GiftAssociateId FROM GiftAssociates WHERE Associate = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get GiftAssociateId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["GiftAssociateId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get GiftAssociate Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetGiftAssociateId");
                throw ex;
            }

        }
        /// <summary>
        /// Get Gift Associate Name
        /// </summary>
        /// <param name="giftAssociateId"></param>
        /// <returns></returns>
        public string GetGiftAssociateName(long giftAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Associate FROM GiftAssociates WHERE GiftAssociateId = " + giftAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get Gift Associate Names");

                if (dataRow == null) return "";

                return dataRow["Associate"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Gift Associate Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetGiftAssociateName");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a GiftAssociate. 
        /// </summary>
        /// <param name="giftAssociateId"></param>
        /// <returns></returns>
        public bool DeleteGiftAssociate(long giftAssociateId)
        {
            try
            {
                string query = "DELETE GiftAssociates WHERE GiftAssociateId =" + giftAssociateId;
                oDatabase.ExecuteNonQuery(query, "Remove GiftAssociate");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Gift Associate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteGiftAssociate");
                return false;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoWorkers"></param>
        public void DeleteInactiveWorkers(List<DtoWorkerFile> dtoWorkers)
        {
            try
            {
                List<string> deleteList = new List<string>();
                foreach (var item in dtoWorkers)
                {
                    deleteList.Add("DELETE GiftAssociates WHERE WorkerFileId =" + item.WorkerFileId);
                }
                oDatabase.ExecuteMultipleNonQueryList(deleteList, "Remove Inactive workers");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Inactive Workers", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteInactiveWorkers");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void AddGiftAssociates()
        {
            try
            {
                WorkerFile workerFile = new WorkerFile();
                List<DtoWorkerFile> workers = workerFile.LoadMultiple(1);
                List<DtoGiftAssociate> actualGiftWorkers = LoadMultiple();
                List<DtoGiftAssociate> giftWorkers = new List<DtoGiftAssociate>();

                if (actualGiftWorkers.Count > 0)
                {
                    List<DtoWorkerFile> newWorkers = workers.Where(x => !actualGiftWorkers.Any(y => y.WorkerFileId == x.WorkerFileId)).ToList();
                    foreach (var item in newWorkers)
                    {
                        giftWorkers.Add(new DtoGiftAssociate
                        {
                            Associate = item.Names,
                            Process = item.Process,
                            WorkerFileId = item.WorkerFileId,
                            Month = item.AdmissionDate?.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture),
                            Day = Convert.ToInt32(item.AdmissionDate?.Day),
                            EmailSent = false
                        });
                    }
                }
                else
                {
                    foreach (var item in workers)
                    {
                        giftWorkers.Add(new DtoGiftAssociate
                        {
                            Associate = item.Names,
                            Process = item.Process,
                            WorkerFileId = item.WorkerFileId,
                            Month = item.AdmissionDate?.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture),
                            Day = Convert.ToInt32(item.AdmissionDate?.Day),
                            EmailSent = false
                        });
                    }
                }
                if (giftWorkers.Count > 0)
                {
                    SaveMultiple(giftWorkers);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void DeleteInactiveGiftAssociates()
        {
            WorkerFile workerFile = new WorkerFile();
            List<DtoWorkerFile> inactiveWorkers = workerFile.LoadMultiple(0);
            List<DtoGiftAssociate> actualGiftWorkers = LoadMultiple();

            if (actualGiftWorkers.Count > 0)
            {
                List<DtoWorkerFile> inactiveAssociates = inactiveWorkers.Where(x => actualGiftWorkers.Any(y => y.WorkerFileId == x.WorkerFileId) && x.Status == false).ToList();
                DeleteInactiveWorkers(inactiveAssociates);
            }
        }
        /// <summary>
        /// Updates the email notification flag
        /// </summary>
        /// <param name="flag"></param>
        public void UpdateEmailFlag(bool flag, long giftAssociateId)
        {
            try
            {
                string query = "UPDATE GiftAssociates SET EmailSent = " + (flag ? "1" : "0") + " WHERE GiftAssociateId =" + giftAssociateId;
                oDatabase.ExecuteNonQuery(query, "Update Email Sent GiftAssociate");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Email Flag", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "UpdateEmailFlag");
            }
        }
        #endregion
    }
}
