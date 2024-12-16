using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;

namespace YMMHRSystemLogic
{
    public class Motive
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        /// <summary>
        /// Guarda un registro en Motive.
        /// </summary>
        /// <param name="motiveObject"></param>
        /// <returns></returns>
        public bool SaveMotive(DtoMotive motive)
        {
            try
            {
                if (motive.IdMotive == 0)
                {
                    motive.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = motive, TableName = "Motive" });
                mapping.Save();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Motive", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveMotive");
                return false;
            }
        }

        /// <summary>
        /// Eliminar un registro en Motive.
        /// </summary>
        /// <param name="Motive"></param>
        /// <returns></returns>
        public bool DeleteMotive(long Idmotive)
        {
            try
            {
                string query = "DELETE FROM Motive WHERE IdMotive =" + Idmotive;
                oDatabase.ExecuteNonQuery(query, "Remove Motive");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Motive", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteMotive");
                return false;
            }
        }

        /// <summary>
        /// Obtener todos los registros.
        /// </summary>
        /// <returns>List of Motives</returns>
        public List<DtoMotive> GetMotives()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoMotive> motivesList = new List<DtoMotive>();

                mapping.Load<DtoMotive>("SELECT IdMotive, DescriptionMotive, UserCreated, DateAdded FROM Motive ORDER BY IdMotive", "DescriptionMotive", new DtoMotive());
                motivesList.AddRange(mapping.dtoList.Select(renglon => (DtoMotive)renglon.Dto));

                return motivesList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Motives", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMotives");
                throw ex;
            }
        }

        /// <summary>
        /// Loads the Motive DTO
        /// </summary>
        /// <param name="motiveId"></param>
        /// <returns>DtoType Loaded</returns>
        public DtoMotive Load(long motiveId)
        {
            DtoMotive motive = new DtoMotive();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (motiveId != 0)
                {
                    mapping.Load<DtoMotive>("Motive", new DtoMotive(), "IdMotive=" + motiveId);
                    motive = (DtoMotive) mapping.dtoList.FirstOrDefault().Dto;
                }

                return motive;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Motive", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }

        /// <summary>
        /// Get Type Id
        /// </summary>
        /// <param name="DescriptionMotive"></param>
        /// <returns>Id Type</returns>
        public long GetMotiveId(string motive)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT IdMotive FROM Motive WHERE DescriptionMotive = '" + motive + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get MotiveId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["MotiveId"]);
            }
                catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Motive Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMotiveId");
                throw ex;
            }

        }

        /// <summary>
        /// Get Type Description
        /// </summary>
        /// <param name="IdMotive"></param>
        /// <returns>Description Type</returns>
        public string GetMotiveDescription(long? IdMotive)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT DescriptionMotive FROM Motive WHERE IdMotive = " + IdMotive + "";
                dataRow = oDatabase.GetRow(sqlString, "Get DescriptionMotive");
                return dataRow["DescriptionMotive"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Motive Description", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMotiveId");
                throw ex;
            }
        }

        /// <summary>
        /// Register Motive 
        /// </summary>
        /// <param name="motiveObject"></param>
        /// <returns>Motive registered</returns>
        public void Save(DtoMotive motive)
        {
            try
            {
                User user = new User();

                if (motive.IdMotive == 0)
                {
                    motive.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = motive, TableName = "Motive" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Motive", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
    }
}
