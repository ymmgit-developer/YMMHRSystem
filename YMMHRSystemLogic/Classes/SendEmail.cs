using DBFramework;
using static EmailComponent.Email;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailComponent;
using System.Web;
using System.Text.RegularExpressions;

namespace YMMHRSystemLogic
{
    public class SendEmail
    {
        Log log = new Log();
        Email email = new Email();
        SQLTools sqlTools = new SQLTools();

        /// <summary>
        /// Send a simple plain email, with options of adding attachments and hidden copies.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emailRecipientsTo">List of visible email addresses</param>
        /// <param name="issue"></param>
        /// <param name="emailRecipientsBcc">List of email addresses for hidden copies.</param>
        /// <param name="attachments"></param>
        /// <returns>true o false</returns>
        public bool SendSimpleEmail(string message, string issue, string emailSender = "", List<string> emailRecipientsTo = null, List<string> emailRecipientsBcc = null, string[] attachments = null)
        {
            try
            {
                SMTPConfiguration(1);

                if (!emailSender.Equals(""))
                {
                    email.SmtpSvr.EmailSender = emailSender;
                }
                email.SmtpMail.Subject = issue;
                email.SmtpMail.EmailBody = message;

                if (attachments != null)
                {
                    email.SmtpMail.Attachments = attachments;
                }
                if (emailRecipientsTo != null)
                {
                    email.SmtpMail.VisibleEmailTo = emailRecipientsTo;
                }
                if (emailRecipientsBcc != null)
                {
                    email.SmtpMail.HiddenEmailTo = emailRecipientsBcc;
                }

                email.SendEmail();
                return email.ErrorMessage.Equals("");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("YMM HR System", "Send Simple Email", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SendSimpleEmail");
                throw ex;
            }

        }
        /// <summary>
        /// Send an Email by using a html templateplantilla HTML, with options of adding attachments and hidden copies.
        /// </summary>
        /// <param name="emailRecipientsTo"></param>
        /// <param name="templateName">array of template variables</param>
        /// <param name="templateFields"></param>
        /// <param name="issue"></param>
        /// <param name="emailRecipientsBcc"></param>
        /// <param name="attachments"></param>
        /// <returns>true o false</returns>
        public bool SendEmailTemplate(string issue, string templateName, string[,] templateFields, string emailSender = "", List<string> emailRecipientsTo = null, List<string> emailRecipientsBcc = null, string[] attachments = null)
        {
            try
            {
                SMTPConfiguration(1);

                email.SmtpMail.Subject = issue;
                email.SmtpMail.HTMLEmail = true;
                if (!emailSender.Equals(""))
                {
                    email.SmtpSvr.EmailSender = emailSender;
                }
                //string templatePath = Environment.CurrentDirectory + "\\Templates\\" + templateName + ".html";
                string templatePath = HttpContext.Current.Server.MapPath("~\\Templates\\" + templateName + ".html");
                email.SmtpMail.EmailBody = LoadTemplate(templateFields, templatePath);
                if (attachments != null)
                {
                    email.SmtpMail.Attachments = attachments;
                }
                if (emailRecipientsTo != null)
                {
                    email.SmtpMail.VisibleEmailTo = emailRecipientsTo;
                }
                if (emailRecipientsBcc != null)
                {
                    email.SmtpMail.HiddenEmailTo = emailRecipientsBcc;
                }
                email.SendEmail();
                return email.ErrorMessage.Equals("");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("YMM HR System", "Send Email Template", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SendEmailTemplate");
                throw ex;
            }
        }
        /// <summary>
        /// Gets the Email-SMTP Configuration.
        /// </summary>
        /// <param name="EmailConfigurationId"></param>
        private void SMTPConfiguration(long EmailConfigurationId)
        {
            try
            {
                string sqlString = "SELECT * FROM EmailConfiguration WHERE EmailConfigurationId=" + EmailConfigurationId;

                DataTable table = sqlTools.GetTable(sqlString, "");
                table.TableName = "EmailConfiguration";

                if (table.Rows.Count > 0)
                {
                    //Validate SMTP User
                    if (!Convert.IsDBNull(table.Rows[0]["SMTPUser"]) && !(table.Rows[0]["SMTPUser"].Equals("")))
                    {
                        email.SmtpSvr.User = table.Rows[0]["SMTPUser"].ToString();
                    }
                    else
                    {
                        throw new Exception("The column " + table.Columns["SMTPUser"].ColumnName +
                                            " does not exist in the " + table.TableName +
                                            " table where the Email Configuration Id was: " +
                                            EmailConfigurationId);
                    }
                    //Validate SMTP Password
                    if (!Convert.IsDBNull(table.Rows[0]["SMTPPassword"]) && !(table.Rows[0]["SMTPPassword"].Equals("")))
                    {
                        email.SmtpSvr.Password = table.Rows[0]["SMTPPassword"].ToString();
                    }
                    else
                    {
                        throw new Exception("The column " + table.Columns["SMTPPassword"].ColumnName +
                                            " does not exist in the " + table.TableName +
                                            " table where the Email Configuration Id was: " +
                                            EmailConfigurationId);
                    }

                    //Validate Email Sender
                    if (!Convert.IsDBNull(table.Rows[0]["EmailSender"]) && !(table.Rows[0]["EmailSender"].Equals("")))
                    {
                        email.SmtpSvr.EmailSender = table.Rows[0]["EmailSender"].ToString();
                    }
                    else
                    {
                        throw new Exception("The column " + table.Columns["EmailSender"].ColumnName +
                                            " does not exist in the " + table.TableName +
                                            " table where the Email Configuration Id was: " +
                                            EmailConfigurationId);
                    }

                    //Validate EmailSignature
                    if (!Convert.IsDBNull(table.Rows[0]["EmailSignature"]) && !(table.Rows[0]["EmailSignature"].Equals("")))
                    {
                        email.SmtpSvr.EmailSignature = table.Rows[0]["EmailSignature"].ToString();
                    }
                    else
                    {
                        throw new Exception("The column " + table.Columns["EmailSignature"].ColumnName +
                                            " does not exist in the " + table.TableName +
                                            " table where the Email Configuration Id was: " +
                                            EmailConfigurationId);
                    }

                    //Validate Host
                    if (!Convert.IsDBNull(table.Rows[0]["SMTPHost"]) && !(table.Rows[0]["SMTPHost"].Equals("")))
                    {
                        email.SmtpSvr.Host = table.Rows[0]["SMTPHost"].ToString();
                    }
                    else
                    {
                        throw new Exception("The column " + table.Columns["SMTPHost"].ColumnName +
                                            " does not exist in the " + table.TableName +
                                            " table where the Email Configuration Id was: " +
                                            EmailConfigurationId);
                    }

                    //Validate Puerto
                    if (!Convert.IsDBNull(table.Rows[0]["SMTPPort"]) && !(table.Rows[0]["SMTPPort"].Equals("")))
                    {
                        email.SmtpSvr.Port = Convert.ToInt32(table.Rows[0]["SMTPPort"]);
                    }
                    else
                    {
                        throw new Exception("The column " + table.Columns["SMTPPort"].ColumnName +
                                            " does not exist in the " + table.TableName +
                                            " table where the Email Configuration Id was: " +
                                            EmailConfigurationId);
                    }

                    //Validate SSL
                    if (!Convert.IsDBNull(table.Rows[0]["SSL"]) && !(table.Rows[0]["SSL"].Equals("")))
                    {
                        email.SmtpSvr.SSL = (bool)table.Rows[0]["SSL"];
                    }
                    else
                    {
                        throw new Exception("The column " + table.Columns["SSL"].ColumnName +
                                            " does not exist in the " + table.TableName +
                                            " table where the Email Configuration Id was: " +
                                            EmailConfigurationId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "SMTP Configuration", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SMTPConfiguration");
                throw ex;
            }
        }
        /// <summary>
        /// Gets Admin Email
        /// </summary>
        /// <returns></returns>
        public string GetAdminEmail()
        {
            try
            {
                DataRow row = null;
                string sqlQuery = "SELECT AdminEmail FROM EmailConfiguration WHERE EmailConfigurationId = 1";
                row = sqlTools.GetRow(sqlQuery, "Get Admin Email");

                if (row == null || row["AdminEmail"].ToString() == "") return "";

                return row["AdminEmail"].ToString();

            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Admin Email", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetAdminEmail");
                throw ex;
            }
        }

        public Boolean CheckStructureMail(List<string> ListMail)
        {
            try
            {
                // Expresión regular para validar el formato del correo
                string pattern = @"^[a-zA-Z]+\.[a-zA-Z]+@motherson\.com$";
                Regex regex = new Regex(pattern);

                foreach (string email in ListMail)
                {
                    if (!regex.IsMatch(email))
                    {
                        // Si algún correo no cumple con el formato, retornamos false
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Check Structure Mail", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "CheckStructureMail");
                return false;
            }
        }

    }
}
