import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActiveService } from '../../services/active.service';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoadingComponent } from '../layout/loading/loading.component';

@Component({
  selector: 'app-search-ticker',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, LoadingComponent],
  templateUrl: './search-ticker.component.html',
  styleUrl: './search-ticker.component.scss'
})
export class SearchTickerComponent {
  form: FormGroup;
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private activeService: ActiveService,
    private route: Router
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required]
    })
  }

  onSubmit()
  {
    if (this.form.valid) 
    {
      this.isLoading = true;
      this.activeService.search(this.form.get('name')?.value).subscribe({
        next: (response: HttpResponse<any>) => {
          this.isLoading = false;
          if (response.status === 200)
          {
            var name = this.form.get('name')?.value;
            this.route.navigate(["/view-ticker/" + name]);
          }
        },
        error: (err) => {
          this.isLoading = false;
          if (err.status === 404)
          {
            alert("Não foi encontrado nenhum ativo com o ticker informado.");
            return;
          }
          console.log(err);
        }
      })
    }
    else 
    {
      this.form.markAllAsTouched()
    }
  }
}
