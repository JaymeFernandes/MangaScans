import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PageStatusService {

  private MenuState = new BehaviorSubject<Boolean>(false);
  menuState$ = this.MenuState.asObservable();

  ToggleMenu() {
    this.MenuState.next(!this.MenuState.value);
  }

  constructor() { }
}
