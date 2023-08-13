using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker
{
    public class Entry
    {
        public EntryType Type { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
