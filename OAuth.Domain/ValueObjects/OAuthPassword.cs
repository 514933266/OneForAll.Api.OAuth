using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OAuth.Domain.ValueObjects
{
    public class OAuthPassword
    {
        public string Old { get; set; }

        public string New { get; set; }

        public string Repeat { get; set; }
    }
}
