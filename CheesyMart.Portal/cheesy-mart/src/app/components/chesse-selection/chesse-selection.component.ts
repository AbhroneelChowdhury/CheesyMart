import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ElementRef, OnInit} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {RouterLink} from '@angular/router';
import {forkJoin, Observable, of} from 'rxjs';
import {catchError, takeUntil, tap} from 'rxjs/operators';
import {CheesyProductModel, CheesyProductCatalogClient} from '../../services/cheesy-client.service';

import {MatButtonModule} from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';
import {HttpClientModule} from '@angular/common/http';
import {MatIconModule} from '@angular/material/icon';
import {MatChipsModule} from '@angular/material/chips';
import {ConfirmationDialogComponent} from '../../dialogs/confirmation-dialog/confirmation-dialog.component';
import {MatDialog} from '@angular/material/dialog';
import {ComponentBase} from '../componentbase';
import {BreakpointObserver} from '@angular/cdk/layout';
import {CamelSpacePipe} from '../../pipes/camel-space.pipe';
import {RouterModule} from '@angular/router';
import {environment} from '../../../environments/environment';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatSnackBar} from '@angular/material/snack-bar';
import {Store} from '@ngrx/store';
import {CartActions, CartApiActions} from '../../state/cart.actions';
import {selectCartCollection} from '../../state/cart.selectors';

@Component({
  selector: 'app-chesse-selection',
  standalone: true,
  providers: [CheesyProductCatalogClient],
  templateUrl: './chesse-selection.component.html',
  styleUrl: './chesse-selection.component.scss',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatIconModule,
    HttpClientModule,
    MatChipsModule,
    CamelSpacePipe,
    RouterModule,
  ],
})
export class ChesseSelectionComponent extends ComponentBase implements OnInit {
  state = 'busy';
  dataModel: CheesyProductModel[] = [];
  cartCollection$ = this.store.select(selectCartCollection);

  constructor(
    private cheesyClient: CheesyProductCatalogClient,
    private el: ElementRef,
    public dialog: MatDialog,
    public override responsive: BreakpointObserver,
    private _snackBar: MatSnackBar,
    private store: Store,
  ) {
    super(responsive);
  }

  override ngOnInit() {
    super.ngOnInit();
    const initObservables: Observable<any>[] = [];
    initObservables.push(
      this.cheesyClient.getAll(null, null, null).pipe(
        tap((response) => {
          if (response && response.products) this.dataModel = response.products;
        }),
      ),
    );

    forkJoin(initObservables)
      .pipe(
        tap(() => {
          this.state = 'loaded';
          let cart = this.dataModel;
          this.store.dispatch(CartApiActions.retrievedProductList({cart}));
        }),
        catchError((error) => {
          this.state = 'error';
          this._snackBar.open(error, 'close');
          return of();
        }),
        takeUntil(this.ngUnsubscribe$),
      )
      .subscribe();
  }

  deleteCheeseProductFromCatalog(chesseProduct: CheesyProductModel) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: `Delete ${chesseProduct.name} from catalog`,
        message: `Are you sure you want to delete ${chesseProduct.name} from catalog ?`,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.cheesyClient
          .delete(chesseProduct.id!)
          .pipe(
            tap(() => {
              let Id = chesseProduct.id!;
              this.store.dispatch(CartActions.removeProduct({Id}));
              this.loadCatalog();
            }),
            catchError((error) => {
              this.state = 'error';
              return of();
            }),
            takeUntil(this.ngUnsubscribe$),
          )
          .subscribe();
      }
    });
  }

  loadCatalog() {
    this.cheesyClient
      .getAll(null, null, null)
      .pipe(
        tap((response) => {
          if (response && response.products) {
            this.dataModel = response.products;
          }
        }),
        catchError((error) => {
          this.state = 'error';
          return of();
        }),
        takeUntil(this.ngUnsubscribe$),
      )
      .subscribe();
  }

  getProductImageUrl(id: number) {
    return `${environment.cheesymartApiEndpoint}/api/ProductImage/${id}`;
  }

  AddToCart(cheeseProduct: CheesyProductModel) {
    if (cheeseProduct) {
      let Id = cheeseProduct.id!;
      this.store.dispatch(CartActions.addProduct({Id}));
    }
  }

  RemoveFromCart(cheeseProduct: CheesyProductModel) {
    if (cheeseProduct) {
      let Id = cheeseProduct.id!;
      this.store.dispatch(CartActions.removeProduct({Id}));
    }
  }

  CheckIfAdddedToCart(cheeseProduct: CheesyProductModel) {
    let found: boolean = false;
    this.cartCollection$.forEach((item) => {
      item.forEach((pi) => {
        if (pi?.id === cheeseProduct.id) found = true;
      });
    });

    return found;
  }
}
