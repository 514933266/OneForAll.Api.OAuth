using OAuth.Domain.Models;
using OAuth.Domain.ValueObjects;
using System.Threading.Tasks;

namespace OAuth.Domain.Interfaces
{
    public interface IOAuthLoginManager
    {
        /// <summary>
        ///  登录验证
        /// </summary>
        /// <param name="user">实体</param>
        /// <returns>结果</returns>
        Task<OAuthLoginResult> LoginAsync(OAuthLogin user);
    }
}
