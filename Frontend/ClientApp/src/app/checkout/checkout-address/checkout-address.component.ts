import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { KupacUpdate } from 'src/app/shared/models/kupac';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent {
@Input() checkoutForm?: FormGroup;

constructor(private accountService: AccountService, private toastr: ToastrService){}

saveAddress(){
  const id = localStorage.getItem('userId');
  if(id != null){
      this.accountService.updateKupac(new KupacUpdate(id,
   this.checkoutForm?.get('addressForm')?.get('firstName')?.value,
   this.checkoutForm?.get('addressForm')?.get('lastName')?.value,
   this.checkoutForm?.get('addressForm')?.get('phoneNumber')?.value,
   this.checkoutForm?.get('addressForm')?.get('street')?.value+','+
   this.checkoutForm?.get('addressForm')?.get('streetNumber')?.value+','+
   this.checkoutForm?.get('addressForm')?.get('zipcode')?.value+','+
   this.checkoutForm?.get('addressForm')?.get('city')?.value)).subscribe({
    next: () => {
      this.toastr.success('Uspešno ste sačuvali adresu')
      this.checkoutForm?.get('addressForm')?.reset(this.checkoutForm?.get('addressForm')?.value)
    }
    })
}
  }

}
