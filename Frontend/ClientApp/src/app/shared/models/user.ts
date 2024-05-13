export class User{
    token: string;
    expiresOn: string;
    korisnickoIme: string;
    role: string;
    userId: string

    constructor(token: string, expiresOn: string, korisnickoIme: string, role: string, userId:string){
        this.token=token;
        this.expiresOn =expiresOn;
        this.korisnickoIme = korisnickoIme;
        this.role = role;
        this.userId = userId
    
    }
}