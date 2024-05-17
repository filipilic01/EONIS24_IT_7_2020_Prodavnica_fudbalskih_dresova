import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Dres } from 'src/app/shared/models/dres';
import { ShopRoutingModule } from 'src/app/shop/shop-routing-module';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ShopService } from 'src/app/shop/shop.service';

import { ProductDialogComponent } from '../dialogs/product-dialog/product-dialog.component';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit{
  @ViewChild('search') searchTerm?: ElementRef
  bsModalRef: BsModalRef | undefined;
  dresovi:Dres[] = []
  seasonSelected = ''
  typeSelected = ''
  sortSelected = ''
  totalCount = 0
  pageSize = 1000
  pageIndex = 1
  search = ''
constructor(private shopService: ShopService, private modalService: BsModalService){
  
}

  ngOnInit(): void {
    this.shopService.getDresovi(false, this.seasonSelected, this.typeSelected, this.sortSelected,this.pageSize, this.pageIndex,this.search).subscribe({
      next: response => {
        this.dresovi = response.data
        this.pageSize = response.pageSize
        this.pageIndex = response.pageIndex
        this.totalCount = response.count
      },
      error: error => console.error()
      
    })  
  }

  openEditDialog(dres: Dres){
    this.shopService.getVelicineByDresId(dres.dresId).subscribe(res=> {
      const initialState = {
        dresId: dres.dresId,
        imeIgraca: dres.imeIgraca,
        tim: dres.tim,
         sezona: dres.sezona,
         brend: dres.brend,
         cena: dres.cena,
         slikaUrl: dres.slikaUrl,
         tip: dres.tip,
         zemlja: dres.zemlja,
         takmicenje: dres.takmicenje,
         status:dres.status,
         timUrl: dres.timUrl,
         adminId: dres.adminId,
         flag: 2,
         velicine: res
      };
      this.bsModalRef = this.modalService.show(ProductDialogComponent, {initialState});
      this.bsModalRef.onHidden?.subscribe(() => {
        this.ngOnInit()
      })
    })

    
    
  }

  openAddDialog(){
    this.bsModalRef = this.modalService.show(ProductDialogComponent, {
      initialState:{
        flag: 1
      }
    });
    this.bsModalRef.onHidden?.subscribe(() => {
      this.ngOnInit()
    })
  }

  openDeleteDialog(dres: Dres){
    this.shopService.getVelicineByDresId(dres.dresId).subscribe(res=> {
      const initialState = {
        dresId: dres.dresId,
        imeIgraca: dres.imeIgraca,
        tim: dres.tim,
         sezona: dres.sezona,
         brend: dres.brend,
         cena: dres.cena,
         slikaUrl: dres.slikaUrl,
         tip: dres.tip,
         zemlja: dres.zemlja,
         takmicenje: dres.takmicenje,
         status:dres.status,
         timUrl: dres.timUrl,
         adminId: dres.adminId,
         flag: 3,
         velicine: res
      };
      this.bsModalRef = this.modalService.show(ProductDialogComponent, {initialState});
      this.bsModalRef.onHidden?.subscribe(() => {
        this.ngOnInit()
      })
    })

  }
  onSearch(){
    this.search = this.searchTerm?.nativeElement.value;
    this.pageIndex =1
    this.ngOnInit()
  }
  
  onReset(){
    if(this.searchTerm){
      this.searchTerm.nativeElement.value=''
    }
  
    this.pageSize=1000
    this.pageIndex=1
    this.search=''
    
    this.ngOnInit()
  }
 


}
