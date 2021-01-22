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
    public class FindingType
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the FindingType DTO
        /// </summary>
        /// <param name="findingTypeId"></param>
        /// <returns>DtoFindingType Loaded</returns>
        public DtoFindingType Load(long findingTypeId)
        {
            DtoFindingType FindingType = new DtoFindingType();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (findingTypeId != 0)
                {
                    mapping.Load<DtoFindingType>("FindingTypes", new DtoFindingType(), "FindingTypeId=" + findingTypeId);
                    FindingType = (DtoFindingType)mapping.dtoList.FirstOrDefault().Dto;
                }

                return FindingType;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load FindingType", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register FindingType 
        /// </summary>
        /// <param name="FindingType"></param>
        /// <returns>FindingType registered</returns>
        public void Save(DtoFindingType FindingType)
        {
            try
            {
                User user = new User();
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = FindingType, TableName = "FindingTypes" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save FindingType", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple FindingType with fields.
        /// </summary>
        /// <returns>Load FindingType Dto</returns>
        public List<DtoFindingType> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoFindingType> findingTypeList = new List<DtoFindingType>();

                mapping.Load<DtoFindingType>("SELECT FindingTypeId, Name, Points FROM FindingTypes ORDER BY FindingTypeId", "FindingType", new DtoFindingType());
                findingTypeList.AddRange(mapping.dtoList.Select(renglon => (DtoFindingType)renglon.Dto));

                return findingTypeList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple FindingType", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get FindingType Id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetFindingTypeId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FindingTypeId FROM FindingTypes WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get FindingTypeId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["FindingTypeId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get FindingType Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetFindingTypeId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a FindingType. 
        /// </summary>
        /// <param name="findingTypeId"></param>
        /// <returns></returns>
        public bool DeleteFindingType(long findingTypeId)
        {
            try
            {
                string query = "DELETE FindingTypes WHERE FindingTypeId =" + findingTypeId;
                oDatabase.ExecuteNonQuery(query, "Remove FindingType");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete FindingType", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteFindingType");
                return false;
            }

        }
        /// <summary>
        /// Get FindingType Points
        /// </summary>
        /// <param name="finding"></param>
        /// <returns></returns>
        public int GetFindingTypePoints(string finding)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Points FROM FindingTypes WHERE Name = '" + finding + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get Points");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow["Points"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Finding Type Points", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetFindingTypePoints");
                throw ex;
            }

        }
        #endregion
    }
}
