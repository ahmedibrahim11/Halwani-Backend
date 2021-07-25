//using Halawani.Core;
//using Halawani.Core.Helper;
//using Halwani.Core.ModelRepositories.Interfaces;
//using Halwani.Core.ViewModels.RequestTypeModels;
//using Halwani.Core.ViewModels.GenericModels;
//using Halwani.Data.Entities.ProductCategories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Halwani.Data.Entities.Incident;
//using Microsoft.AspNetCore.Http;
//using System.Transactions;
//using System.IO;
//using System.Security.Claims;
//using Halwani.Data.Entities.User;
//using Halwani.Core.ViewModels.TicketModels;
//using Microsoft.Extensions.Configuration;
//using Halwani.Utilites.Email;

//namespace Halwani.Core.ModelRepositories
//{
//    public class UserRequestRepository
//    {
//        private readonly IConfiguration configuration;
//        private readonly IEmailService emailService;
//        public UserRequestRepository(IConfiguration _configuration, IEmailService _emailService)
//        {
//            configuration = _configuration;
//            emailService = _emailService;
//        }
//        public RepositoryOutput AskForSupport(AskForSupportViewModel model, ClaimsIdentity userClaims)
//        {
//            try
//            {
//                var email = configuration["Request:Support"];
//                var userName = 
//                Dictionary<string, string> Variables = new Dictionary<string, string>
//                                            {
//                                                { "[UserName]", userClaims}
//                                            };
//                try
//                {
//                    _emailService.SendEmail(new EmailContentModel
//                    {
//                        Body = "",
//                        subject = "Email Confirmed",
//                        ToList = appUser.Email,
//                        HtmlFilePath = "confirmEmail.html",
//                        Variables = Variables
//                    });
//                }
//                catch (Exception ex)
//                {
//                    //RepositoryHelper.LogException(ex);
//                }
//                return RepositoryOutput.CreateSuccessResponse();
//            }
//            catch (Exception ex)
//            {
//                RepositoryHelper.LogException(ex);
//                return RepositoryOutput.CreateErrorResponse(ex.Message);
//            }
//        }
//    }
//}
