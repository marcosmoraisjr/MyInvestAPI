import { HttpResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActiveService } from '../../services/active.service';
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

@Component({
  selector: 'app-view-ticker-final',
  standalone: true,
  imports: [LoadingComponent],
  templateUrl: './view-ticker-final.component.html',
  styleUrl: './view-ticker-final.component.scss'
})
export class ViewTickerFinalComponent implements OnInit{
  activeName: string = '';
  isLoading: boolean = true;

  constructor(
    private activedRoute: ActivatedRoute,
    private activeService: ActiveService
  ) {}

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

  ngOnInit(): void {
    var param = this.activedRoute.snapshot.paramMap.get('name');

    param !== null ? this.activeName = param : alert("Aconteceu um erro ao tentar buscar o ticker!");

    this.activeService.search(this.activeName).subscribe({
      next: (response: HttpResponse<any>) => {
        this.isLoading = false;
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
        alert("Aconteceu um erro ao tentar buscar o ticker!");
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
      roe: body.roe || '',
      crecimento_De_Dividendos_5_anos: body.crecimentoDeDividendos || ''
    } 
  }
}
