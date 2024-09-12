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

  percentValue: number | null = null;
  dYDisplayValue: string = '';

  constructor(
    private fb: FormBuilder,
    private activeService: ActiveService,
    private route: Router
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      dy: ['', Validators.required]
    })
  }

  onSubmit()
  {
    if (this.form.valid) 
    {
      this.isLoading = true;
      
      if (this.percentValue === null)
      {
        if (typeof window !== 'undefined')
        {
          alert("O DY (Dividend Yield) não pode ser nulo !");
        }
        this.isLoading = false;
        return;
      }
    
      this.activeService.search(this.form.get('name')?.value, this.percentValue).subscribe({
        next: (response: HttpResponse<any>) => {
          this.isLoading = false;
          if (response.status === 200)
          {
            var name = this.form.get('name')?.value;
            this.route.navigate([`/view-ticker/${name}/${this.percentValue}`]);
          }
        },
        error: (err) => {
          this.isLoading = false;
          if (err.status === 404)
          {
            alert("Não foi encontrado nenhum ativo com o ticker informado.");
            return;
          }
          if (err.status === 500)
          {
            alert("Houve um erro ao tentar buscar esse ativo!");
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

  onInputChange(event: any): void
  {
    const inputValue = event.target.value.replace('%', '').trim();
    const numericValue = parseFloat(inputValue);

    if (!isNaN(numericValue))
    {
      this.percentValue = numericValue;
      this.dYDisplayValue = `${numericValue}%`;
    }
    else
    {
      this.percentValue = null;
      this.dYDisplayValue = '';
    }
  }
}
