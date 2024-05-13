import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { ProductsComponent } from './products/products.component';
import { OrdersComponent } from './orders/orders.component';

const routes: Routes= [
  {path:'users', component:UsersComponent, data: {breadcrumb: 'Korisnici'}},
  {path: 'products', component: ProductsComponent, data: {breadcrumb: 'Dresovi'}},
  {path: 'orders', component: OrdersComponent, data: {breadcrumb: 'Porudžbine'}}
]


@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[RouterModule]
})
export class AdminRoutingModule { }
