using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronotrackService.Application
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserByEmailAsync(string email);
    }
}
