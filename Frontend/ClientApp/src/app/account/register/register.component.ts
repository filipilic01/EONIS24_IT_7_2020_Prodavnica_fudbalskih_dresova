import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { KupacCreation } from 'src/app/shared/models/kupac';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  passwordComplexity = "^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{6,}$"
  errors: string [] | null = null

  registerForm = new FormGroup({
    firstname: new FormControl('', Validators.required),
    lastname: new FormControl('', Validators.required),
    username: new FormControl('', Validators.required),
    password: new FormControl('', [Validators.required, Validators.pattern(this.passwordComplexity)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    

  })

  

  constructor(private accountService: AccountService, private router: Router, private toast: ToastrService){

  }

  
  onSubmit() {
    const username = this.registerForm.get('username')!.value as string;
    const password = this.registerForm.get('password')!.value as string;
    const email = this.registerForm.get('email')!.value as string;
    const firstname = this.registerForm.get('firstname')!.value as string;
    const lastname = this.registerForm.get('lastname')!.value as string;
  
  
    this.accountService.registerKupac(new KupacCreation(username,firstname,lastname,password,email,'','')).subscribe({
      next: () => {
        // Prikaz tost poruke kada je registracija uspešna
        this.toast.success('Uspešno ste se registrovali!');
        // Redirekcija na stranicu za prijavljivanje
        this.router.navigateByUrl('/login');
      },
      error: error => this.errors = error.errors
      
    });
  }

}
