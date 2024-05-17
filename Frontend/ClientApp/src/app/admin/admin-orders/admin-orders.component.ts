import { Component, OnInit } from '@angular/core';
import { CartService } from 'src/app/cart/cart.service';
import { Porudzbina } from 'src/app/shared/models/porudzbina';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit{
porudzbine: Porudzbina[]=[]

  constructor(private cartService: CartService){
  }

  ngOnInit(): void {
    this.cartService.getPorudzbine().subscribe(res=>{
      this.porudzbine=res;
    })
  }


}
