﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Tenant { get; set; }
    }
}
