import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CheckoutGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const queryParams = route.queryParams;
    if (queryParams && queryParams['from'] === 'toCheckout') {
      return true; 
    } else {
      this.router.navigateByUrl('/not-found');
      return false;
    }
  }
}
