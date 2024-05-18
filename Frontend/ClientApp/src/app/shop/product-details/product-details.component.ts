import { Component, OnInit } from '@angular/core';
import { Dres } from 'src/app/shared/models/dres';
import { ShopService } from '../shop.service';
import { ActivatedRoute, Router } from '@angular/router';
import { VelicinaDresa } from 'src/app/shared/models/velicina-dresa';
import { ToastrService } from 'ngx-toastr';
import { CartService } from 'src/app/cart/cart.service';
import { PorudzbinaCreation } from 'src/app/shared/models/porudzbina';
import { StavkaPorudzbine, StavkaPorudzbineCreation, StavkaPorudzbineUpdate } from 'src/app/shared/models/stavka-porudzbine';

import { BreadcrumbService } from 'xng-breadcrumb';
import { provideCloudflareLoader } from '@angular/common';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
dres?: Dres
velicine: VelicinaDresa[] = []
slike: string[]=[]
selectedVelicina: VelicinaDresa | null = null;
selectedQuantity: number = 1;
temp: number = 0
temp2: number = 0
temp3: number=0
stavke: StavkaPorudzbine[] =[]
currentId = ''



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
    this.getDres(id);
    this.getVelicineByDresId(id);
    this.getStavkeByPorudzbinaId();

  }
  }

  getDres(id: string){
    
    if (id) this.shopService.getDres(id).subscribe({
      next: dres => {
        this.dres=dres,
        this.bcService.set('@productDetails', dres.tim + ' ' + dres.imeIgraca + ' ' + dres.sezona + ' ' + dres.tip )
        if (this.dres && this.dres.slikaUrl) {
    
          const parts: string[] = this.dres.slikaUrl.split('#');
          
          if (parts && parts.length > 0) {
             
              this.slike = parts;
             
              
              
              console.log(this.slike); 
          }
        }
      
      },
      error: error => console.log(error)
    })
  }

  getVelicineByDresId(id: string){
    this.shopService.getVelicineByDresId(id).subscribe({
      next: response => {
       
        this.velicine = this.sortVelicine(response);
      }
    })
  }

  getStavkeByPorudzbinaId(){
    const porudzbinaId = localStorage.getItem('porudzbina') ?? '';
    if(porudzbinaId === ''){
      console.log('hello');
    }
    else{
      this.cartService.getStavkeByPorudzbinaId().subscribe(res=>{
        this.stavke=res;
        console.log(this.stavke);
    })
    }
    
  }

  onButtonGroupClick(event: any, velicina:VelicinaDresa){
    let clickedElement = event.target || event.srcElement;
    
    if( clickedElement.nodeName === "BUTTON" ) {
  
      let isCertainButtonAlreadyActive = clickedElement.parentElement.querySelector(".active");
     
      if( isCertainButtonAlreadyActive ) {
        isCertainButtonAlreadyActive.classList.remove("active");
      }
  
      clickedElement.className += " active";
    }
  
    this.selectedVelicina = velicina
  }

  sortVelicine(velicine: VelicinaDresa[]): VelicinaDresa[] {
    const customOrder = ["XS", "S", "M", "L", "XL", "XXL", "XXXL"];
    return velicine
      .filter(velicina => velicina.kolicina > 0) 
      .sort((a, b) => {
        const indexA = customOrder.indexOf(a.velicinaDresaVrednost);
        const indexB = customOrder.indexOf(b.velicinaDresaVrednost);
        
        if (indexA === -1 && indexB === -1) {
          return a.velicinaDresaVrednost.localeCompare(b.velicinaDresaVrednost); 
        }
        if (indexA === -1) return 1; 
        if (indexB === -1) return -1; 
        return indexA - indexB; 
      });
  }

  generateNumberArray(n: number): number[] {
    return Array.from({length: n}, (_, i) => i + 1);
  }
 
  onQuantityChange(event: any): void {
    this.selectedQuantity = event.target.value;
    
}
  addItemToCart(){
    if(this.selectedVelicina === null){
      this.toastr.warning("Molimo Vas izaberite odredjenu velicinu")
    }
    else{
      const porudzbinaId = localStorage.getItem('porudzbina') ?? '';
      const kupacId = localStorage.getItem('userId')?? '';
      if(kupacId === ''){
        this.router.navigateByUrl('account/login');
      }
      else{
        if(porudzbinaId === ''){
        
          this.cartService.createPorudzbina(new PorudzbinaCreation(0,"0001-01-01T00:00:00","0001-01-01T00:00:00",false, kupacId)).subscribe(res => {
          
          localStorage.setItem('porudzbina', res.porudzbinaId ?? '');
          const selectedPorudzbinaId = localStorage.getItem('porudzbina') ?? '';
          if(this.selectedVelicina?.velicinaDresaId)
          this.cartService.createStavkaPorudzbine(new StavkaPorudzbineCreation(this.selectedQuantity, selectedPorudzbinaId, this.selectedVelicina.velicinaDresaId)).subscribe(res => {
        
            this.router.navigateByUrl('/cart');
          })
         
        });
      }
      else{
        const selectedPorudzbinaId = localStorage.getItem('porudzbina') ?? '';
        this.stavke.forEach(element => {
          if(element.velicinaDresa.velicinaDresaId==this.selectedVelicina?.velicinaDresaId){
            this.currentId=this.selectedVelicina.velicinaDresaId
              this.temp = parseInt(element.brojStavki.toString())
              console.log(typeof(this.temp))
              this.temp3= parseInt(this.selectedQuantity.toString());
              console.log(typeof(this.temp3))
              this.temp2 = this.temp3 + this.temp
              console.log(this.temp2)
            element.brojStavki= this.selectedQuantity+element.brojStavki
            this.cartService.updateStavkaPorudzbine(new StavkaPorudzbineUpdate(element.stavkaPorudzbineId, this.temp2)).subscribe(res => {
              this.router.navigateByUrl('/cart');
            })
          }
        });

         
        if(this.currentId === ''){
          this.cartService.createStavkaPorudzbine(new StavkaPorudzbineCreation(this.selectedQuantity, selectedPorudzbinaId, this.selectedVelicina.velicinaDresaId)).subscribe(res => {
        
          this.router.navigateByUrl('/cart');
      })
        } 
      }
      }
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
