import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement, loadStripe } from '@stripe/stripe-js';
import { ToastrService } from 'ngx-toastr';
import { CartService } from 'src/app/cart/cart.service';
import { PorudzbinaUpdate } from 'src/app/shared/models/porudzbina';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {

@Input() checkoutForm?: FormGroup
@ViewChild('cardNumber') cardNumberElement?: ElementRef
@ViewChild('cardExpiry') cardExpiryElement?: ElementRef
@ViewChild('cardCvc') cardCvcElement?: ElementRef
stripe: Stripe | null = null
cardNumber?: StripeCardNumberElement
cardExpiry?: StripeCardExpiryElement
cardCvc?: StripeCardCvcElement
cardNumberComplete = false
cardExpiryComplete= false
cardCvcComplete= false
cardErrors:any
total = 0
secret: string = ''


constructor(private cartService: CartService, private router: Router, private toastr: ToastrService){}

ngOnInit(): void {
  loadStripe('pk_test_51PGhCrDdZKU4JlSP6Q7TeKxWMhpDsTSsoVQmdXVdTHwuQCEGEdzd1sBuzbbWa7LTL9C6BwRzdloiJV5NF2yzLJXr00kQOnq8aT').then(stripe => {
    this.stripe=stripe;
    const elements = stripe?.elements();
    if (elements){
      this.cardNumber = elements.create('cardNumber');
      this.cardNumber.mount(this.cardNumberElement?.nativeElement);
      this.cardNumber.on('change', event => {
        this.cardNumberComplete = event.complete
        if (event.error) this.cardErrors= event.error.message
        else this.cardErrors=null
      })

      this.cardExpiry = elements.create('cardExpiry');
      this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
      this.cardExpiry.on('change', event => {
        this.cardExpiryComplete = event.complete
        if (event.error) this.cardErrors= event.error.message
        else this.cardErrors=null
      })

      this.cardCvc = elements.create('cardCvc');
      this.cardCvc.mount(this.cardCvcElement?.nativeElement);
      this.cardCvc.on('change', event => {
        this.cardCvcComplete = event.complete
        if (event.error) this.cardErrors= event.error.message
        else this.cardErrors=null
      })
    }
  })
}
get paymentFormComplete(){
  return this.checkoutForm?.get('paymentForm')?.valid 
  && this.cardNumberComplete && this.cardExpiryComplete && this.cardCvcComplete
}
submitOrder(){
  
  const porudzbina = localStorage.getItem('porudzbina');
    if (porudzbina !== null) {

      this.cartService.getPorudzbina().subscribe(res=>{
        this.total=res.ukupanIznos;
        if (res.clientSecret !== null) {
          this.secret = res.clientSecret;
        }

        
          this.stripe?.confirmCardPayment(this.secret, {
            payment_method: {
              card: this.cardNumber!,
              billing_details: {
                name: this.checkoutForm?.get('paymentForm')?.get('nameOnCard')?.value
              }
            }
          }).then(result => {
            console.log(result);
            if (result.paymentIntent){
              
              localStorage.removeItem('porudzbina')
            this.cartService.brojStavki=0
            
            this.router.navigateByUrl('/checkout/success');
             
            }
            else{
              this.toastr.error(result.error.message)
            }
          
          })
      })
      
      
    }
}

}
