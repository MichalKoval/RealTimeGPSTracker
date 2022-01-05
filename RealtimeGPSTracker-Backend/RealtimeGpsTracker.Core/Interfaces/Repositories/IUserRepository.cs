using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> DeleteUserAsync(string userId);
    }
}
