import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Dres, DresCreation, DresUpdate } from 'src/app/shared/models/dres';
import { VelicinaDresa, VelicinaDresaCreation, VelicinaDresaUpdate } from 'src/app/shared/models/velicina-dresa';
import { ShopService } from 'src/app/shop/shop.service';

@Component({
  selector: 'app-product-dialog',
  templateUrl: './product-dialog.component.html',
  styleUrls: ['./product-dialog.component.scss']
})
export class ProductDialogComponent implements OnInit {

  options: number[] = Array.from({length: 100}, (_, i) => i);
  slike: string[]=[]
  dresCreated?: Dres
  dresId: string | undefined;
  imeIgraca: string | undefined;
  tim: string | undefined;
   sezona: string | undefined;
   brend: string | undefined;
   cena: number | undefined;
   obrisan: boolean | undefined
   slikaUrl: string | undefined
   tip: string | undefined
   zemlja: string | undefined
   takmicenje: string | undefined
   status: string | undefined
   timUrl: string | undefined
   adminId: string | undefined
   flag:number | undefined

   velicine: VelicinaDresa[] = []
   L: number | undefined
   XS: number | undefined
   S: number | undefined
   M: number | undefined
   XL: number | undefined
   XXL: number | undefined
   XXXL: number | undefined
   idL: string = ''
   idXS: string =''
   idS: string =''
   idM: string =''
   idXL: string =''
   idXXL: string =''
   idXXXL: string =''
   vel: string[] = ['XS', 'S', 'M', 'L', 'XL', 'XXL', 'XXXL']
  
  constructor(public bsModalRef: BsModalRef, public shopService: ShopService, private toastr: ToastrService) { }
  
  
  ngOnInit(): void {
    this.velicine= this.sortVelicine(this.velicine);
   this.velicine.forEach(element => {
    if(element.velicinaDresaVrednost == 'XS'){
       this.XS= element.kolicina
       this.idXS = element.velicinaDresaId
    }
   
    
    else if(element.velicinaDresaVrednost == 'S'){
        this.S= element.kolicina
        this.idS = element.velicinaDresaId
    }
    
    else if(element.velicinaDresaVrednost == 'M'){
        this.M= element.kolicina
        this.idM= element.velicinaDresaId
    }
    
    else if(element.velicinaDresaVrednost == 'L'){
        this.L= element.kolicina
        this.idL= element.velicinaDresaId
    }
   
    else if(element.velicinaDresaVrednost == 'XL'){
        this.XL= element.kolicina
        this.idXL= element.velicinaDresaId
    }
    
    else if(element.velicinaDresaVrednost == 'XXL'){
        this.XXL= element.kolicina
        this.idXXL= element.velicinaDresaId
    }
    
    else if(element.velicinaDresaVrednost == 'XXXL'){
        this.XXXL=element.kolicina
        this.idXXXL= element.velicinaDresaId
    }
    

   });
    
    
    if ( this.slikaUrl) {
    
      const parts: string[] = this.slikaUrl.split('#');
      
      if (parts && parts.length > 0) {
         
          this.slike = parts;
          
          
      }
    }
  }

  sortVelicine(velicine: VelicinaDresa[]): VelicinaDresa[] {
    const customOrder = ["XS", "S", "M", "L", "XL", "XXL", "XXXL"];
    return velicine.sort((a, b) => {
      return customOrder.indexOf(a.velicinaDresaVrednost) - customOrder.indexOf(b.velicinaDresaVrednost);
    });
  }
  closeModal() {
    this.bsModalRef.hide();
  }

