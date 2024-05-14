import { Component, OnInit } from '@angular/core';
import { CartService } from '../cart/cart.service';
import { Porudzbina } from '../shared/models/porudzbina';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit{
porudzbina?: Porudzbina
price =''

  constructor(private cartService: CartService, private fb:FormBuilder){}
  
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
    paymentForm: this.fb.group({
      nameOnCard: ['', Validators.required]
    })
  })
  

  ngOnInit(): void {
    this.cartService.getPorudzbina().subscribe(res=> {
      this.porudzbina=res;
      this.price=this.formatPrice(this.porudzbina.ukupanIznos)
    })
  }


  formatPrice(price: number): string {
    if (price === undefined) {
        return ''; 
    }
    return price.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&.').replace('.', ',');
}


}
