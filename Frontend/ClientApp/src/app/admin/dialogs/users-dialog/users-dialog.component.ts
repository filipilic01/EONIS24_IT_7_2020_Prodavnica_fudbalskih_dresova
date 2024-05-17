import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-users-dialog',
  templateUrl: './users-dialog.component.html',
  styleUrls: ['./users-dialog.component.scss']
})
export class UsersDialogComponent implements OnInit{
  kupacId: string | undefined;
  kupacIme: string | undefined;
  kupacPrezime: string | undefined;
   kupacKorisnickoIme: string | undefined;
   kupacEmail: string | undefined;
   kupacBrojTelefona: string | undefined;
   kupacAdresa: string | undefined

   constructor(public bsModalRef: BsModalRef, public accountService: AccountService, private toastr: ToastrService) { }


   ngOnInit(): void {
     
   }

   closeModal() {
    this.bsModalRef.hide();
  }

  deleteKupac(){
    if(this.kupacId)
this.accountService.deleteKupac(this.kupacId).subscribe({
  next: () => {
          
    this.toastr.success("Uspešno obrisan kupac");
    this.bsModalRef.hide();
  },
  error: error => this.toastr.error(error.message)

})
  }

}
