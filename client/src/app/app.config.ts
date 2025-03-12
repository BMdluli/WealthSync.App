import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptorService } from './_interceptors/auth-interceptor.service';
import { ErrorInterceptor } from './_interceptors/error.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(
      withInterceptors([ErrorInterceptor, AuthInterceptorService])
    ),
    provideRouter(routes),
    provideAnimations(),
    provideToastr({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
  ],
};
