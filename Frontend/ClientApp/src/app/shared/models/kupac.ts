export class Kupac {
    kupacId: string;
    kupacKorisnickoIme: string;
    kupacIme: string;
    kupacPrezime: string;
    kupacLozinka: string;
    kupacEmail: string;
    kupacBrojTelefona: string;
    kupacAdresa: string;

    constructor(
        kupacId: string,
        kupacKorisnickoIme: string,
        kupacIme: string,
        kupacPrezime: string,
        kupacLozinka: string,
        kupacEmail: string,
        kupacBrojTelefona: string,
        kupacAdresa: string
    ) {
        this.kupacId = kupacId;
        this.kupacKorisnickoIme = kupacKorisnickoIme;
        this.kupacIme = kupacIme;
        this.kupacPrezime = kupacPrezime;
        this.kupacLozinka = kupacLozinka;
        this.kupacEmail = kupacEmail;
        this.kupacBrojTelefona = kupacBrojTelefona;
        this.kupacAdresa = kupacAdresa;
    }
}

export class KupacCreation {
    kupacKorisnickoIme: string;
    kupacIme: string;
    kupacPrezime: string;
    kupacLozinka: string;
    kupacEmail: string;
    kupacBrojTelefona: string;
    kupacAdresa: string;

    constructor(

        kupacKorisnickoIme: string,
        kupacIme: string,
        kupacPrezime: string,
        kupacLozinka: string,
        kupacEmail: string,
        kupacBrojTelefona: string,
        kupacAdresa: string
    ) {

        this.kupacKorisnickoIme = kupacKorisnickoIme;
        this.kupacIme = kupacIme;
        this.kupacPrezime = kupacPrezime;
        this.kupacLozinka = kupacLozinka;
        this.kupacEmail = kupacEmail;
        this.kupacBrojTelefona = kupacBrojTelefona;
        this.kupacAdresa = kupacAdresa;
    }
}

export class KupacNoPassword {
    kupacKorisnickoIme: string;
    kupacIme: string;
    kupacPrezime: string;

    kupacEmail: string;
    kupacBrojTelefona: string;
    kupacAdresa: string;

    constructor(

        kupacKorisnickoIme: string,
        kupacIme: string,
        kupacPrezime: string,
        
        kupacEmail: string,
        kupacBrojTelefona: string,
        kupacAdresa: string
    ) {

        this.kupacKorisnickoIme = kupacKorisnickoIme;
        this.kupacIme = kupacIme;
        this.kupacPrezime = kupacPrezime;
      
        this.kupacEmail = kupacEmail;
        this.kupacBrojTelefona = kupacBrojTelefona;
        this.kupacAdresa = kupacAdresa;
    }
}

export class KupacUpdate {
    kupacId: string;
    kupacIme: string;
    kupacPrezime: string;
    kupacBrojTelefona: string;
    kupacAdresa: string;

    constructor(
        kupacId: string,
        kupacIme: string,
        kupacPrezime: string,
        kupacBrojTelefona: string,
        kupacAdresa: string
    ) {
        this.kupacId=kupacId;
        this.kupacIme = kupacIme;
        this.kupacPrezime = kupacPrezime;
        this.kupacBrojTelefona = kupacBrojTelefona;
        this.kupacAdresa = kupacAdresa;
    }
}
