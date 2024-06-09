﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaret.Application.DTOs.Facebook
{
    public class FacebookAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        
        /*
         {"access_token":"8051595048197662|2JMtS7Zl8QqwIe_RbZU9YtiXDZ4",
        "token_type":"bearer"}
         */
    }
}
