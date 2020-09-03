using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Helpers;

namespace Console_Ecommerce_System.Presentation
{
    public static class CommonData
    {
        public static IUser CurrentUser { get; set; }
        public static UserType CurrentUserType { get; set; }
    }
}