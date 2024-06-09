﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaret.Application.DTOs.Facebook
{
    public class FacebookUserInfoResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        
    }
    /*{"email":"onlineogretmen43\u0040gmail.com","name":"Ramazan Ramazan","id":"988489166316424"}*/
}
