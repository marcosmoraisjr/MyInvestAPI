import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActiveService } from '../../services/active.service';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpResponse } from '@angular/common/http';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LoadingComponent } from '../layout/loading/loading.component';

interface Active {
  data: string,
  ativo: string,
  nomeDoAtivo: string,
  tipo: string,
  dividentYield: string,
  precoAtual: string,
  p_VP: string,
  preco_Teto: string,
  indicacao: string,
  p_L: string,
  roe: string,
  crecimento_De_Dividendos_5_anos: string
}

interface Purse {
  id: string,
  name: string
}

@Component({
  selector: 'app-view-ticker',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingComponent],
  templateUrl: './view-ticker.component.html',
  styleUrl: './view-ticker.component.scss'
})
export class ViewTickerComponent implements OnInit{
  userId: string = '';
  activeName: string = '';
  purses: Purse[] = [];
  selectedPurseId: string = '';
  @ViewChild('containerError', { static : false }) containerError!: ElementRef;
  isLoading: boolean = true;

  percentValue: number | null = null;
  dYDisplayValue: string = '';

  hasUpdatedInputAutomatically: boolean = false;

  active: Active = {
    data: '',
    ativo: '',
    nomeDoAtivo: '',
    tipo: '',
    dividentYield: '',
    precoAtual: '',
    p_VP: '',
    preco_Teto: '',
    indicacao: '',
    p_L: '',
    roe: '',
    crecimento_De_Dividendos_5_anos: ''
  }

  constructor(
    private activeService: ActiveService,
    private authService: AuthService,
    private activedRoute: ActivatedRoute,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getId();
    var param = this.activedRoute.snapshot.paramMap.get('name');
    var percentValueParam = this.activedRoute.snapshot.paramMap.get('percentValue');

    param !== null ? this.activeName = param : alert("Aconteceu um erro ao tentar buscar o ticker!");
    percentValueParam !== null ? this.percentValue = parseInt(percentValueParam) : alert("Aconteceu um erro ao tentar buscar o ticker!");
    
    this.searchTicker();
    
    this.userService.getPurses(this.userId).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200)
        {
          response.body.purses.forEach((purse: any) => {
            const newPurse: Purse = {
              id: purse.purse_Id,
              name: purse.name
            };
            this.purses.push(newPurse);
          });
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.log(err);
      }
    })
  }

  searchTicker()
  {
    if (this.percentValue === null)
    {
      alert("O DY (Dividend Yield) não pode ser nulo111!");
      return;
    }

    this.activeService.search(this.activeName, this.percentValue).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200)
        {
          this.populateActiveFields(response.body);
        }
        else 
        {
          alert("Ocorreu um erro interno no sistema!");
        }
      },
      error: (err) => {
        this.isLoading = false;
        if (typeof window !== 'undefined')
        {
          alert("Aconteceu um erro ao tentar buscar o ticker!");
        }
      }
    })
  }

  populateActiveFields(body: any): void
  {
    this.active = {
      data: body.data || '',
      ativo: body.ativo || '',
      nomeDoAtivo: body.nomeDoAtivo || '',
      tipo: body.tipo || '',
      dividentYield: body.dividentYield || '',
      precoAtual: body.precoAtual || '',
      p_VP: body.p_VP || '',
      preco_Teto: body.preco_Teto || '',
      indicacao: body.indicacao || '',
      p_L: body.p_L || '',
      roe: body.roe || body.roe,
      crecimento_De_Dividendos_5_anos: body.crecimento_De_Dividendos_5_anos || ''
    } 
    if (!this.hasUpdatedInputAutomatically){
      this.dYDisplayValue = body.dividentYield;
      this.hasUpdatedInputAutomatically = false;
    }

    this.isLoading = false;
  }

  addActive(): void 
  {
    if (!this.authService.verifyIfUserIdLogged())
    {
      this.router.navigate(["/create-account"]);
      return;
    }

    if (this.selectedPurseId !== '')
    {
      this.isLoading = true;

      if (this.percentValue === null)
      {
        if (typeof window !== 'undefined')
          {
            alert("O DY (Dividend Yield) não pode ser nulo !");
          }
          return;
      }

      this.activeService.create(this.selectedPurseId, this.active.tipo, this.active.ativo, this.percentValue?.toString()).subscribe({
        next: (response: HttpResponse<any>) => {
          this.isLoading = false;
          if (response.status === 201)
          {
            this.router.navigate(["/purses"]);
          }
        },
        error: (err) => {
          this.isLoading = false;
          console.log(err);
        }
      })
    }
    else {
      this.containerError.nativeElement.classList.add('active');
    }
  }

  onInputChange(event: any): void
  {
    const inputValue = event.target.value.replace('%', '').trim();
    const numericValue = parseFloat(inputValue);

    if (numericValue !== this.percentValue && numericValue > 0)
    {
      this.percentValue = numericValue;
      this.dYDisplayValue = `${numericValue}%`;
    }
      
    if (numericValue !== 0 && numericValue > 0 && numericValue !== null && !isNaN(numericValue))
    {
      this.isLoading = true;
      this.searchTicker();
    }
  }
    
}

