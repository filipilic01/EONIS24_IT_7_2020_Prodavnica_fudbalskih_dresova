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
        public async Task<T> AddAsync(T add)
        {
            var Dres = await _context.Set<T>().AddAsync(add);
            await _context.SaveChangesAsync();
            return Dres.Entity;
        }
        public async Task<T> UpdateAsync(T updateRequest, T existingDres, Func<T, T, T> update)
        {
            var updatedEntity = update(existingDres, updateRequest);
            _context.Entry(existingDres).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return updatedEntity;
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

   

        public bool GetKorisnickoIme(string KorisnickoIme, string flag)
        {
            if (flag == "Admin")
            {
                var admin =  _context.Admins.Where(u => u.AdminKorisnickoIme == KorisnickoIme).FirstOrDefault();
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
                var Kupac = _context.Kupacs.Where(u => u.KupacKorisnickoIme == KorisnickoIme).FirstOrDefault();
                if (Kupac == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
        }

        public bool GetEmail(string email, string flag)
        {
            if (flag == "Admin")
            {
                var admin = _context.Admins.Where(u => u.AdminEmail == email).FirstOrDefault();
                if (admin == null)
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
                var Kupac = _context.Kupacs.Where(u => u.KupacEmail == email).FirstOrDefault();
                if (Kupac == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

        public Admin GetAdminByKorisnickoIme(string KorisnickoIme)
        {
           return _context.Admins.FirstOrDefault(a => a.AdminKorisnickoIme == KorisnickoIme);

        }
        public Kupac GetKupacByKorisnickoIme(string KorisnickoIme)
        {
            return _context.Kupacs.FirstOrDefault(a => a.KupacKorisnickoIme == KorisnickoIme);
        }

        public async Task<List<StavkaPorudzbine>> GetStavkaPorudzbineByPorudzbinaId(Guid id)
        {
            var items =  await _context.StavkaPorudzbines.Where(o => o.PorudzbinaId == id).AsNoTracking().Include(s => s.VelicinaDresa).ThenInclude(d => d.Dres).ToListAsync();
            return items;
        }
        public async Task<List<Porudzbina>> GetPorudzbinasByKupacId(Guid id)
        {
            var porudzbinas = await _context.Porudzbinas.Where(o => o.KupacId == id).ToListAsync();
            return porudzbinas;
        }
        public async Task<List<VelicinaDresa>> GetVelicineDresovaByDresId(Guid id)
        {
            var velicine = await _context.VelicinaDresas.Where(o => o.DresId == id).ToListAsync();
            return velicine;
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }

        
    }
}
