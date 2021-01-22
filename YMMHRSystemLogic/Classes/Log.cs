using DBFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class Log
    {
        /// <summary>
        /// Write Exceptions to Error Log
        /// </summary>
        /// <param name="app">Host Application</param>
        /// <param name="methodObjective">General Objective of the method</param>
        /// <param name="userId">User's ID</param>
        /// <param name="exceptionMessage">Exception's Message</param>
        /// <param name="stackTrace">Exception's Stack Trace</param>
        /// <param name="method"> Method Name</param>
        public void WriteToErrorLog(string app, string methodObjective, string userId, string exceptionMessage, string stackTrace, string method)
        {
            try
            {
                SQLTools sQLTools = new SQLTools();
                string userIdstring = userId == null || userId == "" ? "NULL" : "'" + userId + "'";
                string sqlString = "INSERT INTO ErrorLogs (Date,App,UserId,MethodObjective,ErrorMessage,StackTrace,Method) VALUES (GetDate(),'" + app + "'," + userIdstring + ",'" + methodObjective + "','" + exceptionMessage + "','" + stackTrace + "','" + method + "')";

                sQLTools.ExecuteNonQuery(sqlString, "Error Log Insert");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
