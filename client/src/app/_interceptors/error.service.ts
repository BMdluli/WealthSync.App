import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const ErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toaster = inject(ToastrService);
  const ngZone = inject(NgZone); // Ensure UI updates happen in the Angular zone

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      ngZone.run(() => {
        // Ensures Toastr runs inside Angular's change detection
        if (error) {
          switch (error.status) {
            case 400:
              toaster.error('Invalid request', 'Error 400');
              break;
            case 401:
              toaster.error('Invalid credentials', 'Unauthorized'); // Toastr should now work
              break;
            case 404:
              router.navigateByUrl('/not-found');
              break;
            case 500:
              router.navigateByUrl('/server-error', {
                state: { error: error.error },
              });
              break;
            default:
              toaster.error('Something unexpected went wrong');
              console.log(error);
          }
        }
      });
      return throwError(() => error);
    })
  );
};
