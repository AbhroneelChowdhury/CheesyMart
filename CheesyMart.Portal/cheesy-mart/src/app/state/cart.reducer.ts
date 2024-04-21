import {createReducer, on} from '@ngrx/store';
import {CartApiActions} from './cart.actions';
import {CheesyProductModel} from '../services/cheesy-client.service';

export const initialState: ReadonlyArray<CheesyProductModel> = [];

export const cartReducer = createReducer(
  initialState,
  on(CartApiActions.retrievedProductList, (_state, {cart}) => cart),
);
