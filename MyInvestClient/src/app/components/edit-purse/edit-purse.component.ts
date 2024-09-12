import { Component, OnInit } from '@angular/core';
import { LoadingComponent } from '../layout/loading/loading.component';
import { AuthService } from '../../services/auth.service';
import { PurseService } from '../../services/purse.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-edit-purse',
  standalone: true,
  imports: [
    LoadingComponent, ReactiveFormsModule, CommonModule
  ],
  templateUrl: './edit-purse.component.html',
  styleUrl: './edit-purse.component.scss'
})
export class EditPurseComponent implements OnInit{
  isLoading: boolean = true;
  form: FormGroup;
  userId: string = '';
  purseId: string = '';

  constructor(
    private fb: FormBuilder,
    private purseService: PurseService,
    private authService: AuthService,
    private route: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      user_Id: ['']
    })
  }

  onSubmit():void 
  {
    if (this.form.valid)
    {
      this.isLoading = true;
      this.purseService.update(this.form.value).subscribe({
        next: (response: HttpResponse<any>) => {
          if (response.status === 204)
          {
            this.isLoading = false;
            alert("Carteira atualizada com sucesso!");
            this.route.navigate(["/purses"]);
          }
          else 
          {
            this.isLoading = false;
            console.log("Foi retornada uma resposta inesperada pelo servidor!");
          }
        },
        error: (err) => {
          this.isLoading = false;
          alert("Aconteceu um erro ao atualizar a carteira! \n" + err.message);
        }
      })
    }
    else 
    {
      this.form.markAllAsTouched();
    }
  }

  ngOnInit(): void {
    var param = this.activatedRoute.snapshot.paramMap.get('id');
    if (param !== null)
    {
      this.purseId = param;
    }

    this.userId = this.authService.getId();

    this.purseService.getById(this.purseId).subscribe({
      next: (response: HttpResponse<any>) => {
        this.populateForm(response.body);
      },
      error: (err) => {
        if (err.status === 404)
        {
          alert("Carteira não encontrada!");
        }
        if (err.status === 500)
        {
          alert("Houve um erro ao buscar as carteiras!");
        }
        this.isLoading = false;
      }
    });
  }

  populateForm(body: any) {
    this.form.patchValue({
      name: body.name,
      description: body.description,
      user_Id: body.purse_Id
    })
    this.isLoading = false;
  }
}
