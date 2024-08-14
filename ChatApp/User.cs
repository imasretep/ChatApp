using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class User
    {
        public string Username { get; set; }

        public User()
        {

        }
        public User(string username)
        {
            Username = username;
        }
    }
}
