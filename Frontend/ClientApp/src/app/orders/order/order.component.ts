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
  if(localStorage.getItem('role')=== 'Admin'){
    this.cartService.getPorudzbine().subscribe(res=>{
    this.porudzbine=res
    })
  }
  else if(localStorage.getItem('role')=== 'Kupac'){
    this.cartService.getPorudzbineByKupacId().subscribe(res=>{
    this.porudzbine= res;
  
  })
  }
  else{
    console.log('d')
  }
  
}
}
