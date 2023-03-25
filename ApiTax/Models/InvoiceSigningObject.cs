using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTax.Models
{
    public class InvoiceSigningObject
    {
        public List<body>  body { get; set; }
      public List<Payment>  payment { get; set; }
        public Extension extension { get; set; }

        public Header Header { get; set; }

    }
}