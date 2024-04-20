import { Component } from '@angular/core';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-banner-header',
  standalone: true,
  imports: [MatToolbarModule ,MatButtonModule, RouterModule  ],
  templateUrl: './banner-header.component.html',
  styleUrl: './banner-header.component.scss'
})
export class BannerHeaderComponent {

}
