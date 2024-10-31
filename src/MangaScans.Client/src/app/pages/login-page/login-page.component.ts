import { Component, inject, signal, Signal } from "@angular/core";
import { AuthService } from "../../services/authentication/authentication.service";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";

@Component({
  selector: "app-login-page",
  standalone: true,
  imports: [ ReactiveFormsModule ],
  templateUrl: "./login-page.component.html",
  styleUrl: "./login-page.component.scss",
})
export class LoginPageComponent {

  formLogin!: FormGroup;
  fb = inject(FormBuilder);
  errors = signal<string[]>([]);

  constructor(private loginServices: AuthService, private routerService: Router) {
    this.formLogin = this.fb.group({
      email: ['', [Validators.email, Validators.required]],
      password: ['', [Validators.minLength(6), Validators.required]],
    });
  }

  onSubmitLogin() {
    if (this.formLogin.valid) {
      this.Login(this.formLogin.value.email, this.formLogin.value.password);
    }
  }

  Login(email: string, password: string) {
    this.loginServices.Login(email, password).subscribe((response) => {
      if (response?.isAuthenticated) {
        this.routerService.navigate(['/']);
      }
      else {
        this.errors.set(response?.Erros || ["Erro de autenticação."]);
      }
    });
  }
}
