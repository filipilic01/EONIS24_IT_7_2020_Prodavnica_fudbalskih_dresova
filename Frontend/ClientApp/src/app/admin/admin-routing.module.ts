import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { ProductsComponent } from './products/products.component';
import { AdminOrdersComponent } from './admin-orders/admin-orders.component';
import { AdminOrderDetailsComponent } from './admin-order-details/admin-order-details.component';

const routes: Routes= [
  {path:'users', component:UsersComponent, data: {breadcrumb: 'Korisnici'}},
  {path: 'products', component: ProductsComponent, data: {breadcrumb: 'Dresovi'}},
  {path: 'admin-orders', component: AdminOrdersComponent, data: {breadcrumb: 'Porudžbine'}},
  {path:':id',component:AdminOrderDetailsComponent,data:{breadcrumb:{alias:'orderDetails'}}},
]


@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[RouterModule]
})
export class AdminRoutingModule { }
