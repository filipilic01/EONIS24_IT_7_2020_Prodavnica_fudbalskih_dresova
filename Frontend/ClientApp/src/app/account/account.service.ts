import { Injectable } from '@angular/core';
import { BehaviorSubject, ReplaySubject, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { KupacCreation, KupacNoPassword } from '../shared/models/kupac';
import { AuthCreds } from '../shared/models/auth-creds';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http:HttpClient, private router: Router) { }

  login(values: AuthCreds){
    return this.http.post<User>(this.baseUrl + 'Auth', values).pipe(
      map(user => {
        localStorage.setItem('token', user.token);
        localStorage.setItem('expiresOn', user.expiresOn);
        localStorage.setItem('korisnickoIme', user.korisnickoIme);
        localStorage.setItem('role', user.role);
        localStorage.setItem('userId', user.userId);
        this.currentUserSource.next(user);
        return user;
      })
    )
  }

  loadCurrentUser(token: string | null){
    
    if(token === null){
      this.currentUserSource.next(null);
      return of(null);
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get<User>(this.baseUrl+ 'Auth/currentUser', {headers}).pipe(
      map(user => {
        if(user){
          localStorage.setItem('token', user.token)
        localStorage.setItem('expiresOn', user.expiresOn);
        localStorage.setItem('korisnickoIme', user.korisnickoIme);
        localStorage.setItem('role', user.role);
        localStorage.setItem('userId', user.userId);
        this.currentUserSource.next(user);
        return user;
        }
        else{
          return null
        }
        
      })
    )

  }

  registerKupac(kupac:KupacCreation){
    return this.http.post(this.baseUrl+ 'Kupac',kupac);
  }

  getKupci(){
    const token = localStorage.getItem('token');
    let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get<KupacNoPassword[]>(this.baseUrl+ 'Kupac',{headers} )
  }

  logout(){
    localStorage.removeItem('token');
    localStorage.removeItem('expiresOn');
    localStorage.removeItem('korisnickoIme');
    localStorage.removeItem('role');
    localStorage.removeItem('userId');
    localStorage.removeItem('porudzbina');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }
}
