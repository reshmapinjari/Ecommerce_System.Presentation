using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Console_Ecommerce_System.Entities;

namespace Console_Ecommerce_System.Contracts
{
    public interface IAddressBL : IDisposable
    {
        Task<bool> AddAddressBL(Address newAddress);
        Task<List<Address>> GetAllAddresssBL();
        Task<Address> GetAddressByAddressIDBL(Guid searchAddressID);
        Task<List<Address>> GetAddressByCustomerIDBL( Guid CustomerID);
        Task<bool> UpdateAddressBL(Address updateAddress);
        Task<bool> DeleteAddressBL(Guid deleteAddressID);
    }
}


