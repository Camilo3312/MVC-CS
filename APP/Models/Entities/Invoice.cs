using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Models.Entities
{
    public class Invoice
    {
        public int id { set; get; }
        public int idclient { set; get; }
        public int cod { set; get; }

        public List<InvoiceDetaill> detaills { set; get; }
    }
}
