using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class FoodType
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the FoodType DTO
        /// </summary>
        /// <param name="foodTypeId"></param>
        /// <returns>DtoFoodType Loaded</returns>
        public DtoFoodType Load(long foodTypeId)
        {
            DtoFoodType foodType = new DtoFoodType();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (foodTypeId != 0)
                {
                    mapping.Load<DtoFoodType>("FoodTypes", new DtoFoodType(), "FoodTypeId=" + foodTypeId);
                    foodType = (DtoFoodType)mapping.dtoList.FirstOrDefault().Dto;
                }

                return foodType;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load FoodType", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register FoodType 
        /// </summary>
        /// <param name="foodType"></param>
        /// <returns>FoodType registered</returns>
        public void Save(DtoFoodType foodType)
        {
            try
            {
                User user = new User();

                if (foodType.FoodTypeId == 0)
                {
                    foodType.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    foodType.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = foodType, TableName = "FoodTypes" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save FoodType", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple FoodType with fields.
        /// </summary>
        /// <returns>Load FoodType Dto</returns>
        public List<DtoFoodType> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoFoodType> shiftList = new List<DtoFoodType>();

                mapping.Load<DtoFoodType>("SELECT FoodTypeId, Name, Code, Cost, UserCreated, DateAdded FROM FoodTypes ORDER BY FoodTypeId", "FoodTypes", new DtoFoodType());
                shiftList.AddRange(mapping.dtoList.Select(renglon => (DtoFoodType)renglon.Dto));

                return shiftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple FoodTypes", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get FoodType Id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetFoodTypeId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FoodTypeId FROM FoodTypes WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get FoodTypeId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["FoodTypeId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get FoodType Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetFoodTypeId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a FoodType. 
        /// </summary>
        /// <param name="foodTypeId"></param>
        /// <returns></returns>
        public bool DeleteFoodType(long foodTypeId)
        {
            try
            {
                string query = "DELETE FoodTypes WHERE FoodTypeId =" + foodTypeId;
                oDatabase.ExecuteNonQuery(query, "Remove FoodType");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete FoodType", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteFoodType");
                return false;
            }

        }
        #endregion
    }
}
