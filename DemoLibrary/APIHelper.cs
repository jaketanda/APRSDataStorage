﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary
{
    public static class APIHelper
    {
        public static HttpClient APIClient { get; set; }
        
        public static void InitializeClient()
        {
            APIClient = new HttpClient();
            APIClient.BaseAddress = new Uri("https://api.aprs.fi/api/get?");
            APIClient.DefaultRequestHeaders.Accept.Clear();
            APIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
