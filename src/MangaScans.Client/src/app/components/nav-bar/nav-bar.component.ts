import { Component, signal, HostListener } from '@angular/core';
import { MangasIconComponent } from '../icons/mangas-icon/mangas-icon.component';
import { PageStatusService } from '../../services/page-status.service';

@Component({
  selector: 'nav-bar',
  standalone: true,
  imports: [ MangasIconComponent ],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss'
})
export class NavBarComponent {
  menuState = signal(false);
  isAtTop = signal(true);

  constructor(private status: PageStatusService) {
    this.status.menuState$.subscribe(state => {
      this.menuState.set(!!state);
    });
  }

  ToggleMenu(){
    this.status.ToggleMenu();
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;

    if (scrollTop === 0) {
      this.isAtTop.set(true);
    } else {
      this.isAtTop.set(false);
    }
  }
}
