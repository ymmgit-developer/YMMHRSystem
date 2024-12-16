using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;

namespace YMMHRSystemLogic
{
    public class Types
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        /// <summary>
        /// Guarda un registro en Type.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public bool SaveType(DtoType type)
        {
            try
            {
                if (type.IdType == 0)
                {
                    type.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = type, TableName = "Type" });
                mapping.Save();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Type", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveType");
                return false;
            }
        }

        /// <summary>
        /// Eliminar un registro en Type.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public bool DeleteType(long Idtype)
        {
            try
            {
                string query = "DELETE FROM Type WHERE IdType =" + Idtype;
                oDatabase.ExecuteNonQuery(query, "Remove Type");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Type", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteType");
                return false;
            }
        }

        /// <summary>
        /// Obtener todos los registros.
        /// </summary>
        /// <returns>List of Types</returns>
        public List<DtoType> GetTypes()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoType> typesList = new List<DtoType>();

                mapping.Load<DtoType>("SELECT IdType, DescriptionType, UserCreated, DateAdded FROM Type ORDER BY IdType", "Type", new DtoType());
                typesList.AddRange(mapping.dtoList.Select(renglon => (DtoType)renglon.Dto));

                return typesList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Types", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTypes");
                throw ex;
            }
        }

        /// <summary>
        /// Loads the Type DTO
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns>DtoType Loaded</returns>
        public DtoType Load(long typeId)
        {
            DtoType type = new DtoType();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (typeId != 0)
                {
                    mapping.Load<DtoType>("Type", new DtoType(), "IdType=" + typeId);
                    type = (DtoType)mapping.dtoList.FirstOrDefault().Dto;
                }

                return type;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Type", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }

        /// <summary>
        /// Get Type Id
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Id Type</returns>
        public long GetTypeId(string type)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT IdType FROM Type WHERE DescriptionType = '" + type + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get TypeId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["TypeId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Type Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTypeId");
                throw ex;
            }

        }

        /// <summary>
        /// Get Type Description
        /// </summary>
        /// <param name="IdType"></param>
        /// <returns>Description Type</returns>
        public string GetTypeDescription(long? IdType)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT DescriptionType FROM Type WHERE IdType = " + IdType + "";
                dataRow = oDatabase.GetRow(sqlString, "Get TypeId");
                return dataRow["DescriptionType"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Type Description", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTypeDescription");
                throw ex;
            }
        }

        /// <summary>
        /// Register Type 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Type registered</returns>
        public void Save(DtoType type)
        {
            try
            {
                User user = new User();

                if (type.IdType == 0)
                {
                    type.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = type, TableName = "Type" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Type", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
    }
}
