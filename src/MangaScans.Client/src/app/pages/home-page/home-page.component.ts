import { Component, signal } from '@angular/core';
import { HomeService } from '../../services/home/home.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
  
export class HomePageComponent {

  Url = environment.apiUrl;
  page = signal(1);
  categories = signal([]);
  mangas = signal<any[]>([]);;

  constructor(private homeService: HomeService) { }

  ngOnInit(): void {
    this.homeService.getTopMangas(this.page(), this.categories())
      .subscribe({
        next: (response) => {
          console.log(response.data);
          this.mangas.set(response.data);
        },
        error: (error) => {
          this.mangas.set([]);
        }
      });
  }
}

