import { Kupac } from "./kupac";

export class Porudzbina {
    porudzbinaId: string;
    ukupanIznos: number;
    datumPorudzbine: string | null;
    kupac: Kupac;
    kupacId: string;

    constructor(
        porudzbinaId: string,
        ukupanIznos: number,
        datumPorudzbine: string | null,
        kupac: Kupac,
        kupacId: string
    ) {
        this.porudzbinaId = porudzbinaId;
        this.ukupanIznos = ukupanIznos;
        this.datumPorudzbine = datumPorudzbine;
        this.kupac = kupac;
        this.kupacId = kupacId;
    }
}

export class PorudzbinaCreation {
    ukupanIznos: number;
    datumPorudzbine: string | null;
    kupacId: string;

    constructor(
        ukupanIznos: number,
        datumPorudzbine: string | null,
        kupacId: string
    ) {
        this.ukupanIznos = ukupanIznos;
        this.datumPorudzbine = datumPorudzbine;
        this.kupacId = kupacId;
    }
}


export class PorudzbinaUpdate {
    porudzbinaId: string;
    ukupanIznos: number;
    datumPorudzbine: string | null;
    

    constructor(
        porudzbinaId: string,
        ukupanIznos: number,
        datumPorudzbine: string | null,
       
    ) {
        this.porudzbinaId=porudzbinaId
        this.ukupanIznos = ukupanIznos;
        this.datumPorudzbine = datumPorudzbine;
       
    }
}
