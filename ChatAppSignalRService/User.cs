﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppSignalRService
{
    public class User
    {
        public string Username { get; set; }

        public User(string username)
        {
            Username = username;

        }
    }
}
