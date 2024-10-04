import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MangaResponse } from '../../Interfaces/MangaResponse';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  private Url = environment.apiUrl;
  private recommendationURL = 'api/recommendation/';
  private searchURL = 'api/search/';


  constructor(private http: HttpClient) { }

  getTopMangas(page: number, categories: number[]): Observable<MangaResponse> {
    const headers = new HttpHeaders({
      'Page': page.toString(),
      'Categories': categories.join(',')
    });

    return this.http.get<MangaResponse>(`${this.Url}${this.recommendationURL}`, { headers });
  }
}
