import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NavbarComponent } from '../layout/navbar/navbar.component';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpResponse } from '@angular/common/http';
import { LoadingComponent } from '../layout/loading/loading.component';

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
  isLoading: boolean = true;

  constructor(
    private authService: AuthService,
    private userService: UserService,
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
        this.isLoading = false;
        alert("Houve um erro ao tentar buscar as carteiras");
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
      });
      this.isLoading = false;
    }
    else {
      this.message.nativeElement.classList.add('active');
      this.isLoading = false;
    }
  }

  redirectToPurse(purseId: number)
  {
    this.route.navigate(["/view-actives/" + purseId]);
  }

  createPurse(): void 
  {
    this.route.navigate(["/create-purse"]);
  }
}
