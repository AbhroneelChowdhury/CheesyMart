import {BreakpointObserver} from '@angular/cdk/layout';
import {Store} from '@ngrx/store';
import {selectCartCollection} from '../../state/cart.selectors';
import {ComponentBase} from '../componentbase';
import {Component, ViewChildren, QueryList, ElementRef} from '@angular/core';
import {CommonModule} from '@angular/common';
import {OnInit} from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';
import {CheesyProductModel} from '../../services/cheesy-client.service';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInput, MatInputModule} from '@angular/material/input';
import {PricePerKiloDirective} from '../../directives/price-per-kilo.directive';
import {MatButtonModule} from '@angular/material/button';
import {Router, RouterLink, RouterModule} from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatFormFieldModule,
    MatInputModule,
    PricePerKiloDirective,
    MatButtonModule,
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent extends ComponentBase implements OnInit {
  @ViewChildren('inputQuantity') inputQuantity!: QueryList<ElementRef>;
  cartCollection$ = this.store.select(selectCartCollection);
  selectedItems: (CheesyProductModel | undefined)[] = [];

  constructor(
    public override responsive: BreakpointObserver,
    private _snackBar: MatSnackBar,
    private store: Store,
  ) {
    super(responsive);
  }

  override ngOnInit() {
    super.ngOnInit();

    this.cartCollection$.forEach((item) => {
      this.selectedItems = item;
    });
  }

  calculate() {
    let total = 0;

    this.selectedItems.forEach((item, index) => {
      this.inputQuantity.forEach((entryPrice, idx) => {
        if (index === idx && entryPrice.nativeElement.value) {
          let lineItem = item?.pricePerKilo! * Number(entryPrice.nativeElement.value);
          total = total + lineItem;
        }
      });
    });

    this._snackBar.open('Your total is: $' + total, 'close');
  }
}
