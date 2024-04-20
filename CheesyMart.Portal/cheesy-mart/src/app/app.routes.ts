import {Routes} from '@angular/router';
import {ChesseSelectionComponent} from './components/chesse-selection/chesse-selection.component';
import {ManageCheeseProductComponent} from './components/manage-cheese-product/manage-cheese-product.component';
import {CartComponent} from './components/cart/cart.component';

export const routes: Routes = [
  {path: '', component: ChesseSelectionComponent},
  {path: 'manage-product', component: ManageCheeseProductComponent},
  {path: 'manage-product/:id', component: ManageCheeseProductComponent},
  {path: 'cart', component: CartComponent},
  {path: '**', component: ChesseSelectionComponent},
];