  saveModal(){

   this.slikaUrl=this.slike.join('#');
   const userId = localStorage.getItem('userId');
  if (userId !== null) {
      this.adminId = userId;
  }

    if(this.flag === 1){
      if (this.imeIgraca && this.tim && this.sezona && this.brend && this.cena && this.slikaUrl && this.tip && this.zemlja && this.takmicenje && this.status && this.timUrl && this.adminId && this.XS !== undefined && this.S !== undefined && this.L !== undefined && this.M !== undefined && this.XL !== undefined && this.XXL !== undefined && this.XXXL !== undefined) {
        if(this.cena < 0){
          this.toastr.error("Cena mora biti veca od 0");
        }
        else{
           this.shopService.addDres(new DresCreation(this.imeIgraca, this.tim, this.sezona, this.brend, this.cena, this.slikaUrl, this.tip, false, this.zemlja, this.takmicenje, this.status, this.timUrl, this.adminId)).subscribe(res => {
          this.dresCreated = res;
          console.log(this.dresCreated)
          if (this.dresCreated.dresId) {
            
            this.shopService.addVelicinaDresa(new VelicinaDresaCreation('XS', this.XS!, this.dresCreated.dresId)).subscribe(res=>{
              console.log("success1")
            })
            this.shopService.addVelicinaDresa(new VelicinaDresaCreation('S', this.S!, this.dresCreated.dresId)).subscribe(res=>{
              console.log("success2")
            })
            this.shopService.addVelicinaDresa(new VelicinaDresaCreation('L', this.L!, this.dresCreated.dresId)).subscribe(res=>{
              console.log("success3")
            })
            this.shopService.addVelicinaDresa(new VelicinaDresaCreation('XL', this.XL!, this.dresCreated.dresId)).subscribe(res=>{
              console.log("success4")
            })
            this.shopService.addVelicinaDresa(new VelicinaDresaCreation('XXL', this.XXL!, this.dresCreated.dresId)).subscribe(res=>{
              console.log("success5")
            })
            this.shopService.addVelicinaDresa(new VelicinaDresaCreation('M', this.M!, this.dresCreated.dresId)).subscribe(res=>{
              console.log("success6")
            })
            this.shopService.addVelicinaDresa(new VelicinaDresaCreation('XXXL', this.XXXL!, this.dresCreated.dresId)).subscribe(res=>{
              console.log("success7")
              this.bsModalRef.hide();
              
            })
          }
        });
        }
        
       
      } else {
        this.toastr.error('Neki od obaveznih podataka nisu uneti.');
      }
      
    }
    else{
      if (this.dresId && this.imeIgraca && this.tim && this.sezona && this.brend
         && this.cena && this.slikaUrl && this.tip && this.zemlja && this.takmicenje && 
         this.status && this.timUrl && this.XS !== undefined && this.S !== undefined && this.L !== undefined 
         && this.M !== undefined &&  this.XL !== undefined && this.XXL !== undefined && this.XXXL !== undefined 
         ) {
          if(this.cena<0){
            this.toastr.error("Cena ne sme biti manja od 0")
          }
          else{
            this.shopService.updateDres(new DresUpdate(this.dresId, this.imeIgraca, this.tim, this.sezona, this.brend, this.cena, this.slikaUrl, this.tip,false, this.zemlja, this.takmicenje, this.status, this.timUrl)).subscribe(res => {
          this.dresCreated = res;
          console.log(this.dresCreated)
          if (this.dresCreated.dresId) {
            
            this.shopService.updateVelicinaDresa(new VelicinaDresaUpdate(this.idXS,'XS',this.XS!)).subscribe(res=>{
              console.log("success1")
            })
            this.shopService.updateVelicinaDresa(new VelicinaDresaUpdate(this.idS,'S',this.S!)).subscribe(res=>{
              console.log("success2")
            })
            this.shopService.updateVelicinaDresa(new VelicinaDresaUpdate(this.idM,'M', this.M!)).subscribe(res=>{
              console.log("success3")
            })
            this.shopService.updateVelicinaDresa(new VelicinaDresaUpdate(this.idXL,'XL',this.XL!)).subscribe(res=>{
              console.log("success4")
            })
            this.shopService.updateVelicinaDresa(new VelicinaDresaUpdate(this.idL,'L',this.L!)).subscribe(res=>{
              console.log("success5")
            })
            this.shopService.updateVelicinaDresa(new VelicinaDresaUpdate(this.idXXL,'XXL', this.XXL!)).subscribe(res=>{
              console.log("success6")
            })
            this.shopService.updateVelicinaDresa(new VelicinaDresaUpdate(this.idXXXL,'XXXL',this.XXXL!)).subscribe(res=>{
              console.log("success7")
              this.bsModalRef.hide();
              
            })
          }
        });
          }
        
      } else {
        this.toastr.error('Neki od obaveznih podataka nisu uneti.');
      }
    }
  }

  deleteModal(){
    if(this.dresId && this.imeIgraca && this.tim && this.sezona && this.brend
      && this.cena && this.slikaUrl && this.tip && this.zemlja && this.takmicenje && 
      this.status && this.timUrl && this.XS !== undefined && this.S !== undefined && this.L !== undefined 
      && this.M !== undefined &&  this.XL !== undefined && this.XXL !== undefined && this.XXXL !== undefined 
      ){
      this.shopService.updateDres(new DresUpdate(this.dresId, this.imeIgraca, this.tim, this.sezona, this.brend, this.cena, this.slikaUrl, this.tip,true, this.zemlja, this.takmicenje, this.status, this.timUrl)).subscribe(res => {
        this.dresCreated=res
        this.toastr.success("Uspešno obrisan dres!");

        this.ngOnInit()
        this.bsModalRef.hide();
      })
    }
    
  }
  
}
