import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActiveService } from '../../services/active.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { LoadingComponent } from '../layout/loading/loading.component';
import { AuthService } from '../../services/auth.service';

interface Active {
  id: string,
  code: string,
  type: string,
  dyDesiredPercentage: string
}

@Component({
  selector: 'app-view-actives',
  standalone: true,
  imports: [CommonModule, LoadingComponent],
  templateUrl: './view-actives.component.html',
  styleUrl: './view-actives.component.scss'
})
export class ViewActivesComponent implements OnInit{
  purseId: string = '';
  userId: string = '';
  actives: Active[] = [];
  @ViewChild('message', { static: false }) message!: ElementRef;
  @ViewChild('titles', { static: false }) titles!: ElementRef;
  isLoading: boolean = true;

  constructor(
    private activeService: ActiveService,
    private activedRoute: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getId();

    var param = this.activedRoute.snapshot.paramMap.get('purse');
    if (param != null)
    {
      this.purseId = param; 
    } 
    else 
    {
      alert("Ocorreu um erro ao tentar buscar os ativos!");
      return;
    }

    this.activeService.searchActivesByPurseId(param).subscribe({
      next: (response: HttpResponse<any>) => {
        this.isLoading = false;
        if (response.status === 200)
        {
          this.populateTheArrayOfActives(response.body);
        }
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 404)
        {
          this.message.nativeElement.classList.add('active');
          return;
        }
        if (err.status === 500)
        {
          if (typeof window !== 'undefined')
          {
            alert("Carteira criada com sucesso!");
          }
        }
        console.log(err);
      }
    })
  }

  populateTheArrayOfActives(body: any): void
  {
    if (body.actives.length > 0)
    {
      this.actives = body.actives.map((active: any) => {
        return {
          id: active.active_Id,
          code: active.code,
          type: active.type,
          dyDesiredPercentage: active.dyDesiredPercentage
        }
      });
      this.titles.nativeElement.classList.add('active');
      this.isLoading = false;
    }
    else
    {
      this.message.nativeElement.classList.add('active');
      this.isLoading = false;
    }
  }

  redirectToActive(code: string, dyDesiredPercentage: string): void
  {
    this.router.navigate([`/view-active-info/${code}/${dyDesiredPercentage}`]);
  }

  createActive(): void 
  {
    this.router.navigate(["/"]);
  }

  deleteActive(purseId: any): void 
  {
    this.activeService.delete(purseId).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 204)
        {
          this.actives.filter(active => active.id !== purseId);
        }

        if (this.actives.length == 0) {
          this.titles.nativeElement.classList.remove('active');
          this.message.nativeElement.classList.add('active');
        }
      },
      error: (err) => {
        if (typeof window !== 'undefined')
        {
          alert("Ocorreu um erro ao tentar deletar o ativo!");
          console.log(`Ocorreu um erro ao tentar deletar o ativo! err: ${err.message}`);
          return;
        }
      }
    })
  }

  updateActive(activeId: string, activeCode: string, percentValue: string)
  {
    this.router.navigate(["/edit-ticker/" + activeId + "/" +  activeCode + "/" + percentValue])
  }
}
