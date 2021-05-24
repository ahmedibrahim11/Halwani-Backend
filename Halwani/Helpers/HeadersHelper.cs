using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Halwani.Helpers
{
    public class HeadersHelper
    {
        public static string GetAuthToken(HttpRequest request)
        {
            try
            {
                //I removed keyword bearer to connect with signalR hubConnectionBuilder
                request.Headers.TryGetValue("Authorization", out var token);
                return token.FirstOrDefault().Replace("Bearer", "").Replace("bearer", "");
                //return token;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
