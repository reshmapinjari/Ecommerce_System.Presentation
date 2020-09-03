using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Console_Ecommerce_System.Entities;

namespace Console_Ecommerce_System.Contracts
{
    public interface ICustomerBL : IDisposable
    {
        Task<(bool,Guid)> AddCustomerBL(Customer newCustomer);
        Task<List<Customer>> GetAllCustomersBL();
        Task<Customer> GetCustomerByCustomerIDBL(Guid searchCustomerID);
        Task<List<Customer>> GetCustomersByNameBL(string supplierName);
        Task<Customer> GetCustomerByEmailBL(string email);
        Task<Customer> GetCustomerByEmailAndPasswordBL(string email, string password);
        Task<bool> UpdateCustomerBL(Customer updateCustomer);
        Task<bool> UpdateCustomerPasswordBL(Customer updateCustomer);
        Task<bool> DeleteCustomerBL(Guid deleteCustomerID);
    }
}


