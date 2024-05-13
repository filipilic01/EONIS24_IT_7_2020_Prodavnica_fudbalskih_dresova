import { Injectable } from '@angular/core';
import { ShopModule } from './shop.module';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Pagination } from '../shared/models/pagination';
import { Dres, DresCreation, DresUpdate } from '../shared/models/dres';
import { VelicinaDresa, VelicinaDresaCreation, VelicinaDresaUpdate } from '../shared/models/velicina-dresa';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl= 'http://localhost:5132/api/'


  constructor(private http: HttpClient) { }

  getDresovi(season: string, type: string, sort: string, pageSize: number, pageIndex: number, search: string) {
    
    if(search){
      return this.http.get<Pagination<Dres[]>>(this.baseUrl + 'Dres?PageIndex=' + pageIndex +
    '&PageSize=' + pageSize + '&Sezona=' + season + '&Tip=' + type + '&Sort=' + sort + '&Search=' +search);  
    }
    return this.http.get<Pagination<Dres[]>>(this.baseUrl + 'Dres?PageIndex=' + pageIndex +
    '&PageSize=' + pageSize + '&Sezona=' + season + '&Tip=' + type + '&Sort=' + sort);
  }

  getDres(id: string){
    return this.http.get<Dres>(this.baseUrl + 'Dres/' + id);
  }

  addDres(dres: DresCreation){
    const token = localStorage.getItem('token');
    let headers= new HttpHeaders();
      headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.post<Dres>(this.baseUrl + 'Dres', dres, {headers});
  }

  deleteDres(id: string){
    const token = localStorage.getItem('token');
    let headers= new HttpHeaders();
      headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.delete(this.baseUrl +'Dres/' + id, {headers});
  }

  updateDres(dres: DresUpdate){
    const token = localStorage.getItem('token');
    let headers= new HttpHeaders();
      headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.put<Dres>(this.baseUrl+'Dres', dres, {headers});

  }

  addVelicinaDresa(velicina: VelicinaDresaCreation){
    const token = localStorage.getItem('token');
    let headers= new HttpHeaders();
      headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.post<VelicinaDresa>(this.baseUrl+ 'VelicinaDresa', velicina, {headers})
  }

  updateVelicinaDresa(velicina: VelicinaDresaUpdate){
    const token = localStorage.getItem('token');
    let headers= new HttpHeaders();
      headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.put<VelicinaDresa>(this.baseUrl+ 'VelicinaDresa', velicina, {headers})
  }

  getVelicinaDresa(id: string){
    return this.http.get<VelicinaDresa>(this.baseUrl+ 'VelicinaDresa/'+id)
  }

  getVelicineByDresId(id: string){
    return this.http.get<VelicinaDresa[]>(this.baseUrl+ 'VelicinaDresa/Dres/' + id);
  }
  
}
