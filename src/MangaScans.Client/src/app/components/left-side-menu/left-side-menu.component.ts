import { Component, signal } from '@angular/core';
import { PageStatusService } from '../../services/page-status.service';
import { MangasIconComponent } from '../icons/mangas-icon/mangas-icon.component';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'left-side-menu',
  standalone: true,
  imports: [MangasIconComponent, RouterModule],
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
