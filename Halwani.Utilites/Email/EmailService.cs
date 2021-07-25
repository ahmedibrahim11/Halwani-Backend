using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Halwani.Utilites.Email
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

//        string resetPasswordUrl = _configuration["ResetPasswordUrl:ResetPassUrl"] + "/" + appUser.Id + "/" + token;

//        Dictionary<string, string> Variables = new Dictionary<string, string>
//                            {
//                                { "[UserName]", model.FirstName + " " + model.FatherName + " " + model.GrandFatherName + " " +              model.FamilyName},
//                                { "[URL]", resetPasswordUrl }
//                            };
//                try
//                {
//                    _emailService.SendEmail(new EmailContentModel
//                    {
//                        Body = "",
//                        subject = "Email Confirmed",
//                        ToList = appUser.Email,
//                        HtmlFilePath = "confirmEmail.html",
//                        Variables = Variables
//    });
//                }
//                catch (Exception ex)
//{
//    //RepositoryHelper.LogException(ex);
//}
public bool SendEmail(EmailContentModel emailContent)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                string[] ToArr = (emailContent != null && !string.IsNullOrEmpty(emailContent.ToList)) ? emailContent.ToList.Split(',') : null;
                string senderFrom = _configuration["EmailSettings:Sender"]?.ToString();
                string senderPassword = _configuration["EmailSettings:Password"]?.ToString();
                foreach (var item in ToArr)
                {
                    mailMessage.To.Add(item);

                }

                var htmlFileContent = emailContent.Body;
                if (!string.IsNullOrEmpty(emailContent.HtmlFilePath))
                {
                    htmlFileContent = File.ReadAllText(Directory.GetCurrentDirectory() + @"\App_Data\Emails\" + emailContent.HtmlFilePath);
                    emailContent.Variables.Add("[FacebookUrl]", ConfigurationManager.AppSettings["FacebookUrl"]);
                    emailContent.Variables.Add("[TwitterUrl]", ConfigurationManager.AppSettings["TwitterUrl"]);
                    emailContent.Variables.Add("[LinkedinUrl]", ConfigurationManager.AppSettings["LinkedinUrl"]);
                    emailContent.Variables.Add("[IconsUrl]", ConfigurationManager.AppSettings["IconsUrl"]);
                    emailContent.Variables.Add("[BaseUrl]", ConfigurationManager.AppSettings["BaseUrl"]);
                    foreach (var variableName in emailContent.Variables.Keys)
                    {
                        htmlFileContent = htmlFileContent.Replace(variableName, emailContent.Variables[variableName]);
                    }
                }

                mailMessage.Subject = emailContent.subject;
                mailMessage.Body = htmlFileContent;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(senderFrom);

                foreach (var attachment in emailContent.Attachments)
                {
                    if (attachment != null)
                    {
                        mailMessage.Attachments.Add(new Attachment(attachment));
                    }
                }


                SmtpClient smtpClient = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(senderFrom, senderPassword),
                    Host = _configuration["EmailSettings:SmtpServer"],
                    Port = int.Parse(_configuration["EmailSettings:Port"]),
                    EnableSsl = true
                };

                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex.Message);
                return false;
            }
        }
    }
    public interface IEmailService
    {
        bool SendEmail(EmailContentModel emailContent);
    }
}
