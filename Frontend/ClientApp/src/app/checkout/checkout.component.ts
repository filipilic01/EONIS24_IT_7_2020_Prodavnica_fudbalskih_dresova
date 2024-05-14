import { Component, OnInit } from '@angular/core';
import { CartService } from '../cart/cart.service';
import { Porudzbina } from '../shared/models/porudzbina';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit{
porudzbina?: Porudzbina
price =0

adresa: string[]=[]


  constructor(private cartService: CartService, private fb:FormBuilder, private accountService: AccountService){}
  
  checkoutForm = this.fb.group({
    addressForm: this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      street: ['', Validators.required],
      streetNumber: ['', Validators.required],
      city: ['', Validators.required],
      zipcode : ['', Validators.required],
      phoneNumber: ['', Validators.required]
    }),
    deliveryForm:this.fb.group({
      deliveryType: ['', Validators.required]
    }),
    paymentForm: this.fb.group({
      nameOnCard: ['', Validators.required]
    })
  })
  

  ngOnInit(): void {
    this.getAddress();
    this.cartService.getPorudzbina().subscribe(res=> {
      this.porudzbina=res;
      this.price=this.porudzbina.ukupanIznos
     // this.price=this.formatPrice(this.porudzbina.ukupanIznos)
    })
  }

  formatPrice(price: number): string {
    if (price === undefined) {
        return ''; 
    }
    return price.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&.').replace('.', ',');
}

getAddress(){

  this.accountService.getKupac().subscribe({
    next: kupac => {
      if(kupac.kupacAdresa === ''){
        console.log(5456)
        kupac && this.checkoutForm.get('addressForm')?.get('firstName')?.patchValue(kupac.kupacIme)
              kupac && this.checkoutForm.get('addressForm')?.get('lastName')?.patchValue(kupac.kupacPrezime)
      }else{
        const parts: string[] = kupac.kupacAdresa.split(',');
          
          if (parts && parts.length > 0) {
             
              this.adresa = parts;
             

              kupac && this.checkoutForm.get('addressForm')?.get('street')?.patchValue(this.adresa[0])
              kupac && this.checkoutForm.get('addressForm')?.get('streetNumber')?.patchValue(this.adresa[1])
              kupac && this.checkoutForm.get('addressForm')?.get('zipcode')?.patchValue(this.adresa[2])
              kupac && this.checkoutForm.get('addressForm')?.get('city')?.patchValue(this.adresa[3])
              kupac && this.checkoutForm.get('addressForm')?.get('firstName')?.patchValue(kupac.kupacIme)
              kupac && this.checkoutForm.get('addressForm')?.get('lastName')?.patchValue(kupac.kupacPrezime)
              kupac && this.checkoutForm.get('addressForm')?.get('phoneNumber')?.patchValue(kupac.kupacBrojTelefona)
          }
      }
    }
  })
}


}
