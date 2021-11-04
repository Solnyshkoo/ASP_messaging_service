using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PeergradeAgainAndAgain.Models;

namespace PeergradeAgainAndAgain.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        ///  Create user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>result</returns>
        [HttpPost("create-user")]
        public IActionResult CreateUser([FromQuery] User user)
        {
            if (user.Email == null  || user.Email.Length == 1 || !user.Email.Contains("@") )
            {
                return BadRequest($"Email некорректный.");
            }
            User tempUser = new User
            {
                Email = user.Email,
                Name = user.Name
            };
            
            if (Models.User.FindUser(tempUser.Email))
            {
                return NotFound($"Пользователя с таким email уже зарегистрирован.");
            }
            Program.Users.Add(tempUser.Email, tempUser);
            Program.Serialization();
            return Ok($"Пользователь {tempUser.Email} зарегистрирован.");
        }
        
        /// <summary>
        /// Get user according to its email.
        /// </summary>
        /// <param name="email"> email</param>
        /// <returns>user info</returns>
        [HttpGet("get-user-by-email")]
        public IActionResult GetUserByEmail([FromQuery] string email)
        {
            List<User> result = new List<User>();
            foreach (var item in Program.Users)
                if (item.Key == email)
                    result.Add(item.Value);
            if (result.Count == 0)
                return NotFound($"Пользователь {email} не найден.");
            return Ok(result[0]);
        }
        
        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="limit">amount of users</param>
        /// <param name="offset">start posotion</param>
        /// <returns>information about all users</returns>
        [HttpGet("get-all-users")]
        public IActionResult GetUsers([FromQuery] int limit, int offset)
        {
            if (limit <= 0 || offset < 0 )
                return BadRequest("Неправильный формат");
            if (limit + (offset - 1) > Program.Users.Count) // если кол-во превышвет максимальное выводятся вся!
                limit = Program.Users.Count - offset + 1;
            List<User> result = new List<User>();
            for (int i = offset - 1; i < limit + offset - 1; i++)
            {
                result.Add(Program.Users.Values[i]);
            }
            if (result.Count == 0)
                return NotFound("Нет пользоватлей.");
            return Ok(result);
        }
        /// <summary>
        /// Create user using random 
        /// </summary>
        /// <param name="amount">amount</param>
        /// <returns>users</returns>
        [HttpPost("create-random-user")]
        public IActionResult CreateRandomUser([FromQuery] int amount)
        {
            if (amount <= 0 || amount >= 1001)
                return NotFound("Число должно быть пололожительным и не большим 1000.");
            
            List<User> users = new List<User>();
            for (int i = 0; i < amount; i++)
            {
                User tempUser = new User
                {
                    Name = Models.User.RandomName(), 
                    Email =  Models.User.RandomEmail()
                };
                if (Models.User.FindUser(tempUser.Email))
                {
                    amount++;
                }
                else
                {
                    users.Add(tempUser);
                    Program.Users.Add(tempUser.Email, tempUser);
                }
            }
            Program.Serialization();
            return Ok(users);
        }
        
    }
}