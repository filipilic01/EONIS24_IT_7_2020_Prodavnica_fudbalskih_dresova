import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { CartService } from './cart/cart.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements  OnInit {
  title = 'ClientApp';

  constructor(private accountService: AccountService, private cartService: CartService){}

  ngOnInit(): void {
    const porudzbinaId = localStorage.getItem('porudzbina');
    if(porudzbinaId != null){
      this.cartService.getStavkeByPorudzbinaId().subscribe(res=>{
          
    })
    }
    
    this.loadCurrentUser();  
  }

  loadCurrentUser(){
    const token = localStorage.getItem('token');
    
      this.accountService.loadCurrentUser(token).subscribe(res=>{
      });
    
  }

}
