import { Component, signal } from '@angular/core';
import { MenuIconComponent } from '../icons/menu-icon/menu-icon.component';
import { SearchIconComponent } from '../icons/search-icon/search-icon.component';
import { PageStatusService } from '../../services/page-status.service';

@Component({
  selector: 'nav-bar',
  standalone: true,
  imports: [MenuIconComponent, SearchIconComponent],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss'
})
export class NavBarComponent {
  menuState = signal(false);

  constructor(private status: PageStatusService) {
    this.status.menuState$.subscribe(state => {
      this.menuState.set(!!state);
    });
  }

  ToggleMenu(){
    this.status.ToggleMenu();
  }
}
