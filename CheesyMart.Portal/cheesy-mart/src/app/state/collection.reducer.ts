import {createReducer, on} from '@ngrx/store';
import {CartActions} from './cart.actions';

export const initialState: ReadonlyArray<number> = [];

export const collectionReducer = createReducer(
  initialState,
  on(CartActions.removeProduct, (state, {Id}) => state.filter((id) => id !== Id)),
  on(CartActions.addProduct, (state, {Id}) => {
    if (state.indexOf(Id) > -1) return state;

    return [...state, Id];
  }),
);
