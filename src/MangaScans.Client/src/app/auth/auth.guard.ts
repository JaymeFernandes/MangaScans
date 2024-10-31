import { CanActivateFn } from "@angular/router";
import { AuthService } from "../services/authentication/authentication.service";
import { inject, Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root",
})
class PermissionsServices {
  constructor(
    private auth: AuthService,
    private routerServices: Router,
  ) {}

  async GetSession(): Promise<boolean> {
    const isAuthenticated = await this.auth.GetSession();

    if (!isAuthenticated) {
      await this.routerServices.navigate(["/login/"]);
    }
    return isAuthenticated;
  }

  async LoginSession(): Promise<boolean> {
    const isAuthenticated = await this.auth.GetSession();
    if (isAuthenticated) {
      await this.routerServices.navigate(["/"]);
    }
    return !isAuthenticated;
  }
}

export const authGuard: CanActivateFn = (route, state) => {
  return inject(PermissionsServices).GetSession();
};

export const loginGuard: CanActivateFn = (route, state) => {
  return inject(PermissionsServices).LoginSession();
};
