using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var enitityForDelete = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(enitityForDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> AddAsync(T add)
        {
            var jersey = await _context.Set<T>().AddAsync(add);
            await _context.SaveChangesAsync();
            return jersey.Entity;
        }

        public async Task<T> UpdateAsync(T updateRequest, T existingJersey, Func<T, T, T> update)
        {
           
            var updatedEntity = update(existingJersey, updateRequest);
            _context.Entry(existingJersey).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return updatedEntity;
        }

        public bool GetUsername(string username, string flag)
        {
            if (flag == "Admin")
            {
                var admin =  _context.Admins.Where(u => u.AdminUserName == username).FirstOrDefault();
                if(admin == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                var customer = _context.Customers.Where(u => u.CustomerUserName == username).FirstOrDefault();
                if (customer == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
        }

        public Admin GetAdminByUsername(string username)
        {
           return _context.Admins.FirstOrDefault(a => a.AdminUserName == username);

        }
        public Customer GetCustomerByUsername(string username)
        {
            return _context.Customers.FirstOrDefault(a => a.CustomerUserName == username);
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }

        
    }
}
