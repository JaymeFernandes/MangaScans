import { Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { MangaPageComponent } from './pages/manga-page/manga-page.component';
import { ReadPageComponent } from './pages/read-page/read-page.component';

export const routes: Routes = [
    { path: '', component: HomePageComponent },
    { path: 'manga/:id', component: MangaPageComponent },
    { path: "read/:id/:chapter", component: ReadPageComponent }
];
