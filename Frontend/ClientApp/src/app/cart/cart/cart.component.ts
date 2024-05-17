import { Component, OnInit } from '@angular/core';
import { CartService } from '../cart.service';
import { ShopService } from 'src/app/shop/shop.service';
import { StavkaPorudzbine, StavkaPorudzbineUpdate } from 'src/app/shared/models/stavka-porudzbine';
import { VelicinaDresa } from 'src/app/shared/models/velicina-dresa';
import { ToastrService } from 'ngx-toastr';
import { Porudzbina, PorudzbinaUpdate } from 'src/app/shared/models/porudzbina';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
stavke: StavkaPorudzbine[] = []
total = 0
  constructor(private cartService: CartService, private shopService: ShopService, private toastr: ToastrService, private router: Router){

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

  addItemToCart(stavka: StavkaPorudzbine){
    if(stavka.velicinaDresa.kolicina == 0){
      this.toastr.warning("Nema više izabrane veličine na stanju!")
    }
    else{
      this.cartService.updateStavkaPorudzbine(new StavkaPorudzbineUpdate(stavka.stavkaPorudzbineId, ++stavka.brojStavki)).subscribe(res => {
        console.log(res);
        this.ngOnInit();
      })
      
    }
  }

  removeItemFromCart(stavka: StavkaPorudzbine){
    if(stavka.brojStavki == 1){
      this.cartService.deleteStavkaPorudzbine(stavka.stavkaPorudzbineId).subscribe(res=> {
        this.ngOnInit();
      })
    }else{
      this.cartService.updateStavkaPorudzbine(new StavkaPorudzbineUpdate(stavka.stavkaPorudzbineId, --stavka.brojStavki)).subscribe(res => {
      console.log(res);
      this.ngOnInit();
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

  toCheckout() {
    const porudzbina = localStorage.getItem('porudzbina');
    if (porudzbina !== null) {
      const now = new Date(); 
      const isoString = now.toISOString(); 
      this.cartService.updatePorudzbina(new PorudzbinaUpdate(porudzbina, this.total+300,"0001-01-01T00:00:00",isoString, false)).subscribe(res=>{
        this.router.navigate(['/checkout'], { queryParams: { from: 'toCheckout' } });
      })
    }
  }
  

  
}
