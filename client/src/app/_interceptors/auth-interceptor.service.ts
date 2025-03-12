import { HttpInterceptorFn } from '@angular/common/http';

export const AuthInterceptorService: HttpInterceptorFn = (req, next) => {
  const authReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${localStorage.getItem('token') || ''}`,
    },
  });

  return next(authReq);
};
