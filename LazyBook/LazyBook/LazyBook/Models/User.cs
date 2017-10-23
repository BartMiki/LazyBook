using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyBook.Models
{
    class User
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("Email")]
        public string Email { get; set; }

        [Newtonsoft.Json.JsonProperty("Password")]
        public string Password { get; set; }

        [Newtonsoft.Json.JsonProperty("UserName")]
        public string UserName { get; set; }

        public User() { }

        public User(string email, string password, string userName)
        {
            Email = email;
            Password = password;
            UserName = userName;
        }
    }
}
