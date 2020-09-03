using System;
using System.Collections.Generic;

namespace Console_Ecommerce_System.Entities
{
    public interface IUser
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}
