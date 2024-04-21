import {ApplicationConfig} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {provideStore} from '@ngrx/store';
import {cartReducer} from './state/cart.reducer';
import {collectionReducer} from './state/collection.reducer';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideStore({
      cart: cartReducer,
      collection: collectionReducer,
    }),
  ],
};
