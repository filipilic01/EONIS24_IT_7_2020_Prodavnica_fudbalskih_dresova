using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Permissions;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private IPaymentService _paymentService;
        private const string WhSecret = "whsec_11ed931cb5833f10330676d7655758671a2f0b038bf4ec2c0bf5bec0db3481d6";
        private ILogger<PaymentController> _logger;
        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger) 
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("{porudzbinaId}")]
        public async Task<ActionResult<PorudzbinaPaymentDto>> CreateOrUpdatePaymentIntent(Guid porudzbinaId)
        {
            return await _paymentService.CreateOrUpdatePaymentIntent(porudzbinaId);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent intent;
            Porudzbina porudzbina;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment succeeded: ", intent.Id);
                    porudzbina = await _paymentService.UpdatePorudzbinaPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: ", porudzbina.PorudzbinaId);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment failed: ", intent.Id);
                    porudzbina = await _paymentService.UpdatePorudzbinaPaymentFailed(intent.Id);
                    _logger.LogInformation("Order updated to payment failed: ", porudzbina.PorudzbinaId);
                    break;
             

            }

            return new EmptyResult();
        }
        
    }
}
