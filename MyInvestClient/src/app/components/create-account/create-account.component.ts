import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../layout/navbar/navbar.component';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoadingComponent } from '../layout/loading/loading.component';

@Component({
  selector: 'app-create-account',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, LoadingComponent],
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.scss'
})
export class CreateAccountComponent implements OnInit{
  form: FormGroup;
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private route: Router
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.minLength(11)]],
      password: ['', [Validators.required, this.passwordStrengthValidator()]],
      verifyPassword: ['', Validators.required]
    }, {validators: this.passwordsMatchValidator() });
  }

  ngOnInit(): void {
    if (this.authService.verifyIfUserIdLogged()) {
      this.route.navigate(["/"])
    }
  }

  onSubmit(): void
  {
    if (this.form.valid)
    {
      this.isLoading = true;
      this.userService.create(this.form.value).subscribe({
        next: (response: HttpResponse<any>) => {
          if (response.status === 201)
          {
            this.authService.configureLocalStorage(response.body);
            this.isLoading = false;
            alert("Usuário criado com sucesso!");
            this.route.navigate(["/"]);
          }
        },
        error: (error) => {
          this.isLoading = false;
          alert("Ocorreu um erro ao tentar criar o usuário.");
        }
      })
    }
    else 
    {
      this.form.markAllAsTouched();
    }
  }

  passwordsMatchValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.get('password')?.value;
      const verifyPassword = control.get('verifyPassword')?.value;

      if (password && verifyPassword && password !== verifyPassword) {
        return { passwordsMismatch: true };
      }
      return null;
    };
  }

  passwordStrengthValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return null;
      }

      const isValidLength = value.length >= 8;

      if (!isValidLength) {
        return { passwordStrength: true };
      }
      return null;
    }
  }
}
