﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services
{
    public interface ILoginService
    {
        Task Login(string email, string password);
    }
}