using DBFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMMHRSystemLogic.DTO;

namespace YMMHRSystemLogic
{
    public class Group
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        #endregion

        #region General Methods
        public List<DtoGroup> GetGroupsList()
        {
            try
            {
                string query = "SELECT GroupId, GroupName, IsActive FROM Groups WHERE IsActive = 1 ORDER BY GroupName;";
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.Load<DtoGroup>(query, "Groups", new DtoGroup());
                return mapping.dtoList.Select(r => (DtoGroup)r.Dto).ToList();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Groups", SQLTools.userId.ToString(),
                    ex.Message, ex.StackTrace, "GetGroupsList");
                throw;
            }
        }
        #endregion
    }
}
