using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;

namespace YMMHRSystemLogic
{
    public class EntryExitAuthorization
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        #region Standard Methods
        /// <summary>
        /// Registers an entry/exit authorization record in the database.
        /// </summary>
        /// <param name="inputoutput">
        /// An instance of <see cref="DtoEntryExitAuthorization"/> containing the information 
        /// for the entry/exit authorization to be saved. If <c>IdRecordsInOut</c> is zero, 
        /// the function assumes this is a new record and assigns the current date and time 
        /// to <c>CreateDate</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the record was saved successfully; otherwise, <c>false</c>.
        /// </returns>
        public bool Save(DtoEntryExitAuthorization inputoutput)
        {
            try
            {
                User user = new User();
                if (inputoutput.IdRecordsInOut == 0)
                {
                    inputoutput.CreateDate = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = inputoutput, TableName = "EntryExitAuthorization" });

                mapping.Save();
                return true;
            }
            catch (Exception ex) 
            {
                log.WriteToErrorLog("HR System", "Save Register In-Out", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }

        public bool Update(DtoEntryExitAuthorization inputoutput)
        {
            try
            {
                string formattedDateFor = inputoutput.DateFor.ToString("yyyy/MM/dd");
                string query = "UPDATE EntryExitAuthorization SET Associate = '" + @inputoutput.Associate + "'" +
                                ", Process = '" + @inputoutput.Process + "', DateFor = '" + formattedDateFor + "', TimeFor = '" + inputoutput.TimeFor +
                                "', IdType = " + @inputoutput.IdType + ", IdSalary = " + @inputoutput.IdSalary + ", IdMotive = " + @inputoutput.IdMotive +
                                ", CurrentState = " + 0 + " WHERE IdRecordsInOut = " +@inputoutput.IdRecordsInOut;
                oDatabase.ExecuteNonQuery(query, "Update Register In-Out");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Register In-Out", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Update");
                return false;
            }
        }

