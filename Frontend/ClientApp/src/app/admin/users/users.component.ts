import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AccountService } from 'src/app/account/account.service';
import { CartService } from 'src/app/cart/cart.service';
import { KupacNoPassword } from 'src/app/shared/models/kupac';
import { UsersDialogComponent } from '../dialogs/users-dialog/users-dialog.component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit{
 users: KupacNoPassword[] =[]
 bsModalRef: BsModalRef | undefined;
 aktivni: number[] =[]
  constructor(private accountService: AccountService, private cartService: CartService, private modalService: BsModalService){}

  ngOnInit(): void {
    this.accountService.getKupci().subscribe(res=>{
      this.users=res;
      for(let i =0 ;i<this.users.length;i++){
        this.cartService.getBrojPorudzbinaByKupac(this.users[i]).subscribe(res=>{
          this.aktivni[i] = res;
        })
      }
      
      
    })
  }

  deleteKupac(user: KupacNoPassword){
    const initialState = {
      kupacId: user.kupacId,
      kupacIme: user.kupacIme,
      kupacAdresa: user.kupacAdresa,
      kupacPrezime: user.kupacPrezime,
      kupacEmail: user.kupacEmail,
      kupacBrojTelefona: user.kupacBrojTelefona,
      kupacKorisnickoIme: user.kupacKorisnickoIme

    };
    this.bsModalRef = this.modalService.show(UsersDialogComponent, {initialState});
    this.bsModalRef.onHidden?.subscribe(() => {
      this.ngOnInit()
    })

  }
  

}
