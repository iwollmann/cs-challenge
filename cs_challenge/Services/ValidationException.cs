using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace cs_challenge.Services
{
    public class ValidationException : Exception
    {
        public HttpStatusCode Code { get; set; }

        public ValidationException(HttpStatusCode code, string message): base(message)
        {
            Code = code;
        }
    }
}