using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Ecommerce_System.Entities;

namespace Console_Ecommerce_System.Contracts
{
    public interface IOrderBL : IDisposable
    {
        Task<Order> GetOrderByOrderNumberBL(double orderNumber);
        Task<Order> GetOrderByOrderIDBL(Guid searchOrderID);
        Task<List<Order>> GetOrdersByCustomerIDBL(Guid CustomerID);
        Task<(bool,Guid)> AddOrderBL(Order newOrder);
        Task<bool> UpdateOrderBL(Order updateOrder);
        Task<bool> DeleteOrderBL(Guid deleteOrderID);
    }
}
