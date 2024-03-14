using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Request
    {
        public string JsonData { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Code { get; set; }
    }
}
