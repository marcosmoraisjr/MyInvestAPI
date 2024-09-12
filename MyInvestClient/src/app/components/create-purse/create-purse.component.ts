import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { PurseService } from '../../services/purse.service';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoadingComponent } from '../layout/loading/loading.component';

@Component({
  selector: 'app-create-purse',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, LoadingComponent],
  templateUrl: './create-purse.component.html',
  styleUrl: './create-purse.component.scss'
})
export class CreatePurseComponent implements OnInit{
  form: FormGroup;
  userId: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private purseService: PurseService,
    private authService: AuthService,
    private route: Router
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      user_Id: ['']
    })
  }

  ngOnInit(): void {
    this.userId = this.authService.getId();
    this.form.patchValue({
      user_Id : this.userId
    });
  }

  onSubmit(): void
  { 
    if (this.form.valid)
    {
      this.isLoading = true;
      this.purseService.create(this.form.value).subscribe({
        next: (response: HttpResponse<any>) => {
          if (response.status === 201)
          {
            this.isLoading = false;
            if (typeof window !== 'undefined')
            {
              alert("Carteira criada com sucesso!");
            }
            
            this.route.navigate(["/purses"]);
          }
          else 
          {
            this.isLoading = false;
            console.log("Houve uma resposta inesperada do servidor.");
            if (typeof window !== 'undefined')
              {
                alert("Houve um problema ao criar a carteira!");
              }
            }
          },
          error: (err) => {
          this.isLoading = false;
          console.log(err);
          if (typeof window !== 'undefined')
          {
            alert("Houve um erro ao tentar criar uma carteira!");
          }
        }
      })
    }
    else {
      this.form.markAllAsTouched();
    }
  }
}
