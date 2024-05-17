import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Porudzbina, PorudzbinaCreation, PorudzbinaUpdate } from '../shared/models/porudzbina';
import { tap, map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { StavkaPorudzbine, StavkaPorudzbineCreation, StavkaPorudzbineUpdate } from '../shared/models/stavka-porudzbine';
import { Kupac, KupacNoPassword } from '../shared/models/kupac';

@Injectable({
  providedIn: 'root'
})
export class CartService {
baseUrl = environment.apiUrl

  brojStavki:number =0

  constructor(private http: HttpClient) { }

  createPaymentIntent(){
    const token = localStorage.getItem('token');
    let headers= new HttpHeaders();
      headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.post(this.baseUrl+'Payment/' + localStorage.getItem('porudzbina'), {}, {headers});
  }

  createStavkaPorudzbine(stavkaPorudzbina: StavkaPorudzbineCreation): Observable<any> {
    const token = localStorage.getItem('token');
    let headers= new HttpHeaders();
      headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.post(`${this.baseUrl}StavkaPorudzbine`, stavkaPorudzbina ,{headers}).pipe(
      tap(() => {
        this.brojStavki++;
      })
    );
  }

createPorudzbina(porudzbina: PorudzbinaCreation ): Observable<any>{
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.post(this.baseUrl+ 'Porudzbina',porudzbina, {headers})

}
updateStavkaPorudzbine(stavkaPorudzbina: StavkaPorudzbineUpdate){
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.put(this.baseUrl+ 'StavkaPorudzbine', stavkaPorudzbina, {headers});
}
deleteStavkaPorudzbine(id: string) {
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.delete(this.baseUrl + 'StavkaPorudzbine/' + id, {headers}).pipe(
    tap(() => {
      this.brojStavki--; 
    })
  );
}
getPorudzbine(){
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.get<Porudzbina[]>(this.baseUrl + 'Porudzbina', {headers});
}

getPorudzbina(){
  const id = localStorage.getItem('porudzbina');
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.get<Porudzbina>(this.baseUrl + 'Porudzbina/'+id, {headers});
}

getPorudzbinaId(id: string){
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.get<Porudzbina>(this.baseUrl + 'Porudzbina/'+id, {headers});
}

getPorudzbineByKupacId(){
  const id = localStorage.getItem('userId');
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.get<Porudzbina[]>(this.baseUrl + 'Porudzbina/Kupac/'+id, {headers});
}

getBrojPorudzbinaByKupac(user: KupacNoPassword){
  
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
  headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.get<number>(this.baseUrl + 'Porudzbina/Broj/'+user.kupacKorisnickoIme, {headers});
}

deletePorudzbina(){
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.delete(this.baseUrl + 'Porudzbina/' +  localStorage.getItem('porudzbina'), {headers}).pipe(
    tap(() => {
      this.brojStavki=0
    }))
  
}

getStavkeByPorudzbinaId() {
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.get<StavkaPorudzbine[]>(this.baseUrl + 'StavkaPorudzbine/Porudzbina/' + localStorage.getItem('porudzbina'), {headers}).pipe(
    map((stavke: StavkaPorudzbine[]) => {
    this.brojStavki=0
      stavke.forEach(stavka => {
        this.brojStavki=this.brojStavki+stavka.brojStavki
      });
      return stavke; 
    })
  );
}
getStavkePorudzbineByPorudzbinaId(id: string) {
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.get<StavkaPorudzbine[]>(this.baseUrl + 'StavkaPorudzbine/Porudzbina/' + id, {headers});
}

updatePorudzbina(porudzbina: PorudzbinaUpdate){
  const token = localStorage.getItem('token');
  let headers= new HttpHeaders();
  headers = headers.set('Authorization', `Bearer ${token}`);
  return this.http.put(this.baseUrl+ 'Porudzbina', porudzbina, {headers})
}
}
