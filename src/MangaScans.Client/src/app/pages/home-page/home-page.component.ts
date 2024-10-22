import { Component, signal } from '@angular/core';
import { HomeService } from '../../services/home/home.service';
import { environment } from '../../../environments/environment';
import { Manga } from '../../Interfaces/MangaResponse';
import { CardComponent } from '../../components/card/card.component';
import { Error404Component } from '../../components/error404/error404.component';


@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [ CardComponent, Error404Component ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})

  
export class HomePageComponent {

  // Url Base
  Url = environment.apiUrl;

  // page properties
  visibilityNotFound = signal<boolean>(false);
  page = signal<number>(1);

  // API
  totalPages = signal<number[]>([]);
  categories = signal<number[]>([]);
  mangas = signal<Manga[]>([]);;

  constructor(private homeService: HomeService) { }

  ngOnInit(): void {
    this.UpdateMangas();
  }

  UpdateMangas() {
    this.homeService.getTopMangas(this.page(), this.categories())
      .subscribe({
        next: (response) => {
          this.mangas.set(response.data);
          let page = [];
          for (let i = 1; i <= response.pages; i++) {
            page.push(i);
          }

          this.totalPages.set(page);
        },
        error: (error) => {
          this.mangas.set([]);
          this.visibilityNotFound.set(true);
        }
      });
  }

  onSelectionChange(event: any): void {
    this.page.set(event.target.value);
    this.UpdateMangas();
  }

  nextPage(next: boolean) {
    if (next) {
      if (this.page() + 1 > this.totalPages().length) return;
    
      this.page.set(this.page() + 1);

    } else {

      if (this.page() - 1 < 1) return;

      this.page.set(this.page() - 1);
    }

    this.UpdateMangas();
  }
  
}

