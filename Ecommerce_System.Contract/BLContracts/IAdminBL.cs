using System;
using System.Threading.Tasks;
using Console_Ecommerce_System.Entities;
/// <summary>
/// developed by sravani
/// </summary>
namespace Console_Ecommerce_System.Contracts
{
    public interface IAdminBL : IDisposable
    {
        Task<Admin> GetAdminByEmailAndPasswordBL(string email, string password);
        Task<bool> UpdateAdminBL(Admin updateAdmin);
        Task<bool> UpdateAdminPasswordBL(Admin updateAdmin);
        Task<Admin> GetAdminByAdminEmailBL(string Email);
    }
}