using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<PorudzbinaPaymentDto> CreateOrUpdatePaymentIntent(Guid porudzbinaId);

        Task<Porudzbina> UpdatePorudzbinaPaymentSucceeded(string paymentIntentId);

        Task<Porudzbina> UpdatePorudzbinaPaymentFailed(string paymentIntentId);
    }
}
