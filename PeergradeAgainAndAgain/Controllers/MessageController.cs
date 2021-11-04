using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PeergradeAgainAndAgain.Models;

namespace PeergradeAgainAndAgain.Controllers
{
    public class MessageController : Controller
    {
        /// <summary>
        /// Create messages using random.
        /// </summary>
        /// <param name="amount">amount</param>
        /// <returns>messages</returns>
        [HttpPost("create-random-messages")]
        public IActionResult CreateRandomMessage([FromQuery] int amount)
        {
            List<Message> messages = new List<Message>();
            if (amount <= 0 || amount >= 1001)
                return NotFound("Число должно быть пололожительным и не большим 1000.");
            
            if (Program.Users.Count <= 1)
                return NotFound("Недостаточно пользователей.");
            for (int i = 0; i < amount; i++)
            {
                string id = Message.RandomId("none");
                Message tempMessage = new Message()
                {
                    Subject = Message.RandomItem(true), 
                    Messages = Message.RandomItem(false),
                    SenderId = id,
                    ReceiverId = Message.RandomId(id)
                };
                messages.Add(tempMessage);
                Program.AllMessages.Add(tempMessage);
            }
            Program.Serialization();
            return Ok(messages);
        }
        /// <summary>
        /// Create message.
        /// </summary>
        /// <param name="message">message</param>
        /// <returns>message</returns>
        [HttpPost("create-message")]
        public IActionResult CreateMessage([FromQuery] Message message )
        {
            if (!Models.User.FindUser(message.SenderId))
                return NotFound($"Пользователь {message.SenderId} не найден.");
            if (!Models.User.FindUser(message.ReceiverId))
                return NotFound($"Пользователь {message.ReceiverId} не найден.");
            
            Message tempMessage = new Message()
            {
                Subject = message.Subject, 
                Messages = message.Messages,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId
            };
            Program.AllMessages.Add(tempMessage);
            Program.Serialization();
            return Ok("Сообщение отправлено");
        }
        
        /// <summary>
        /// Get message from sender to receiver.
        /// </summary>
        /// <param name="senderId">sender email</param>
        /// <param name="receiverId">receiver email</param>
        /// <returns>all messages</returns>
        [HttpGet("get-message-by-senderId-and-receiverId")]
        public IActionResult GetMessagesByBothId([FromQuery] string senderId, string receiverId)
        {
            if (senderId == null || receiverId == null)
                return NotFound("Поля не должны быть пустыми");
            List<Message> result = Message.GetMessages(senderId, receiverId);
            if (result == null || result.Count == 0)
            {
                return NotFound("По данному запросу ничего не найдено.");
            }
            return Ok(result);
        }
        /// <summary>
        /// Get messages by sender id(email).
        /// </summary>
        /// <param name="senderId">sender id(email)</param>
        /// <returns>all messages</returns>
        [HttpGet("get-message-by-senderId")]
        public IActionResult GetMessagesBySenderId([FromQuery] string senderId)
        {
            if (senderId == null)
                return NotFound("Поля не должны быть пустыми");
            List<Message> result = Message.GetMessages(senderId, true);
            if (result == null || result.Count == 0)
                return NotFound("По данному запросу ничего не найдено.");
            else
                return Ok(result);
        }
        
        /// <summary>
        /// Get messages to the receiver.
        /// </summary>
        /// <param name="receiverId">receiver id(email)</param>
        /// <returns>all messages</returns>
        [HttpGet("get-message-by-receiverId")]
        public IActionResult GetMessagesByRecieverId([FromQuery] string receiverId)
        {
            if (receiverId == null)
                return NotFound("Поля не должны быть пустыми");
            List<Message> result = Message.GetMessages(receiverId, false);
            if (result == null || result.Count == 0)
                return NotFound("По данному запросу ничего не найдено.");
            else
                return Ok(result);
        }

    }
}