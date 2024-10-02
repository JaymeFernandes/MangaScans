import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, retry, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  private Url = environment.apiUrl;
  private recommendationURL = 'recommendation/';
  private searchURL = 'search/';


  constructor(private http: HttpClient) { }

  GetRecommendation(page:number = 1) : Observable<any> {
    return this.http.get<any>('${Url}${recommendationURL}${page}')
      .pipe(
        retry(3),
        catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Erro desconhecido!';
    if (error.error instanceof ErrorEvent) {
      // Erro no lado do cliente
      errorMessage = `Erro: ${error.error.message}`;
    } else {
      // Erro no lado do servidor
      errorMessage = `Código do erro: ${error.status}\nMensagem: ${error.message}`;
    }
    
    console.error(errorMessage);
    // Retorna um Observable com um erro
    return throwError(() => new Error(errorMessage)); // Retornar um erro com throwError
  }
}
