import { Dres } from "./dres";

export class VelicinaDresa {
    velicinaDresaId: string;
    velicinaDresaVrednost: string;
    kolicina: number;
    dres: Dres;
    dresId: string;
  
    constructor(
      velicinaDresaId: string,
      velicinaDresaVrednost: string,
      kolicina: number,
      dres: Dres,
      dresId: string
    ) {
      this.velicinaDresaId = velicinaDresaId;
      this.velicinaDresaVrednost = velicinaDresaVrednost;
      this.kolicina = kolicina;
      this.dres = dres;
      this.dresId = dresId;
    
    }
  }

  export class VelicinaDresaCreation {
   
    velicinaDresaVrednost: string;
    kolicina: number;
  
    dresId: string;
  
    constructor(
   
      velicinaDresaVrednost: string,
      kolicina: number,

      dresId: string
    ) {
   
      this.velicinaDresaVrednost = velicinaDresaVrednost;
      this.kolicina = kolicina;

      this.dresId = dresId;
    
    }
  }

  export class VelicinaDresaUpdate {
   velicinaDresaId: string
    velicinaDresaVrednost: string;
    kolicina: number;
  
    constructor(
      velicinaDresaId: string,
      velicinaDresaVrednost: string,
      kolicina: number,


    ) {
      this.velicinaDresaId=velicinaDresaId
      this.velicinaDresaVrednost = velicinaDresaVrednost;
      this.kolicina = kolicina;


    
    }
  }