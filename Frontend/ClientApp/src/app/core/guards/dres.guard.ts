import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ShopService } from 'src/app/shop/shop.service';


@Injectable({
  providedIn: 'root'
})
export class DresGuard implements CanActivate {

  constructor(private shopService: ShopService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    const id = route.paramMap.get('id'); // Preuzimanje 'id' parametra iz route
    console.log(id); // Dodajte ovo za debagovanje

    if (!id) {
      this.router.navigateByUrl('/not-found');
      return of(false);
    }

    return this.shopService.getDres(id).pipe(
      map(res => {
        if (!res.obrisan) {
          return true;
        } else {
          this.router.navigateByUrl('/not-found');
          return false;
        }
      }),
      catchError((error) => {
        console.error(error);
        this.router.navigateByUrl('/not-found');
        return of(false);
      })
    );
  }
}
