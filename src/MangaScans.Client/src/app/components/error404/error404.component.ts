import { Component, signal } from '@angular/core';

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
  selector: 'app-error404',
  standalone: true,
  imports: [],
  templateUrl: './error404.component.html',
  styleUrl: './error404.component.scss'
})
export class Error404Component {
  text404 = signal<string>('')

  constructor() {
    this.text404.set(messages[Math.floor(Math.random() * messages.length)]);
  }
}
