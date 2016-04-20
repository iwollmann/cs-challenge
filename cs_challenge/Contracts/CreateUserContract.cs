using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace cs_challenge.Contracts
{
    [DataContract]
    [KnownType(typeof(string))]
    public class CreateUserContract
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember(Name ="Nome")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(400, MinimumLength = 1)]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember(Name="Senha")]
        public string Password { get; set; }

        [DataMember(Name="Telefones")]
        public PhoneContract[] Phones { get; set; }
    }
}