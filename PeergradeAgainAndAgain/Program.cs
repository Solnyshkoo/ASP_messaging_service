using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PeergradeAgainAndAgain.Models;

namespace PeergradeAgainAndAgain
{
    public class Program
    {
        public static SortedList<string, User> Users = new SortedList<string, User>();
        public static List<Message> AllMessages = new List<Message>();
        public static void Main(string[] args)
        {
            Deserialization();
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        

        /// <summary>
        /// Serialization
        /// </summary>
        public static void Serialization()
        {
            try
            {
                string jsonUsers = JsonSerializer.Serialize(Users);
                string jsonMessages = JsonSerializer.Serialize(AllMessages);
                File.WriteAllText("Users.json", jsonUsers);
                File.WriteAllText("Messages.json", jsonMessages);
            }
            catch 
            {
                // ignored
            }
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        private static void Deserialization()
        {
            try
            {
                string jsonUsers = File.ReadAllText("Users.json");
                Users = JsonSerializer.Deserialize<SortedList<string, User>>(jsonUsers);
                string jsonMessages = File.ReadAllText("Messages.json");
                AllMessages = JsonSerializer.Deserialize<List<Message>>(jsonMessages);
            }
            catch
            {
                // ignored
            }
        }
    }
}