import { Component, signal } from '@angular/core';
import { PageStatusService } from '../../services/page-status.service';
import { HomeIconComponent } from '../icons/home-icon/home-icon.component';
import { BookIconComponent } from '../icons/book-icon/book-icon.component';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'left-side-menu',
  standalone: true,
  imports: [HomeIconComponent, BookIconComponent, RouterModule],
  templateUrl: './left-side-menu.component.html',
  styleUrl: './left-side-menu.component.scss'
})
export class LeftSideMenuComponent {
  menuState = signal(false);

  constructor(private status: PageStatusService) { 
    this.status.menuState$.subscribe(state => {
      this.menuState.set(!!state);
    });
  }
}
