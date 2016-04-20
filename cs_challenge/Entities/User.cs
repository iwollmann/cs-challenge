using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cs_challenge.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Phone[] Phones { get; set; }

        private DateTime? createdOn;
        public DateTime CreatedOn
        {
            get
            {
                if (createdOn == null)
                    createdOn = DateTime.UtcNow;

                return createdOn.Value;
            }
            set { createdOn = value; }
        }

        private DateTime? updatedOn;
        public DateTime UpdatedOn
        {
            get
            {
                if (updatedOn == null)
                    updatedOn = DateTime.UtcNow;

                return updatedOn.Value;
            }
            set { updatedOn = value; }
        }
        public DateTime? LastLogin { get; set; }
        public string AccessToken { get; set; }
    }
}