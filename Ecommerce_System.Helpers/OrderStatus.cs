using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Ecommerce_System.Helpers
{
    /// <summary>
    /// An Enum defining the status of order
    /// </summary>
    public enum OrderStatus
    {
        OrderConfirmed,
        OrderDispatched,
        Shipped,
        Delivered
    }
}
