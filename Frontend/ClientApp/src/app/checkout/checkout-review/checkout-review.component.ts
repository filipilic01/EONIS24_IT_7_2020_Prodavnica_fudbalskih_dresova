import { Component, OnInit } from '@angular/core';
import { CartService } from 'src/app/cart/cart.service';
import { StavkaPorudzbine } from 'src/app/shared/models/stavka-porudzbine';
import { ShopService } from 'src/app/shop/shop.service';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {
  stavke: StavkaPorudzbine[] = []
  total = 0
    constructor(private cartService: CartService, private shopService: ShopService){
  
    }

    
  ngOnInit(): void {
    const porudzbinaId = localStorage.getItem('porudzbina') ?? '';
    if(porudzbinaId === ''){
      console.log('hello');
    }
    else{
      this.cartService.getStavkeByPorudzbinaId().subscribe(res => {
      this.stavke=res
      this.total=0
      this.stavke.forEach(element => {
        this.total= this.total +  (element.brojStavki*element.velicinaDresa.dres.cena)
      });
    })

    }
    
   
  }

  formatPrice(price: number | undefined) {
    if(price)
    return price.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&.').replace('.', ',');
  else{
    return 0
  }
  }
}
