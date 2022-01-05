using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtimeGpsTracker.Core.Interfaces.Repositories;

namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BaseDbContext _baseDbContext;

        public Repository(BaseDbContext baseDbContext)
        {
            _baseDbContext = baseDbContext;
        }

        //Metody pre ulozenie do databazy sychronne aj asynchronne
        protected void Save() => _baseDbContext.SaveChanges();
        //protected async void SaveAsync() => await _applicationDbContext.SaveChangesAsync();

        protected DataTable ConvertToDatatable(IList<T> data)
        {
            DataTable dataTable = new DataTable();

            // Get all properties for specified object
            IList<PropertyInfo> props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !p.GetGetMethod(false).IsVirtual)
                .ToList();

            DataColumn dataTableColumn;

            for (int i = 0; i < props.Count; i++)                
            {
                PropertyInfo prop = props[i];

                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);

                if (prop.PropertyType == typeof(DateTime))
                {
                    dataTableColumn = new DataColumn(prop.Name, type);
                    dataTableColumn.DateTimeMode = DataSetDateTime.Utc;
                }
                else
                {
                    dataTableColumn = new DataColumn(prop.Name, type);
                }

                dataTable.Columns.Add(dataTableColumn);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }


        //Synchronne
        public T GetById(string id)
        {
            return _baseDbContext.Set<T>().Find(id);
        }

        public int Count(Func<T, bool> predicate)
        {
            return _baseDbContext.Set<T>().Where(predicate).Count();
        }

        public void Add(T entity)
        {
            _baseDbContext.Add(entity);
            Save();
        }

        public void AddMore(IList<T> entities)
        {
            _baseDbContext.AddRange(entities);
            Save();
        }

        public void Update(T entity)
        {
            _baseDbContext.Entry(entity).State = EntityState.Modified;
            Save();
        }

        public void Delete(T entity)
        {
            _baseDbContext.Remove(entity);
            Save();
        }

        public bool Exists(Func<T, bool> predicate)
        {
            return _baseDbContext.Set<T>().Any(predicate);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _baseDbContext.Set<T>().Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _baseDbContext.Set<T>();
        }

        //Asynchronne
        public async Task<T> GetByIdAsync(string id)
        {
            return await _baseDbContext.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            _baseDbContext.Add(entity);
            await _baseDbContext.SaveChangesAsync();
        }

        public async Task AddMoreAsync(IList<T> entities)
        {
            _baseDbContext.AddRange(entities);
            await _baseDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            // _applicationDbContext.Attach(entity); //EntityState.Modified automaticky vola Attach metodu
            _baseDbContext.Entry(entity).State = EntityState.Modified;
            await _baseDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _baseDbContext.Remove(entity);
            await _baseDbContext.SaveChangesAsync();
        }

        //public async Task<bool> ExistsAsync(Func<T, bool> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _baseDbContext.Set<T>().ToListAsync();
        }
    }
}
