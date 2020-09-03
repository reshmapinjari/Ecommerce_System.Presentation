using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Ecommerce_System.Entities;

namespace Console_Ecommerce_System.Contracts
{
    public interface IOrderDetailBL : IDisposable
    {
        Task<List<OrderDetail>> GetOrderDetailsByOrderIDBL(Guid searchOrderID);
        Task<(bool,Guid)> AddOrderDetailsBL(OrderDetail newOrder);
        Task<bool> UpdateOrderDetailsBL(OrderDetail updateOrder);
        Task<bool> DeleteOrderDetailsBL(Guid deleteOrderID);
    }
}
