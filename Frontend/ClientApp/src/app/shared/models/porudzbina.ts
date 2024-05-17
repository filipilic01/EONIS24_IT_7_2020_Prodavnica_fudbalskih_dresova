import { Kupac } from "./kupac";

export class Porudzbina {
    porudzbinaId: string;
    ukupanIznos: number;
    datumKreiranja: string | null;
    datumAzuriranja: string | null;
    kupac: Kupac;
    placena: boolean
    kupacId: string;
    paymentIntentId: string | null;
    clientSecret: string | null;

    constructor(
        porudzbinaId: string,
        ukupanIznos: number,
        datumKreiranja: string | null,
        datumAzuriranja: string | null,
        kupac: Kupac,
        placena: boolean,
        kupacId: string,
        paymentIntentId: string | null,
        clientSecret: string | null
    ) {
        this.porudzbinaId = porudzbinaId;
        this.ukupanIznos = ukupanIznos;
        this.datumAzuriranja = datumAzuriranja;
        this.datumKreiranja= datumKreiranja;
        this.kupac = kupac;
        this.placena=placena;
        this.kupacId = kupacId;
        this.paymentIntentId=paymentIntentId;
        this.clientSecret = clientSecret;
    }
}

export class PorudzbinaCreation {
    ukupanIznos: number;
    datumKreiranja: string | null;
    datumAzuriranja: string | null;
    placena: boolean;
    kupacId: string;

    constructor(
        ukupanIznos: number,
        datumKreiranja: string | null,
        datumAzuriranja: string | null,
        placena: boolean,
        kupacId: string
    ) {
        this.ukupanIznos = ukupanIznos;
        this.datumKreiranja = datumKreiranja;
        this.datumAzuriranja = datumAzuriranja;
        this.placena = placena;
        this.kupacId = kupacId;
    }
}


export class PorudzbinaUpdate {
    porudzbinaId: string;
    ukupanIznos: number;
    datumKreiranja: string | null;
    datumAzuriranja: string | null;
    placena: boolean;
    

    constructor(
        porudzbinaId: string,
        ukupanIznos: number,
        datumKreiranja: string | null,
        datumAzuriranja: string | null,
        placena: boolean
       
    ) {
        this.porudzbinaId=porudzbinaId
        this.ukupanIznos = ukupanIznos;
        this.datumAzuriranja = datumAzuriranja;
        this.datumKreiranja=datumKreiranja;
        this.placena = placena;
       
    }
}
