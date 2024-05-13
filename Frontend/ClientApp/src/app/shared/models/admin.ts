export class Admin {
    adminId: string;
    adminKorisnickoIme: string;
    adminIme: string;
    adminPrezime: string;
    adminLozinka: string;
    adminEmail: string;
    adminBrojTelefona: string;
    adminAdresa: string;

    constructor(
        adminId: string,
        adminKorisnickoIme: string,
        adminIme: string,
        adminPrezime: string,
        adminLozinka: string,
        adminEmail: string,
        adminBrojTelefona: string,
        adminAdresa: string
    
    ) {
        this.adminId = adminId;
        this.adminKorisnickoIme = adminKorisnickoIme;
        this.adminIme = adminIme;
        this.adminPrezime = adminPrezime;
        this.adminLozinka = adminLozinka;
        this.adminEmail = adminEmail;
        this.adminBrojTelefona = adminBrojTelefona;
        this.adminAdresa = adminAdresa;
    }
}

export class AdminCreation {
    adminKorisnickoIme: string;
    adminIme: string;
    adminPrezime: string;
    adminLozinka: string;
    adminEmail: string;
    adminBrojTelefona: string;
    adminAdresa: string;

    constructor(
        adminKorisnickoIme: string,
        adminIme: string,
        adminPrezime: string,
        adminLozinka: string,
        adminEmail: string,
        adminBrojTelefona: string,
        adminAdresa: string
    
    ) {
        this.adminKorisnickoIme = adminKorisnickoIme;
        this.adminIme = adminIme;
        this.adminPrezime = adminPrezime;
        this.adminLozinka = adminLozinka;
        this.adminEmail = adminEmail;
        this.adminBrojTelefona = adminBrojTelefona;
        this.adminAdresa = adminAdresa;
    }
}
