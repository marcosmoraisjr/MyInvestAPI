import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActiveService } from '../../services/active.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { LoadingComponent } from '../layout/loading/loading.component';

interface Active {
  id: string,
  code: string,
  type: string,
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
  actives: Active[] = [];
  @ViewChild('message', {static: false}) message!:ElementRef;
  isLoading: boolean = true;

  constructor(
    private activeService: ActiveService,
    private activedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    var param = this.activedRoute.snapshot.paramMap.get('purse');
    if (param !== null)
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
        console.log(err);
      }
    })
  }

  populateTheArrayOfActives(body: any): void
  {
      this.actives = body.actives.map((active: any) => {
        return {
          id: active.active_Id,
          code: active.code,
          type: active.type,
        }
      });
      console.log(this.actives);
  }

  redirectToActive(code: string): void
  {
    this.router.navigate(["/view-active-info/" + code]);
  }

  createActive(): void 
  {
    this.router.navigate(["/"]);
  }
}
