using Core.Entities;
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

        Task<List<OrderItem>> GetOrderItemsByOrderId(Guid id);
        Task<List<Order>> GetOrdersByCustomerId(Guid id);
        Task<int> CountAsync(ISpecification<T> spec);

        bool GetUsername(string username, string flag);
        bool GetEmail(string email, string flag);
        Task<T> UpdateAsync(T updateRequest, T existingJersey, Func<T, T, T> update);

        Admin GetAdminByUsername(string username);
        Customer GetCustomerByUsername(string username);
        Task DeleteAsync(Guid id);
        Task<T> AddAsync(T add);
    }
}
