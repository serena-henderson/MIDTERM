using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDTERM
{
    [Serializable]

    class Order
    {
        public int OId { get; set; }
        public string OProduct { get; set; }
        public int OPrice { get; set; }
        public string OName { get; set; }
    }
}
