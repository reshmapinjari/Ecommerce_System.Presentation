using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Helpers;

namespace Console_Ecommerce_System.Contracts
{
    public interface IOnlineReturnBL : IDisposable
    {
        Task<bool> AddOnlineReturnBL(OnlineReturn newOnlineReturn);
        Task<List<OnlineReturn>> GetAllOnlineReturnsBL();
        Task<OnlineReturn> GetOnlineReturnByOnlineReturnIDBL(Guid searchOnlineReturnID);
        Task<List<OnlineReturn>> GetOnlineReturnsByPurposeBL(PurposeOfReturn purpose);
        Task<bool> UpdateOnlineReturnBL(OnlineReturn updateOnlineReturn);
        Task<bool> DeleteOnlineReturnBL(Guid deleteOnlinereturnID);
    }
}

