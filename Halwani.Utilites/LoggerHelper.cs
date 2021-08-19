using System;
using System.IO;
//using System.Web.Hosting;

namespace Halwani.Utilites
{
    public class LoggerHelper
    {
        public static void LogError(string msg)
        {
            File.AppendAllText(Directory.GetCurrentDirectory() + @"\App_Data\Logs\" + @"\log.txt", "-----------------\n\n");
            File.AppendAllText(Directory.GetCurrentDirectory() + @"\App_Data\Logs\" + @"\log.txt", "Date : " + DateTime.Now.AddHours(2).ToString() + "\n\n");
            File.AppendAllText(Directory.GetCurrentDirectory() + @"\App_Data\Logs\" + @"\log.txt", msg);
            File.AppendAllText(Directory.GetCurrentDirectory() + @"\App_Data\Logs\" + @"\log.txt", "\t\t\r\r\n\n-----------------\t\t\r\r\n\n");
        }
    }
}
