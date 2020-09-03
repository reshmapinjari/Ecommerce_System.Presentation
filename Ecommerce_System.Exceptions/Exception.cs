﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Ecommerce_System.Exceptions
{
    /// <summary>
    /// Represents project-level user-defined exception.
    /// </summary>
    public class Exception : ApplicationException
    {
        ///constructors
        public Exception() : base()
        {
        }

        public Exception(string message) : base(message)
        {
        }

        public Exception(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}


