import { Component, Input } from '@angular/core';
import { Manga } from '../../Interfaces/MangaResponse';
import { environment } from '../../../environments/environment';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [ RouterModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.scss'
})
export class CardComponent {
  Url = environment.apiUrl;

  @Input( { required: true }) infoManga!: Manga;
}
