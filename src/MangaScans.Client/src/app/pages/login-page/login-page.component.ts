import { Component, inject, signal, Signal } from "@angular/core";
import { AuthService } from "../../services/authentication/authentication.service";
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { MangasIconComponent } from "../../components/icons/mangas-icon/mangas-icon.component";

@Component({
  selector: "app-login-page",
  standalone: true,
  imports: [ ReactiveFormsModule, MangasIconComponent, FormsModule ],
  templateUrl: "./login-page.component.html",
  styleUrls: ["./login-page.component.scss"],
})
export class LoginPageComponent {

  formLogin!: FormGroup;
  formRegister!: FormGroup;



  fbLogin = inject(FormBuilder);
  errors = signal<string[]>([]);

  IsLogin = signal<boolean>(true);

  constructor(private loginServices: AuthService, private routerService: Router) {
    this.formLogin = this.fbLogin.group({
      email: ['', [Validators.email, Validators.required]],
      password: ['', [Validators.minLength(6), Validators.required]],
    });

    this.formRegister = this.fbLogin.group({
      email: ['', [Validators.email, Validators.required]],
      password: ['', [Validators.minLength(6), Validators.required]],
      confirmPassword: ['', Validators.required],
      remember: []
    }, { validators: this.matchPasswordValidator() });
  }

  onSubmitLogin() {
    if (this.formLogin.valid) {
      this.Login(this.formLogin.value.email, this.formLogin.value.password);
    }
  }

  onSubmitRegister() {
    if (this.formRegister.valid) {
      this.Register(this.formRegister.value.email, this.formRegister.value.password, this.formRegister.value.confirmPassword, this.formRegister.value.remember);
    }
  }

  Login(email: string, password: string) {
    this.loginServices.Login(email, password).subscribe((response) => {
      if (response?.isAuthenticated) {
        this.routerService.navigate(['/']);
      }
      else {
        this.errors.set(response?.Erros || ["Usuário ou senha incorretos."]);
      }
    });
  }

  Register(email: string, password: string, confirmPassword: string, remember: boolean) {
    this.loginServices.Register(email, password, confirmPassword).subscribe((response) => {
      if (response?.isSuccess) {
        
        if (remember) {
          this.Login(email, password);
        }
        else {
          this.IsLogin.set(true);
        }
      }
      else {
        this.errors.set(response?.errors || ["não foi possível criar o usuário verifique as informações."]);
      }
    });
  }

  matchPasswordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.get('password')?.value;
      const confirmPassword = control.get('confirmPassword')?.value;
      
      return password === confirmPassword ? null : { passwordMismatch: true };
    };
  }
}
