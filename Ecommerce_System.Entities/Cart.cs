using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Ecommerce_System.Entities
{
    public class Cart
    {
        public Guid CartId { get; set; } //ID of the cart
        public Guid AddressID { get; set; } //address ID of the Customer 
        public Guid CustomerID { get; set; } //ID of the Customer
        
        public int TotalQuantity { get; set; } //Total quantity of the products ordererd
        public int TotalAmount { get; set; } //Cart value
    }
}
