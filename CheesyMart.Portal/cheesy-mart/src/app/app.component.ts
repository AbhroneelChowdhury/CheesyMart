import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BannerHeaderComponent } from './components/banner-header/banner-header.component';
import { CHEESEYMART_API_BASE_URL } from './services/cheesy-client.service';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,BannerHeaderComponent ],
  providers:[{provide: CHEESEYMART_API_BASE_URL, useValue: environment.cheesymartApiEndpoint}],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'cheesy-mart';
}
