﻿using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using Newtonsoft.Json;

namespace PTV_Demo
{
    class Program
    {
        static string url = "/v3/disruptions/route/8";

        static void Main(string[] args)
        {
            //load api key and user ID
            Settings settings = new Helpers().loadSettings("settings.json");

            Console.WriteLine("PTV example.. Beginning ");
            url = $"{url}?devid={settings.userID}";//add the user is to the URL to create 
            ASCIIEncoding encoding = new ASCIIEncoding();
            // encode key
            byte[] keyBytes = encoding.GetBytes(settings.apiKey);
            byte[] urlBytes = encoding.GetBytes(url);
            byte[] tokenBytes = new HMACSHA1(keyBytes).ComputeHash(urlBytes);
            var sb = new StringBuilder();
            // convert signature to string
            Array.ForEach<byte>(tokenBytes, x => sb.Append(x.ToString("X2")));
            // add signature to url
            url = $"{url}&signature={sb.ToString()}";
            Console.WriteLine(url);
        }
    }

    class Helpers
    {

        public Settings loadSettings(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string json = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<Settings>(json);    
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load settings file: " + ex);
                return null;
            }

        }
    }
    public class Settings 
    {
        [JsonProperty("user_id")]
        public int userID { get; set; }
        [JsonProperty("api_key")]
        public string apiKey { get; set; }
    }
}
