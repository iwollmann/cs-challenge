using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace cs_challenge.Contracts
{
    [DataContract]
    [KnownType(typeof(string))]
    public class LoginUserContract
    {
        [DataMember]
        public string Email { get; set; }
        [DataMember(Name ="Senha")]
        public string Password { get; set; }
    }
}