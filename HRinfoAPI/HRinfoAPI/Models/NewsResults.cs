using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRinfoAPI.Models
{
    public class NewsResults
    {
        public string ResultNews { get; set; }
        public DateTime? ResultDateFrom { get; set; }
        public DateTime? ResultDateTo { get; set; }
    }
}