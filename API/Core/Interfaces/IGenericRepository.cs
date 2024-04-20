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

        Task<List<StavkaPorudzbine>> GetStavkaPorudzbineByPorudzbinaId(Guid id);
        Task<List<Porudzbina>> GetPorudzbinasByKupacId(Guid id);
        Task<int> CountAsync(ISpecification<T> spec);

        bool GetKorisnickoIme(string KorisnickoIme, string flag);
        bool GetEmail(string email, string flag);
        Task<T> UpdateAsync(T updateRequest, T existingDres, Func<T, T, T> update);

        Admin GetAdminByKorisnickoIme(string KorisnickoIme);
        Kupac GetKupacByKorisnickoIme(string KorisnickoIme);
        Task DeleteAsync(Guid id);
        Task<T> AddAsync(T add);
    }
}
