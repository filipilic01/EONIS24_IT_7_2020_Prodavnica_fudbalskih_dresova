export class AuthCreds{
    korisnickoIme: string
    lozinka: string

    constructor(korisnicko: string, lozinka:string){
        this.korisnickoIme=korisnicko
        this.lozinka=lozinka
    }
}