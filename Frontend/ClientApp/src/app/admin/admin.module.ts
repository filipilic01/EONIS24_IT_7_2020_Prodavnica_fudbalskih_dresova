import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { UsersComponent } from './users/users.component';

import { AdminRoutingModule } from './admin-routing.module';
import { ProductDialogComponent } from './dialogs/product-dialog/product-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AdminOrdersComponent } from './admin-orders/admin-orders.component';
import { UsersDialogComponent } from './dialogs/users-dialog/users-dialog.component';




@NgModule({
  declarations: [
    ProductsComponent,
    UsersComponent,

    ProductDialogComponent,
     AdminOrdersComponent,
     UsersDialogComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    ModalModule.forRoot()
  ],
  exports: [
    FormsModule
  ]
})
export class AdminModule { }
