import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { CartService } from 'src/app/cart/cart.service';
import { Porudzbina } from 'src/app/shared/models/porudzbina';
import { StavkaPorudzbine } from 'src/app/shared/models/stavka-porudzbine';
import { ShopService } from 'src/app/shop/shop.service';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit{
  
  stavke: StavkaPorudzbine[] =[]
  porudzbina?:Porudzbina
  constructor
  (private shopService: ShopService, 
  public accountService: AccountService,
   private activatedRoute: ActivatedRoute,
   private toastr: ToastrService,
   private cartService: CartService,
   private router: Router,
   private bcService: BreadcrumbService){
  
  }

  ngOnInit(): void {
    const id= this.activatedRoute.snapshot.paramMap.get('id')?.toString()
    if (id) {
      this.cartService.getPorudzbinaId(id).subscribe(res=>{
        this.porudzbina=res;
        
      });
      this.cartService.getStavkePorudzbineByPorudzbinaId(id).subscribe(res=>{
        this.stavke=res
      });
 
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
