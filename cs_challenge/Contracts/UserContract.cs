using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace cs_challenge.Contracts
{
    [DataContract]
    [KnownType(typeof(string))]
    public class UserContract
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember(Name="Nome")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(400, MinimumLength = 1)]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember(Name="Senha")]
        public string Password { get; set; }

        [DataMember(Name = "Telefones")]
        public PhoneContract[] Phones { get; set; }
        [DataMember(Name="Data_Criacao")]
        public DateTime CreatedOn { get; set; }
        [DataMember(Name ="Data_Atualizacao")]
        public DateTime UpdatedOn { get; set; }
        [DataMember(Name = "Ultimo_Login")]
        public DateTime? LastLogin { get; set; }
        [DataMember(Name = "Token")]
        public string AccessToken { get; set; }
    }
}