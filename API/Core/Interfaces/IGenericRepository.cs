using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<List<T>> ListAsync(ISpecification<T> spec);

        Task<T> GetEntityWithSpec(ISpecification<T> spec);

        Task<int> CountAsync(ISpecification<T> spec);

        bool GetUsername(string username, string flag);
        Task<T> UpdateAsync(T updateRequest, T existingJersey, Func<T, T, T> update);

        Task DeleteAsync(Guid id);
        Task<T> AddAsync(T add);
    }
}
