using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Synchronne
        T GetById(string id);
        int Count(Func<T, bool> predicate);
        void Add(T entity);
        void AddMore(IList<T> entities);
        void Update(T entity);
        void Delete(T entity);
        bool Exists(Func<T, bool> predicate);
        IEnumerable<T> Find(Func<T, bool> predicate);
        IEnumerable<T> GetAll();

        // Asynchronne
        Task<T> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task AddMoreAsync(IList<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        //Task<bool> ExistsAsync(Func<T, bool> predicate);
        Task<ICollection<T>> GetAllAsync();
    }
}
