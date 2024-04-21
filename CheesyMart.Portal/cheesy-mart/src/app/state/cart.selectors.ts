import {createSelector, createFeatureSelector} from '@ngrx/store';
import {CheesyProductModel} from '../services/cheesy-client.service';

export const selectCart = createFeatureSelector<ReadonlyArray<CheesyProductModel>>('cart');

export const selectCollectionState = createFeatureSelector<ReadonlyArray<number>>('collection');

export const selectCartCollection = createSelector(
  selectCart,
  selectCollectionState,
  (cart, collection) => {
    return collection.map((Id) => cart.find((cart) => cart.id === Id));
  },
);
