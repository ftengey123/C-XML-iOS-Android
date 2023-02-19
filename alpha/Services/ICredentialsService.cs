using System.Threading.Tasks;

namespace alpha.Services
{
    public interface ICredentialsService
    {
        Task<bool> CheckLogin(string userName, string password);
    }
}