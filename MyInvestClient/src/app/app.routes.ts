import { Routes } from '@angular/router';
import { CreateAccountComponent } from './components/create-account/create-account.component';
import { ViewPursesComponent } from './components/view-purses/view-purses.component';
import { CreatePurseComponent } from './components/create-purse/create-purse.component';
import { SearchTickerComponent } from './components/search-ticker/search-ticker.component';
import { ViewTickerComponent } from './components/view-ticker/view-ticker.component';
import { ViewActivesComponent } from './components/view-actives/view-actives.component';
import { ViewTickerFinalComponent } from './components/view-ticker-final/view-ticker-final.component';

export const routes: Routes = [
     {
          path: '', component: SearchTickerComponent
     },
     {
          path: 'create-account', component: CreateAccountComponent
     },
     {
          path: 'purses', component: ViewPursesComponent
     },
     {
          path: 'create-purse', component: CreatePurseComponent
     },
     {
          path: 'search-ticker', component: SearchTickerComponent
     },
     {
          path: 'view-ticker/:name', component: ViewTickerComponent
     },
     {
          path: 'view-actives/:purse', component: ViewActivesComponent
     },
     {
          path: 'view-active-info/:name', component: ViewTickerFinalComponent
     }
];
