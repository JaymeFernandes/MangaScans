import { Component, signal } from '@angular/core';
import { HomeService } from '../../services/home/home.service';
import { environment } from '../../../environments/environment';
import { Manga } from '../../Interfaces/MangaResponse';

const messages = [
  'Eita! Parece que não achei nenhum mangá por aqui.',
  'Não achei nenhum mangá por aqui.',
  'Ops! Os mangás foram dar uma volta, não achei nenhum por aqui.',
  'Eita lasqueira! Cadê os mangás? Sumiram todos!',
  'Parece que os mangás estão de férias... não encontrei nenhum.',
  'Que bruxaria é essa? Não achei nenhum mangá!',
  'Ué, cadê os mangás? Estão se escondendo de mim!',
  'Vixe! Os mangás desapareceram como um jutsu ninja.',
  'Foi mal, mas os mangás foram abduzidos!',
  'Ai ai... parece que os mangás fugiram do estoque.',
  'Xiii, os mangás devem estar em outra dimensão... nenhum aqui!',
  'Ops, nenhum mangá por aqui! Deve ter sido o bug do milênio.',
  'Criador preguiçoso esqueceu de colocar os mangas de novo!'
];

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})

  
export class HomePageComponent {

  // Url Base
  Url = environment.apiUrl;

  // page properties
  visibilityNotFound = signal<boolean>(false);
  page = signal<number>(1);
  text404 = signal<string>('')

  // API
  totalPages = signal<number[]>([]);
  categories = signal<number[]>([]);
  mangas = signal<Manga[]>([]);;

  constructor(private homeService: HomeService) { }

  ngOnInit(): void {
    this.text404.set(messages[Math.floor(Math.random() * messages.length)]);
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
        },
        complete: () => {
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

