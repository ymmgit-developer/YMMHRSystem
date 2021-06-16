using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class EmailNotification
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        SendEmail sendEmail = new SendEmail();

        #region Standard Methods
        /// <summary>
        /// Loads the EmailNotification DTO
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns>DtoEmailNotification Loaded</returns>
        public DtoEmailNotification Load(long emailNotificationId)
        {
            DtoEmailNotification emailNotification = new DtoEmailNotification();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (emailNotificationId != 0)
                {
                    mapping.Load<DtoEmailNotification>("EmailNotifications", new DtoEmailNotification(), "EmailNotificationId=" + emailNotificationId);
                    emailNotification = (DtoEmailNotification)mapping.dtoList.FirstOrDefault().Dto;
                }

                return emailNotification;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Email Notification", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Extra Diner Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetExtraDinerContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT ExtraDinerContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get ExtraDinerContacts");

                if (dataRow == null) return "";

                return dataRow["ExtraDinerContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Extra Diner Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetExtraDinerContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Extra Transport Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetExtraTransportContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT ExtraTransportContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get ExtraTransportContacts");

                if (dataRow == null) return "";

                return dataRow["ExtraTransportContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Extra Transport Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetExtraTransportContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Legal Requirement Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetLegalRequirementContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT LegalRequirementContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get LegalRequirementContacts");

                if (dataRow == null) return "";

                return dataRow["LegalRequirementContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Legal Requirement Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetLegalRequirementContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Vehicle Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetVehicleContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT VehicleContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get VehicleContacts");

                if (dataRow == null) return "";

                return dataRow["VehicleContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Vehicle Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetVehicleContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Utility Car Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetUtilityCarContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT UtilityCarContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get UtilityCarContacts");

                if (dataRow == null) return "";

                return dataRow["UtilityCarContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Utility Car Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetUtilityCarContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Translation Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetTranslationContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT TranslationContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get Translation Contacts");

                if (dataRow == null) return "";

                return dataRow["TranslationContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Translation Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTranslationContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Coffee Break Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetCoffeeBreakContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT CoffeeBreakContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get CoffeeBreakContacts");

                if (dataRow == null) return "";

                return dataRow["CoffeeBreakContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Coffee Break Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetCoffeeBreakContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Business Trip Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetBusinessTripContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT BusinessTripContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get BusinessTripContacts");

                if (dataRow == null) return "";

                return dataRow["BusinessTripContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Business Trip Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetBusinessTripContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Legal Affair Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetLegalAffairContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT LegalAffairContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get Legal Affair Contacts");

                if (dataRow == null) return "";

                return dataRow["LegalAffairContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Legal Affair Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetLegalAffairContacts");
                throw ex;
            }

        }
        /// <summary>
        /// Get Gift Contacts
        /// </summary>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public string GetGiftContacts(long emailNotificationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT GiftContacts FROM EmailNotifications WHERE EmailNotificationId = " + emailNotificationId;

                dataRow = oDatabase.GetRow(sqlString, "Get GiftContacts");

                if (dataRow == null) return "";

                return dataRow["GiftContacts"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Gift Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetGiftContacts");
                throw ex;
            }

        }

        /// <summary>
        /// Save Extra Diner Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveExtraDinerContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET ExtraDinerContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Extra Diner Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Extra Diner Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveExtraDinerContacts");
                return false;
            }

        }
        /// <summary>
        /// Save Extra Transport Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveExtraTransportContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET ExtraTransportContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Extra Transport Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Extra Diner Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveExtraDinerContacts");
                return false;
            }

        }
        /// <summary>
        /// Save  Legal Requirements Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveLegalRequirementsContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET LegalRequirementContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Legal Requirements Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Legal RequirementsContacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveLegalRequirementsContacts");
                return false;
            }

        }
        /// <summary>
        /// Save Vehicle Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveVehicleContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET VehicleContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Extra Diner Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Vehicle Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveVehicleContacts");
                return false;
            }

        }
        /// <summary>
        /// Save Utility Car Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveUtilityCarContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET UtilityCarContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Utility Car Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Utility Car Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveUtilityCarContacts");
                return false;
            }

        }
        /// <summary>
        /// Save Translation Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveTranslationContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET TranslationContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Translation Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Translation Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveTranslationContacts");
                return false;
            }

        }
        /// <summary>
        /// Save Coffee Break Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveCoffeeBreakContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET CoffeeBreakContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Coffee Break Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Coffee Break Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveCoffeeBreakContacts");
                return false;
            }

        }
        /// <summary>
        /// Save Business Trip Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveBusinessTripContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET BusinessTripContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Business Trip Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Business Trip Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveBusinessTripContacts");
                return false;
            }

        }
        /// <summary>
        /// Save Legal Affair Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveLegalAffairContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET LegalAffairContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Legal Affair Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Legal Affair Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveLegalAffairContacts");
                return false;
            }

        }
        /// <summary>
        /// Save Gift Contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="emailNotificationId"></param>
        /// <returns></returns>
        public bool SaveGiftContacts(string contacts, long emailNotificationId)
        {
            try
            {
                string query = "UPDATE EmailNotifications SET GiftContacts = '" + contacts + "' WHERE EmailNotificationId =" + emailNotificationId;
                oDatabase.ExecuteNonQuery(query, "Save Gift Contacts");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Gift Contacts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveGiftContacts");
                return false;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void SendAnniversaryGiftEmail()
        {
            GiftAssociate giftAssociate = new GiftAssociate();
            List<DtoGiftAssociate> giftAssociatesList = giftAssociate.LoadMultiple();
            foreach (var item in giftAssociatesList)
            {
                if (!item.EmailSent)
                {
                    if (DateTime.ParseExact(item.Month, "MMMM", CultureInfo.InvariantCulture).Month == DateTime.Now.Month && item.Day == DateTime.Now.Day)
                    {
                        sendEmail.SendEmailTemplate("YMM HR System: Anniversary Gift", "TemplateGiftAnniversary", new[,]
                        {
                        {"$ASSOCIATE$", item.Associate},
                        {"$ANNIVERSARY$", item.Day + " " + item.Month}
                    }, sendEmail.GetAdminEmail(), GetGiftContacts(1).Split(',').ToList());
                        giftAssociate.UpdateEmailFlag(true, item.GiftAssociateId);
                    }
                }
                else
                {
                    if (DateTime.ParseExact(item.Month, "MMMM", CultureInfo.InvariantCulture).Month < DateTime.Now.Month && item.Day < DateTime.Now.Day)
                    {
                        giftAssociate.UpdateEmailFlag(false, item.GiftAssociateId);
                    }
                }

            }
        }
        /// <summary>
        /// Send Vehicle Email
        /// </summary>
        public void SendVehicleEmail()
        {
            Vehicle vehicle = new Vehicle();
            List<DtoVehicle> vehicleList = vehicle.LoadMultiple();

            foreach (var item in vehicleList)
            {
                if (!item.VerificationEmailSent)
                {
                    if (item.NextVerification?.Subtract(DateTime.Now).Days == 10)
                    {
                        sendEmail.SendEmailTemplate("YMM HR System: Vehicle Verification", "TemplateVehicleVerification", new[,]
                        {
                        {"$MODEL$", item.Model},
                        {"$YEAR$", item.Year},
                        {"$LICENSE_PLATE$", item.LicensePlate },
                        {"$DATE$", item.NextVerification?.ToString("dd/MM/yyyy") }
                    }, sendEmail.GetAdminEmail(), GetVehicleContacts(1).Split(',').ToList());
                        vehicle.UpdateVerificationEmailFlag(true, item.VehicleId);
                    }
                }
                else
                {
                    if (item.NextVerification < DateTime.Now)
                    {
                        vehicle.UpdateVerificationEmailFlag(false, item.VehicleId);
                    }
                }
            }

            foreach (var item in vehicleList)
            {
                if (!item.ServiceEmailSent)
                {
                    if (item.NextService?.Subtract(DateTime.Now).Days == 10)
                    {
                        sendEmail.SendEmailTemplate("YMM HR System: Vehicle Service", "TemplateVehicleService", new[,]
                        {
                        {"$MODEL$", item.Model},
                        {"$YEAR$", item.Year},
                        {"$LICENSE_PLATE$", item.LicensePlate },
                        {"$DATE$", item.NextService?.ToString("dd/MM/yyyy") }
                    }, sendEmail.GetAdminEmail(), GetVehicleContacts(1).Split(',').ToList());
                        vehicle.UpdateServiceEmailFlag(true, item.VehicleId);
                    }
                }
                else
                {
                    if (item.NextService < DateTime.Now)
                    {
                        vehicle.UpdateServiceEmailFlag(false, item.VehicleId);
                    }

                }
            }
        }
        /// <summary>
        /// Send Vehicle Email
        /// </summary>
        public void SendLegalRequirementEmail()
        {
            LegalRequirement legalRequirement = new LegalRequirement();
            List<DtoLegalRequirement> legalRequirementList = legalRequirement.LoadMultiple();

            foreach (var item in legalRequirementList)
            {
                if (!item.RenewalEmailSent)
                {
                    if (item.RenewalDate?.Subtract(DateTime.Now).Days == 15)
                    {
                        sendEmail.SendEmailTemplate("YMM HR System: Legal Requirement Renewal", "TemplateLegalRequirementRenewal", new[,]
                        {
                        {"$PROCEDURE$", item.LegalProcedure},
                        {"$RENEWAL$", item.RenewalDate?.ToString("dd/MM/yyyy") },
                        {"$EXPIRATION$", item.ExpirationDate?.ToString("dd/MM/yyyy") }
                    }, sendEmail.GetAdminEmail(), GetLegalRequirementContacts(1).Split(',').ToList());
                        legalRequirement.UpdateRenewalEmailFlag(true, item.LegalRequirementId);
                    }
                }
                else
                {
                    if (item.RenewalDate < DateTime.Now)
                    {
                        legalRequirement.UpdateRenewalEmailFlag(false, item.LegalRequirementId);
                    }
                }

            }

            foreach (var item in legalRequirementList)
            {
                if (!item.ExpirationEmailSent)
                {
                    if (item.ExpirationDate?.Subtract(DateTime.Now).Days == 15)
                    {
                        sendEmail.SendEmailTemplate("YMM HR System: Legal Requirement Expiration", "TemplateLegalRequirementExpiration", new[,]
                        {
                        {"$PROCEDURE$", item.LegalProcedure},
                        {"$RENEWAL$", item.RenewalDate?.ToString("dd/MM/yyyy") },
                        {"$EXPIRATION$", item.ExpirationDate?.ToString("dd/MM/yyyy") }
                    }, sendEmail.GetAdminEmail(), GetLegalRequirementContacts(1).Split(',').ToList());
                        legalRequirement.UpdateExpirationEmailFlag(true, item.LegalRequirementId);
                    }
                }
                else
                {
                    if (item.ExpirationDate < DateTime.Now)
                    {
                        legalRequirement.UpdateExpirationEmailFlag(false, item.LegalRequirementId);
                    }
                }

            }
        }

        #endregion
    }
}
