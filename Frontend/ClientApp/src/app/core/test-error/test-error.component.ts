import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent {
baseUrl = environment.apiUrl
validationErrors: string[] = []

constructor(private http: HttpClient){

}

get404Error(){
  this.http.get(this.baseUrl + 'Dres/39e755f7-7625-4213-bf9e-70938d2f7d12').subscribe({
    next: response => console.log(response),
    error: error => console.log(error)
    
  })
}

get500Error(){
  this.http.get(this.baseUrl + 'Dres/500').subscribe({
    next: response => console.log(response),
    error: error => console.log(error)
    
  })
}

get400Error(){
  this.http.get(this.baseUrl + 'Dres/400').subscribe({
    next: response => console.log(response),
    error: error => console.log(error)
    
  })
}

get400ValidationError(){
  this.http.get(this.baseUrl + 'Dres/safs').subscribe({
    next: response => console.log(response),
    error: error => {
      console.log(error)
      this.validationErrors=error.errors
    }
  })
}
}
