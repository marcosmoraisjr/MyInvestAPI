import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NavbarComponent } from '../layout/navbar/navbar.component';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpResponse } from '@angular/common/http';
import { LoadingComponent } from '../layout/loading/loading.component';
import { PurseService } from '../../services/purse.service';

interface Purse {
  purse_Id: number;
  name: string;
  description: string;
  createdAt: string;
}

@Component({
  selector: 'app-view-purses',
  standalone: true,
  imports: [
    CommonModule, LoadingComponent
  ],
  templateUrl: './view-purses.component.html',
  styleUrl: './view-purses.component.scss'
})
export class ViewPursesComponent implements OnInit{
  userId: string = '';
  purses: Purse[] = [];
  @ViewChild('message', { static: false }) message!: ElementRef;
  @ViewChild('titles', { static: false }) titles!: ElementRef;
  isLoading: boolean = true;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private purseService: PurseService,
    private route: Router
  ) {}

  ngOnInit(): void {
    if (!this.authService.verifyIfUserIdLogged())
    {
      this.route.navigate(["/create-account"])
      this.isLoading = false;
      return;
    }

    this.userId = this.authService.getId();

    this.userService.getPurses(this.userId).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200)
        {
          this.refactorDateAndPushToArray(response.body.purses)
        }
      },
      error: (err) => {
        console.log("Houve um erro ao tentar buscar as carteiras " + err.message);
        this.isLoading = false;
        return;
      }
    })
  }

  refactorDateAndPushToArray(purses: any): void 
  {
    if (purses.length > 0) 
    {
      purses.forEach((purse: Purse) => {
        const date = new Date(purse.createdAt);
        purse.createdAt = date.toLocaleDateString('pt-BR');
        this.purses.push(purse);
        this.titles.nativeElement.classList.add('active');
      });
      this.isLoading = false;
    }
    else {
      this.message.nativeElement.classList.add('active');
      this.isLoading = false;
    }
  }
  
  createPurse(): void 
  {
    this.route.navigate(["/create-purse"]);
  }
  
  redirectToViewActives(purseId: number): void 
  {
    this.route.navigate(["/view-actives/" + purseId]);
  }
  
  redirectToUpdatePurse(purseId: number): void
  {
    this.route.navigate(["/edit-purse/" + purseId]);
  }

  deletePurse(id: number): void 
  {
    this.purseService.delete(id).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 204)
        {
          this.purses = this.purses.filter(purse => purse.purse_Id !== id);
          
          if (this.purses.length == 0) {
            this.titles.nativeElement.classList.remove('active');
            this.message.nativeElement.classList.add('active');
          }
        }
        else 
        {
          console.log("Uma responsta inesperada foi retornada pelo servidor!");        
        }
      },
      error: (error) => {
        if (error.status === 404)
        {
          alert("Carteira não encontrada!")
        }
        if (error.status === 500)
        {
          alert("Ocorreu um erro ao tentar deletar a carteira!");
        }
      }
    })
  }
}
