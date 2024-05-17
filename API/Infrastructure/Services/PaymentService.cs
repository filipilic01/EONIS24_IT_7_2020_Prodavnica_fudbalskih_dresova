
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Core.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Porudzbina> _porudzbinaRepository;
        private readonly IConfiguration _configuration;
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public PaymentService(IMapper mapper,IGenericRepository<Porudzbina> genericRepository, IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _porudzbinaRepository = genericRepository;
            _context = context;
            _mapper = mapper;   
        }
        public async Task<PorudzbinaPaymentDto> CreateOrUpdatePaymentIntent(Guid porudzbinaId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var porudzbina = _context.Porudzbinas.Include(s => s.StavkaPorudzbines).Where(s => s.PorudzbinaId == porudzbinaId).Select(s => new Porudzbina
            {
                PorudzbinaId = s.PorudzbinaId,
                UkupanIznos = s.UkupanIznos,
                DatumKreiranja = s.DatumKreiranja,
                DatumAzuriranja = s.DatumAzuriranja,
                KupacId = s.KupacId,
                ClientSecret = s.ClientSecret,
                PaymentIntentId = s.PaymentIntentId,
                Placena = s.Placena,
                StavkaPorudzbines = s.StavkaPorudzbines
            .Select(sp => new StavkaPorudzbine
            {
                StavkaPorudzbineId = sp.StavkaPorudzbineId,
                BrojStavki = sp.BrojStavki,
                VelicinaDresaId = sp.VelicinaDresaId,
                PorudzbinaId = sp.PorudzbinaId,
            })
            .ToList()
            }).FirstOrDefault();

            if(porudzbina == null)
            {
                return null;
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;
            if (string.IsNullOrEmpty(porudzbina.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)porudzbina.UkupanIznos,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                porudzbina.PaymentIntentId = intent.Id;
                porudzbina.ClientSecret = intent.ClientSecret;

            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)porudzbina.UkupanIznos,
                };
                await service.UpdateAsync(porudzbina.PaymentIntentId, options);
            }

            _context.Set<Porudzbina>().Attach(porudzbina);
            _context.Entry(porudzbina).State = EntityState.Modified;
            _context.SaveChanges();
            
            return _mapper.Map<Porudzbina, PorudzbinaPaymentDto>(porudzbina);

        }

        public async Task<Porudzbina> UpdatePorudzbinaPaymentFailed(string paymentIntentId)
        {
            var porudzbina = _context.Porudzbinas.Include(k => k.Kupac).Include(sp => sp.StavkaPorudzbines).
                Where(p => p.PaymentIntentId == paymentIntentId).Select(p => new Porudzbina
                {
                    PorudzbinaId = p.PorudzbinaId,
                    DatumAzuriranja = p.DatumAzuriranja,
                    DatumKreiranja = p.DatumKreiranja,
                    Placena = p.Placena,
                    UkupanIznos = p.UkupanIznos,
                    PaymentIntentId = p.PaymentIntentId,
                    ClientSecret = p.ClientSecret,
                    Kupac = p.Kupac,
                    StavkaPorudzbines = p.StavkaPorudzbines
                    .Select(sp => new StavkaPorudzbine
                    {
                        StavkaPorudzbineId = sp.StavkaPorudzbineId,
                        BrojStavki = sp.BrojStavki,
                        VelicinaDresaId = sp.VelicinaDresaId
                    }).ToList()

                }).FirstOrDefault();

            if (porudzbina != null)
            {
                porudzbina.PorudzbinaId = porudzbina.PorudzbinaId;
                porudzbina.DatumKreiranja = porudzbina.DatumKreiranja;
                porudzbina.Placena = false;
                porudzbina.UkupanIznos = porudzbina.UkupanIznos;
                porudzbina.DatumAzuriranja = porudzbina.DatumAzuriranja;
                porudzbina.KupacId = porudzbina.KupacId;

                _context.Set<Porudzbina>().Attach(porudzbina);
                _context.Entry(porudzbina).State = EntityState.Modified;
                _context.SaveChanges();
                return porudzbina;
            }
            else
            {
                return null;
            }
        }

        public async Task<Porudzbina> UpdatePorudzbinaPaymentSucceeded(string paymentIntentId)
        {
            var porudzbina = _context.Porudzbinas.Include(k => k.Kupac).Include(sp => sp.StavkaPorudzbines).
                Where(p => p.PaymentIntentId == paymentIntentId).Select(p => new Porudzbina
                {
                    PorudzbinaId = p.PorudzbinaId,
                    DatumAzuriranja = p.DatumAzuriranja,
                    DatumKreiranja = p.DatumKreiranja,
                    Placena = p.Placena,
                    UkupanIznos = p.UkupanIznos,
                    PaymentIntentId = p.PaymentIntentId,
                    ClientSecret = p.ClientSecret,
                    Kupac = p.Kupac,
                    StavkaPorudzbines = p.StavkaPorudzbines
                    .Select(sp => new StavkaPorudzbine
                    {
                        StavkaPorudzbineId = sp.StavkaPorudzbineId,
                        BrojStavki = sp.BrojStavki,
                        VelicinaDresaId = sp.VelicinaDresaId
                    }).ToList()

                }).FirstOrDefault();

            if(porudzbina!= null)
            {
                porudzbina.PorudzbinaId = porudzbina.PorudzbinaId;
                porudzbina.DatumKreiranja = DateTime.UtcNow;
                porudzbina.Placena = true;
                porudzbina.UkupanIznos = porudzbina.UkupanIznos;
                porudzbina.DatumAzuriranja = porudzbina.DatumAzuriranja;
                porudzbina.KupacId = porudzbina.KupacId;

                 _context.Set<Porudzbina>().Attach(porudzbina);
                 _context.Entry(porudzbina).State = EntityState.Modified;
                 _context.SaveChanges();
                return porudzbina;
            }
            else
            {
                return null;
            }
        }
        
    }
}