        /// <summary>
        /// Deletes an entry/exit authorization record from the database based on its identifier.
        /// </summary>
        /// <param name="recordId">
        /// The unique identifier of the entry/exit authorization record to be deleted from 
        /// the <c>EntryExitAuthorization</c> table.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the deletion was successful; otherwise, <c>false</c> in 
        /// case of an error.
        /// </returns>
        public bool Delete(long recordId)
        {
            try
            {
                string query = "DELETE EntryExitAuthorization WHERE IdRecordsInOut =" + @recordId;
                oDatabase.ExecuteNonQuery(query, "Remove Register In-Out");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Register In-Out", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Delete");
                return false;
            }
        }

        /// <summary>
        /// Load multiple records based on the specified option.
        /// </summary>
        /// <param name="option">
        /// Defines the type of records to load:
        /// 0 - Records pending first approval, (Aprobadores 1ra lista)
        /// 1 - Records pending second approval, (Aprobadores 2da lista)
        /// 2 - Records created by the specified user, (Usuarios sin permiso de aprobar)
        /// 3 - Authorized records for today. (Vigilancia)
        /// 4 - All records of the week (RH)
        /// </param>
        /// <param name="userid">
        /// Optional parameter. Specifies the user ID for filtering records created by the user (used with option 2).
        /// </param>
        /// <returns>Returns a list of <see cref="DtoEntryExitAuthorization"/> records.</returns>
        public List<DtoEntryExitAuthorization>GetRecords(int option, long userid = 0)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoEntryExitAuthorization> listrecords = new List<DtoEntryExitAuthorization>();
                switch (option)
                {
                    case 0://Records pending first approval
                        mapping.Load<DtoEntryExitAuthorization>("SELECT * FROM EntryExitAuthorization WHERE FirtsAuthorization = 0 ORDER BY DateFor DESC", "EntryExitAuthorization", new DtoEntryExitAuthorization());
                        break;
                    case 1: //Records pending second approval
                        mapping.Load<DtoEntryExitAuthorization>("SELECT * FROM EntryExitAuthorization WHERE SecondAuthorization = 0 ORDER BY DateFor DESC", "EntryExitAuthorization", new DtoEntryExitAuthorization());
                        break;
                    case 2: //Records created by the user
                        mapping.Load<DtoEntryExitAuthorization>("SELECT * FROM EntryExitAuthorization WHERE CreateBy = @userid ORDER BY DateFor DESC", "EntryExitAuthorization", new DtoEntryExitAuthorization());
                        break;
                    case 3: //Authorized records
                        mapping.Load<DtoEntryExitAuthorization>("SELECT * FROM EntryExitAuthorization WHERE CurrentState = 2 AND DateFor = CONVERT(date, GETDATE()) ORDER BY DateFor DESC", "EntryExitAuthorization", new DtoEntryExitAuthorization());
                        break;
                    case 4: //All records of the week
                        mapping.Load<DtoEntryExitAuthorization>("SELECT * FROM EntryExitAuthorization WHERE DATEPART(WEEK, DateFor) = DATEPART(WEEK, GETDATE()) AND DATEPART(YEAR, DateFor) = DATEPART(YEAR, GETDATE()) ORDER BY DateFor DESC;", "EntryExitAuthorization", new DtoEntryExitAuthorization());
                        break;  
                    default:
                        new List<DtoEntryExitAuthorization>();
                        break;          
                }
                listrecords.AddRange(mapping.dtoList.Select(row => (DtoEntryExitAuthorization)row.Dto));
                return listrecords;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Records In-Out", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetRecords");
                throw ex;
            }
        }

        public DtoEntryExitAuthorization GetRecord(long IdRecordsInOut) 
        {
            DBFrameworkMapping mapping = new DBFrameworkMapping();
            DtoEntryExitAuthorization record = new DtoEntryExitAuthorization();
            mapping.Load<DtoEntryExitAuthorization>("EntryExitAuthorization", new DtoEntryExitAuthorization(), "IdRecordsInOut = " + IdRecordsInOut);
            record = (DtoEntryExitAuthorization)mapping.dtoList.First().Dto;
            return record;
        }

        public DtoEntryExitAuthorization DetailCheckInOut(int IdRecordsInOut)
        {
            DtoEntryExitAuthorization record = new DtoEntryExitAuthorization();
            try
            {
                if (IdRecordsInOut != 0) 
                {
                    DBFrameworkMapping mapping = new DBFrameworkMapping();
                    mapping.Load<DtoEntryExitAuthorization>("EntryExitAuthorization", new DtoEntryExitAuthorization(), "IdRecordsInOut=" + IdRecordsInOut);
                    if (mapping.dtoList.Any()) // Verifica si se encontró algún registro
                    {
                        record = (DtoEntryExitAuthorization)mapping.dtoList.First().Dto; // Obtiene el primer registro
                    }
                }
                return record;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Unique Record In-Out", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DetailCheckInOut");
                throw ex;
            }
        }

        #endregion

        #region Process Methods
        /// <summary>
        /// Approves a record based on the specified option.
        /// </summary>
        /// <param name="option">
        /// Defines the type of records to load:
        /// 1 - First approval,
        /// 2 - Second approval,
        /// </param>
        /// <param name="userId">
        /// Specifies the user ID for filtering records created by the user.
        /// </param>
        /// <param name="recordId">
        /// Specifies the ID of the record to approve.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if approval was successful; otherwise, <c>false</c> in case of an error.
        /// </returns>
        public bool ApproveRecords(int option, string userName, long recordId)
        {
            try
            {
                string query;
                switch (option)
                {
                    case 1:
                        query = "UPDATE EntryExitAuthorization SET FirtsAuthorization = 1, FirtsAuthorizedby = '"+ @userName + "' WHERE IdRecordsInOut = " + @recordId;
                        oDatabase.ExecuteNonQuery(query, "Update Register (ApprveOne) In-Out");
                        break;
                    case 2:
                        query = "UPDATE EntryExitAuthorization SET SecondAuthorization = 1, SecondAuthorizedby = '" + @userName + "' WHERE IdRecordsInOut = " + @recordId;
                        oDatabase.ExecuteNonQuery(query, "Update Register (ApprveTwo) In-Out");
                        break;
                    default:
                        return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                log.WriteToErrorLog("HR System", "Approve Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ApproveRecords");
                return false;
            } 
        }

        /// <summary>
        /// Change the status of a record.
        /// </summary>
        /// <param name="option">
        /// Defines the type of records to load:
        /// 0 - creado 
        /// 1 - aprobado por primera vez,
        /// 2 - aprobrado en su totalidad,
        /// </param>
        /// <param name="recordId">
        /// Specifies the ID of the record to change.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if change was successful; otherwise, <c>false</c> in case of an error.
        /// </returns>
        public bool ChangeCurrentStatus(int option, long recordId)
        {
            try
            {
                string query = "UPDATE EntryExitAuthorization SET CurrentState = " + option +  " WHERE IdRecordsInOut = " + recordId;
                oDatabase.ExecuteNonQuery(query, "Update Status Register In-Out");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Status", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ChangeCurrentStatus");
                return false;
            } 
        }

        public List<DtoEntryExitAuthorization>LoadMultipleWithFilter(string startDate, string endDate) 
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoEntryExitAuthorization> dtoEntryExitAuthorizationsList = new List<DtoEntryExitAuthorization>();

                mapping.Load<DtoEntryExitAuthorization>("SELECT IdRecordsInOut, Associate, Process, DateFor, TimeFor, " +
                                                        "IdType, IdSalary, IdMotive, " +
                                                        "FirtsAuthorization, FirtsAuthorizedby, SecondAuthorization, SecondAuthorizedby, " +
                                                        "CurrentState, CreateBy, CreateDate " +
                                                        "FROM EntryExitAuthorization " +
                                                        "WHERE DateFor BETWEEN '"+ startDate + "' AND '" + endDate + "' " +
                                                        "ORDER BY IdRecordsInOut DESC;", "EntryExitAuthorization", new DtoEntryExitAuthorization());
                dtoEntryExitAuthorizationsList.AddRange(mapping.dtoList.Select(renglon => (DtoEntryExitAuthorization)renglon.Dto));
                return dtoEntryExitAuthorizationsList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Input/Output Records With Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleWithFilter");
                throw ex;
            }
        }
        #endregion
    }
}
