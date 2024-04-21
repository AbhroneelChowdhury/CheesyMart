import {CheesyProductModel} from '../services/cheesy-client.service';

export interface AppState {
  cart: ReadonlyArray<CheesyProductModel>;
  collection: ReadonlyArray<number>;
}
