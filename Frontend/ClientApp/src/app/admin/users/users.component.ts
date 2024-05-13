import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';
import { KupacNoPassword } from 'src/app/shared/models/kupac';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit{
 users: KupacNoPassword[] =[]
  constructor(private accountService: AccountService){}
 
  ngOnInit(): void {
    this.accountService.getKupci().subscribe(res=>{
      this.users=res;
    })
  }

  

}
