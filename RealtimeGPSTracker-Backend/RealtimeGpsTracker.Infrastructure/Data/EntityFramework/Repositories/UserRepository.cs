using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using System.Data.SqlClient;

namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BaseDbContext baseDbContext) : base(baseDbContext)
        {

        }

        public async Task<int> DeleteUserAsync(string userId)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@UserId", userId)
                {
                    TypeName = "NVARCHAR(450)"
                }
            };

            var result = await _baseDbContext.Database.ExecuteSqlCommandAsync("EXEC [AspNetUsers_Delete] @UserId", sqlParameters);

            return result;
        }
    }
}