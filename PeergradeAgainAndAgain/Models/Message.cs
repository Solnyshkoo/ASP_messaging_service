using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PeergradeAgainAndAgain.Models
{
    public class Message
    {
        // random
        static Random rand = new Random();
        // text for messages
        private static string letters = File.ReadAllText("text2.txt");
        /// <summary>
        /// Subject.
        /// </summary>
        [Required]
        public string Subject { get; set; }
        /// <summary>
        /// Messages.
        /// </summary>
        [Required]
        public string Messages{ get; set; }
        /// <summary>
        /// SenderId
        /// </summary>
        [Required]
        public string SenderId { get; set; }
        /// <summary>
        /// ReceiverId
        /// </summary>
        [Required]
        public string ReceiverId { get; set; }
        
        /// <summary>
        /// Get random subject or message.
        /// </summary>
        /// <param name="subjectOrNot">true - create subject, false - create message</param>
        /// <returns>subject or message.</returns>
        public static string RandomItem(bool subjectOrNot)
        {
            int n = subjectOrNot ? 30 : 300; // если нужно тему, то кол-во символов меньше
            int length = rand.Next(20, n);
            string subject = "";
            int firstletter = rand.Next(302, 65750);
            firstletter -= length; // чтобы не выйти за рамки кол-во
            subject += $"{letters.Substring(firstletter, length)}";
            return subject;
        }
        /// <summary>
        /// Get random id(email).
        /// </summary>
        /// <param name="email">exaption number</param>
        /// <returns>id(email)</returns>
        public static string RandomId(string email)
        {
            string id = email;
            while (id == email)
            {
                int index = rand.Next(0, Program.Users.Count);
                id = Program.Users.Keys[index];
            }
            return id;
        }

        /// <summary>
        /// Get all messages from sender to receiver.
        /// </summary>
        /// <param name="senderId">sender email</param>
        /// <param name="receiverId">receiver email</param>
        /// <returns>list with all messages</returns>
        public static List<Message> GetMessages(string senderId, string receiverId)
        {
            List<Message> messages = new List<Message>();
            foreach (var item in Program.AllMessages)
            {
                if (item.ReceiverId == receiverId && item.SenderId == senderId)
                {
                    messages.Add(item);
                }
            }
            return messages;
        }

        /// <summary>
        /// Get all meseges from specific sender ot to specific receiver.
        /// </summary>
        /// <param name="id">id(email)</param>
        /// <param name="senderOrNot">true - from sender, false - from receiver.</param>
        /// <returns>list with all messages</returns>
        public static List<Message> GetMessages(string id, bool senderOrNot)
        {
            List<Message> messages = new List<Message>();
            if (senderOrNot)
            {
                foreach (var item in Program.AllMessages)
                    if (item.SenderId == id)
                        messages.Add(item);
            }
            else
            {
                foreach (var item in Program.AllMessages)
                    if (item.ReceiverId == id)
                        messages.Add(item);
            }
            return messages;
        }
    }
}