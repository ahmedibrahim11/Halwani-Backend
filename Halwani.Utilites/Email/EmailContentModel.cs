using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halwani.Utilites.Email
{
    public class EmailContentModel
    {
        public EmailContentModel()
        {
            Attachments = new List<string>();
        }
        public string From { get; set; }
        public string ToList { get; set; }
        public string subject { get; set; }
        public string HtmlFilePath { get; set; }
        public Dictionary<string, string> Variables { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; }
    }
}
