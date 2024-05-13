import { Component } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';
import { CartService } from 'src/app/cart/cart.service';
import { StavkaPorudzbine } from 'src/app/shared/models/stavka-porudzbine';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {

  brojStavki: number | undefined;

  stavke: StavkaPorudzbine[]= []

  constructor(public accountService: AccountService, public cartService: CartService) {
    
}

logout(){
  const porudzbinaId = localStorage.getItem('porudzbina') ?? '';
  if(porudzbinaId === ''){
    this.accountService.logout();
  }
  else{
    this.cartService.getStavkeByPorudzbinaId().subscribe(res=>{
      res.forEach(element => {
        this.cartService.deleteStavkaPorudzbine(element.stavkaPorudzbineId).subscribe(res=>{
          
          
        })
      });
      this.cartService.deletePorudzbina().subscribe(res =>{
        this.accountService.logout();
      })
      
    })
  }
}

 
}
