import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject, NgZone } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const ErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toaster = inject(ToastrService);
  const ngZone = inject(NgZone); // Ensure UI updates happen in the Angular zone

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      ngZone.run(() => {
        if (error) {
          console.log(error);
          switch (error.status) {
            case 400:
              // Check if error.error is an array (your case)
              if (Array.isArray(error.error) && error.error.length > 0) {
                // Extract descriptions from the error array
                const errorMessages = error.error
                  .map((err: any) => err.description)
                  .join(', ');
                toaster.error(errorMessages, 'Bad Request');
              }
              // Fallback for other 400 error formats (e.g., model state errors)
              else if (error.error && error.error.errors) {
                const modelStateErrors = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                toaster.error(
                  modelStateErrors.flat().join(', '),
                  'Bad Request'
                );
              }
              // Generic 400 error fallback
              else if (error.error) {
                toaster.error(error.error, 'Bad Request');
              } else {
                toaster.error('Bad request occurred', 'Bad Request');
              }
              break;
            case 401:
              toaster.error('Unauthorised', error.status.toString());
              break;
            case 404:
              router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {
                state: { error: error.error },
              };
              router.navigateByUrl('/server-error', navigationExtras);
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
