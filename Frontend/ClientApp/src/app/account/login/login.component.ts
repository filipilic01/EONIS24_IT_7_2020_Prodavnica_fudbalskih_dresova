import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { AuthCreds } from 'src/app/shared/models/auth-creds';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  })
  returnUrl: string

  constructor(private accountService: AccountService, private router: Router, private activatedRoute: ActivatedRoute){
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/cart'
  }

  onSubmit() {
    const username = this.loginForm.get('username')!.value as string;
    const password = this.loginForm.get('password')!.value as string;
  
    this.accountService.login(new AuthCreds(username, password)).subscribe({
      next: () => this.router.navigateByUrl('/shop')
    });
  }
}
