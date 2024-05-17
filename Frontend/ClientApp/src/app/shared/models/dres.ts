import { Admin } from "./admin";

export class Dres {
       dresId: string
        imeIgraca: string
        tim: string
         sezona: string
         brend: string
         cena: number
         slikaUrl: string
         tip: string
         obrisan: boolean
         zemlja: string
         takmicenje: string
         status: string
         timUrl: string
         admin: Admin
         adminId: string
        
         
         constructor(dresId: string, imeIgraca: string, tim:string, sezona: string, brend: string, cena: number, slikaUrl: string, tip: string,
            obrisan: boolean, zemlja: string, takmicenje: string, status: string, timUrl: string, admin: Admin, adminId: string){
                this.dresId=dresId
                this.imeIgraca=imeIgraca;
                this.tim=tim;
                this.sezona=sezona;
                this.brend=brend;
                this.cena=cena;
                this.slikaUrl=slikaUrl;
                this.tip=tip
                this.obrisan=obrisan
                this.zemlja=zemlja;
                this.takmicenje=takmicenje;
                this.status=status;
                this.timUrl=timUrl;
                this.admin=admin;
                this.adminId=adminId
            }
     
          
    

}


export class DresCreation {
   
     imeIgraca: string
     tim: string
      sezona: string
      brend: string
      cena: number
      slikaUrl: string
      tip: string
      obrisan: boolean
      zemlja: string
      takmicenje: string
      status: string
      timUrl: string
      adminId: string
     
      
      constructor( imeIgraca: string, tim:string, sezona: string, brend: string, cena: number, slikaUrl: string, tip: string,
        obrisan: boolean, zemlja: string, takmicenje: string, status: string, timUrl: string, adminId: string){
       
             this.imeIgraca=imeIgraca;
             this.tim=tim;
             this.sezona=sezona;
             this.brend=brend;
             this.cena=cena;
             this.slikaUrl=slikaUrl;
             this.tip=tip;
             this.obrisan= obrisan;
             this.zemlja=zemlja;
             this.takmicenje=takmicenje;
             this.status=status;
             this.timUrl=timUrl;
             this.adminId=adminId
         }
  
       
 

}

export class DresUpdate {
    dresId:string
    imeIgraca: string
    tim: string
     sezona: string
     brend: string
     cena: number
     slikaUrl: string
     tip: string
     obrisan: boolean
     zemlja: string
     takmicenje: string
     status: string
     timUrl: string
   
    
     
     constructor(dresId:string, imeIgraca: string, tim:string, sezona: string, brend: string, cena: number, slikaUrl: string, tip: string,
        obrisan: boolean, zemlja: string, takmicenje: string, status: string, timUrl: string){
            this.dresId=dresId
            this.imeIgraca=imeIgraca;
            this.tim=tim;
            this.sezona=sezona;
            this.brend=brend;
            this.cena=cena;
            this.slikaUrl=slikaUrl;
            this.tip=tip;
            this.obrisan = obrisan;
            this.zemlja=zemlja;
            this.takmicenje=takmicenje;
            this.status=status;
            this.timUrl=timUrl;
           
        }
 
      


}
  
       
 
