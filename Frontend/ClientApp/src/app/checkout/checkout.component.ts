import { Component, OnInit } from '@angular/core';
import { CartService } from '../cart/cart.service';
import { Porudzbina } from '../shared/models/porudzbina';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit{
porudzbina?: Porudzbina

  constructor(private cartService: CartService){}

  ngOnInit(): void {
    this.cartService.getPorudzbina().subscribe(res=> {
      this.porudzbina=res;
    })
  }


}
