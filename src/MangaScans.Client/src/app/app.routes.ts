import { Routes } from "@angular/router";
import { HomePageComponent } from "./pages/home-page/home-page.component";
import { MangaPageComponent } from "./pages/manga-page/manga-page.component";
import { ReadPageComponent } from "./pages/read-page/read-page.component";
import { authGuard, loginGuard } from "./auth/auth.guard";
import { LoginPageComponent } from "./pages/login-page/login-page.component";

export const routes: Routes = [
  {
    path: "",
    component: HomePageComponent,
  },
  {
    path: "manga/:id",
    component: MangaPageComponent,
  },
  {
    path: "read/:id/:chapter",
    component: ReadPageComponent,
    canActivate: [authGuard]
  },
  {
    path: "login",
    component: LoginPageComponent,
    canActivate: [loginGuard],
  },
];
