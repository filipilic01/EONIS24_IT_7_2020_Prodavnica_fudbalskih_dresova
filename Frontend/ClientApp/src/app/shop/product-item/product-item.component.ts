import { Component, Input, OnInit } from '@angular/core';
import { Dres } from 'src/app/shared/models/dres';
import { ShopService } from '../shop.service';
import { VelicinaDresa } from 'src/app/shared/models/velicina-dresa';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent implements OnInit{
@Input() dres?: Dres;

velicine: VelicinaDresa[] = []
velCopy: VelicinaDresa[] =[]
slike: string[] = []
constructor(private shopService: ShopService){
  
}

ngOnInit(): void {
  if (this.dres && this.dres.slikaUrl) {
    
    const parts: string[] = this.dres.slikaUrl.split('#');
    
    if (parts && parts.length > 0) {
       
        this.slike = parts;
        
        
    }
  }
  if(this.dres?.dresId){
    this.shopService.getVelicineByDresId(this.dres.dresId).subscribe({
    next: response => {
      
      this.velicine = this.sortVelicine(response);
    }
  });
  }
  
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

formatPrice(price: number) {
  return price.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&.').replace('.', ',');
}

}
