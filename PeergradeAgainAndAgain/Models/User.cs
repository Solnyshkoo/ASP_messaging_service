using System;
using System.ComponentModel.DataAnnotations;

namespace PeergradeAgainAndAgain.Models
{
   
    public class User
    {
        // random
        static Random rand = new Random();
        // letters for user name and email
        private static string letters = "abcdefghijklmnopqrstuvwxyz0123456789";
        
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; }
        
        /// <summary>
        /// Check if such email had been already registrate.
        /// </summary>
        /// <param name="user">user email</param>
        /// <returns>true - exists, false - doesn't exist </returns>
        public static bool FindUser(string user)
        {
            foreach (var users in Program.Users)
            {
                if (user == users.Value.Email)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Get random name.
        /// </summary>
        /// <returns>name</returns>
        public static string RandomName()
        {
            int length = rand.Next(1, 10);
            string name = "";
            for (int i = 0; i < length; i++)
            {
                int letter = rand.Next(0, 25);
                name += $"{letters[letter]}";
            }
            return name;
        }
        /// <summary>
        /// Get random email.
        /// </summary>
        /// <returns>email</returns>
        public static string RandomEmail()
        {
            int length = rand.Next(2, 15);
            string email = "";
            for (int i = 0; i < length; i++)
            {
                int letter = rand.Next(0, 35);
                email += $"{letters[letter]}";
            }

            email += "@mail.ru";
            return email;
        }
    }
    
    
}