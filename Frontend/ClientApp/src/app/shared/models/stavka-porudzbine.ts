import { Porudzbina } from "./porudzbina";
import { VelicinaDresa } from "./velicina-dresa";

export class StavkaPorudzbine {
    stavkaPorudzbineId: string;
    brojStavki: number;
    porudzbina: Porudzbina;
    porudzbinaId: string;
    velicinaDresa: VelicinaDresa;
    velicinaDresaId: string;

    constructor(
        stavkaPorudzbineId: string,
        brojStavki: number,
        porudzbina: Porudzbina,
        porudzbinaId: string,
        velicinaDresa: VelicinaDresa,
        velicinaDresaId: string
    ) {
        this.stavkaPorudzbineId = stavkaPorudzbineId;
        this.brojStavki = brojStavki;
        this.porudzbina = porudzbina;
        this.porudzbinaId = porudzbinaId;
        this.velicinaDresa = velicinaDresa;
        this.velicinaDresaId = velicinaDresaId;
    }
}
    export class StavkaPorudzbineCreation {
        brojStavki: number;
        porudzbinaId: string | null;
        velicinaDresaId: string;
    
        constructor(
            brojStavki: number,
            porudzbinaId: string,
            velicinaDresaId: string
        ) {
            this.brojStavki = brojStavki;
            this.porudzbinaId = porudzbinaId;
            this.velicinaDresaId = velicinaDresaId;
        }
    
}

export class StavkaPorudzbineUpdate {
    stavkaPorudzbineId: string;
    brojStavki: number;
    

    constructor(
        stavkaPorudzbineId: string,
        brojStavki: number

       
        
    ) {
        this.stavkaPorudzbineId=stavkaPorudzbineId
        this.brojStavki = brojStavki;
        
    }

}
