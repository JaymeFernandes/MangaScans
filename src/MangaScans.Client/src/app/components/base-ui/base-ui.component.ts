import { Component } from '@angular/core';
import { AuthService } from '../../services/authentication/authentication.service';

@Component({
  selector: 'app-base-ui',
  standalone: true,
  imports: [],
  templateUrl: './base-ui.component.html',
  styleUrl: './base-ui.component.scss'
})
export class BaseUiComponent {
  constructor(private authServices: AuthService) {
    this.authServices.GetSession();
  }
}
