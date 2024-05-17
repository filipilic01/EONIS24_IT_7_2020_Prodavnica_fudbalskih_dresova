import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ShopComponent } from './shop/shop.component';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { CartComponent } from './cart/cart/cart.component';
import { AuthGuard } from './core/guards/auth.guard';
import { CheckoutGuard } from './core/guards/checkout.guard';
import { DresGuard } from './core/guards/dres.guard';


const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Početna'}},
  {path: 'test-error', component:TestErrorComponent},
  {path: 'server-error', component:ServerErrorComponent},
  {path: 'not-found', component:NotFoundComponent},
  {path: 'admin',canActivate:[AuthGuard], loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule),data: { breadcrumb: 'Admin' }},
  {path: 'shop', loadChildren: () => import('./shop/shop.module').then(m => m.ShopModule),data: {breadcrumb: 'Shop'}},
  {path: 'orders', loadChildren: () => import('./orders/orders.module').then(m => m.OrdersModule),data: {breadcrumb: 'Porudžbine'}},
  {path: 'checkout',canActivate:[AuthGuard, CheckoutGuard], loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule),data: {breadcrumb: 'Checkout'}},
  {path: 'cart', canActivate:[AuthGuard], loadChildren: () => import('./cart/cart.module').then(m => m.CartModule),data: {breadcrumb: 'Korpa'}},
  {path: 'account', loadChildren: () => import('./account/account.module').then(m => m.AccountModule),data: {breadcrumb: 'Nalog'}},
  {path: '**', redirectTo:'', pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
