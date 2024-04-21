import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, FormsModule, Validators} from '@angular/forms';
import {RouterLink} from '@angular/router';
import {forkJoin, Observable, of} from 'rxjs';
import {catchError, finalize, takeUntil, tap} from 'rxjs/operators';
import {
  CheesyProductModel,
  CheesyProductsModel,
  CheesyProductCatalogClient,
} from '../../services/cheesy-client.service';

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

  constructor(
    private cheesyClient: CheesyProductCatalogClient,
    private el: ElementRef,
    public dialog: MatDialog,
    public override responsive: BreakpointObserver,
    private _snackBar: MatSnackBar,
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
        tap(() => (this.state = 'loaded')),
        catchError((error) => {
          this.state = 'error';
          this._snackBar.open(error, 'close');
          return of();
        }),
        takeUntil(this.ngUnsubscribe$),
      )
      .subscribe();
  }

  deleteCheeseFromCatalog(chesse: CheesyProductModel) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: `Delete ${chesse.name} from catalog`,
        message: `Are you sure you want to delete ${chesse.name} from catalog ?`,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.cheesyClient
          .delete(chesse.id!)
          .pipe(
            tap(() => this.loadCatalog()),
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
          if (response && response.products) this.dataModel = response.products;
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
}
