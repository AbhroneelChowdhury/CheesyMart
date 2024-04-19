import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { BannerComponent } from "./core/banner/banner.component";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss',
    imports: [RouterOutlet, BannerComponent, CommonModule]
})
export class AppComponent {


skipToContent() {
  const headingElement = document.querySelector<HTMLHeadElement>('h1');
  if(headingElement){
    headingElement.setAttribute('tabindex', '-1');
    headingElement.focus();
  } 
}
  title = 'Cheesy Mart';
  hidden = false;

}
