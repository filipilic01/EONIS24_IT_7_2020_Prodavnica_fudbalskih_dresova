import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { OrderComponent } from './order/order.component';
import { OrderDetailsComponent } from './order-details/order-details.component';



const routes:Routes=[
  {path:'',component:OrderComponent},
  {path:':id',component:OrderDetailsComponent,data:{breadcrumb:{alias:'orderDetails'}}},
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[
    RouterModule
  ]
})
export class OrdersRoutingModule { }
