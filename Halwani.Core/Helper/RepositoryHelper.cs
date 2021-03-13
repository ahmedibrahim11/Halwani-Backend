using Halwani.Utilites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halawani.Core.Helper
{
    public class RepositoryHelper
    {
        public static void LogException(Exception ex)
        {
            try
            {
                LoggerHelper.LogError(JsonConvert.SerializeObject(ex));
            }
            catch (Exception)
            {
                LoggerHelper.LogError(ex.Message);
            }
        }
    }
}
