import {createActionGroup, props} from '@ngrx/store';
import {CheesyProductModel} from '../services/cheesy-client.service';

export const CartActions = createActionGroup({
  source: 'Cart',
  events: {
    'Add Product': props<{Id: number}>(),
    'Remove Product': props<{Id: number}>(),
  },
});

export const CartApiActions = createActionGroup({
  source: 'Cart API',
  events: {
    'Retrieved Product List': props<{cart: ReadonlyArray<CheesyProductModel>}>(),
  },
});
