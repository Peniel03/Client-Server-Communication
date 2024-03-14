using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class PayrollSheet
    {
        public string WorkShop { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public double ScopeCompletedWork { get; set; }
        public double UnitPrice { get; set; }
        public double AccuredEarnings { get; set; }

    }
}
