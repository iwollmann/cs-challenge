using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace cs_challenge.Contracts
{
    [DataContract]
    public class PhoneContract
    {
        [DataMember(Name = "Numero")]
        public int Number { get; set; }
        [DataMember(Name = "DDD")]
        public int LocalCode { get; set; }
    }
}