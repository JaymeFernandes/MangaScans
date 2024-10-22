import { Component, signal } from '@angular/core';
import { BookIconComponent } from '../../components/icons/book-icon/book-icon.component';
import { MangaService } from '../../services/manga/manga.service';
import { MangaInfoResponse } from '../../Interfaces/MangaResponse';
import { environment } from '../../../environments/environment.development';
import { Error404Component } from '../../components/error404/error404.component';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Action } from 'rxjs/internal/scheduler/Action';

@Component({
  selector: 'app-manga-page',
  standalone: true,
  imports: [ BookIconComponent, Error404Component, RouterModule ],
  templateUrl: './manga-page.component.html',
  styleUrl: './manga-page.component.scss'
})

export class MangaPageComponent {
  Url = environment.apiUrl;
  page = signal<number>(1);
  mangaId : string | null = null;

  pages = signal<number[]>([]);

  manga = signal<MangaInfoResponse | null>(null);
  is404 = signal<boolean>(false);

  constructor(private mangaService: MangaService, private route : ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.mangaId = params.get('id');
    });

    if (this.mangaId == null) {
      this.is404.set(true);
      return;
    }

    this.mangaService.GetManga(this.mangaId).subscribe({
      next: (response) => {
        var pagesTotal = Math.ceil((response.data.chapters?.length ?? 0) / 10);

        for (let i = 1; i <= pagesTotal; i++) {
          this.pages.update(pages => [...pages, i]);
        }

        this.manga.set(response);
      },
      error: (error) => {
        this.is404.set(true);
      }
    });
  }

  nextPage(next: boolean) {
    if (next) {
      if (this.page() + 1 > this.pages().length) return;
    
      this.page.set(this.page() + 1);

    } else {

      if (this.page() - 1 < 1) return;

      this.page.set(this.page() - 1);
    }
  }
}
