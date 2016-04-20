using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cs_challenge.Entities
{
    public class Phone
    {

        public Guid Id { get; set; }
        public int Number { get; set; }
        public int LocalCode { get; set; }

        public virtual User User { get; set; }
    }
}