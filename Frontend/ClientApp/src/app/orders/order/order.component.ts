import { Component, OnInit } from '@angular/core';
import { CartService } from 'src/app/cart/cart.service';
import { Porudzbina } from 'src/app/shared/models/porudzbina';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit{
porudzbine: Porudzbina[] =[]

constructor(private cartService: CartService){}

ngOnInit(): void {
  this.cartService.getPorudzbineByKupacId().subscribe(res=>{
    this.porudzbine= res;
  
  })
}
}
