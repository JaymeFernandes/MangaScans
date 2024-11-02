import { Component, signal, OnInit, effect } from '@angular/core';
import { PageStatusService } from '../../services/page-status.service';
import { MangasIconComponent } from '../icons/mangas-icon/mangas-icon.component';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/authentication/authentication.service';
import { single } from 'rxjs';


@Component({
  selector: 'left-side-menu',
  standalone: true,
  imports: [MangasIconComponent, RouterModule],
  templateUrl: './left-side-menu.component.html',
  styleUrl: './left-side-menu.component.scss'
})
export class LeftSideMenuComponent {
  menuState = signal(false);
  isAuthenticated = signal(false);
  


  constructor(private status: PageStatusService, private authService: AuthService, private routerService: Router) {
    this.authService.isAuthenticated.subscribe(status => {
      this.isAuthenticated.set(status);
    })

    this.status.menuState$.subscribe(state => {
      this.menuState.set(!!state);
    });
  }

  logout() {
    this.authService.logout();
    this.routerService.navigate(['/']);
  }
}
