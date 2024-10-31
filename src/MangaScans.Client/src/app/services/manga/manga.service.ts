import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { MangaInfoResponse } from '../../Interfaces/MangaResponse';
import { ActionResultResponse } from '../../Interfaces/ActionResultResponse';

@Injectable({
  providedIn: 'root'
})
export class MangaService {
  private Url = environment.apiUrl;


  constructor(private http: HttpClient) { }
  
  GetManga(id: string): Observable<MangaInfoResponse> {
    return this.http.get<MangaInfoResponse>(`${this.Url}api/${id}`);
  }

  MangaLike(id: string, isLike: boolean): Observable<any> {
    const headers = new HttpHeaders({
      mangaId: id,
      Authorization: `Bearer ${localStorage.getItem('token')}`
    });

    return this.http.post<ActionResultResponse>(`${this.Url}api/like`, headers);
  }

}
