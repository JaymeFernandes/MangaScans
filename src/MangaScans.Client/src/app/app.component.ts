import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { LeftSideMenuComponent } from './components/left-side-menu/left-side-menu.component';
import { BaseUiComponent } from './components/base-ui/base-ui.component';
import { AuthService } from './services/authentication/authentication.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavBarComponent, LeftSideMenuComponent, BaseUiComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'MangaScans.Client';
}
