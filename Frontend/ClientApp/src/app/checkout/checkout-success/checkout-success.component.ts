import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.scss']
})
export class CheckoutSuccessComponent implements OnInit{
id = ''

constructor(private route:ActivatedRoute, private router: Router){}
  ngOnInit(): void {
    this.route.queryParams.subscribe(params =>{
      this.id=params['yourParam']
    })
  }

  openOrder(){
    this.router.navigateByUrl('/orders/'+ this.id)
  }
}
