import {Component, ElementRef, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatDialog} from '@angular/material/dialog';
import {ActivatedRoute, Router, RouterLink, RouterModule} from '@angular/router';
import {forkJoin, of} from 'rxjs';
import {catchError, takeUntil, tap} from 'rxjs/operators';
import {
  CheesyProductCatalogClient,
  CheesyProductModel,
  MetadataClient,
  ProductImageClient,
  ProductImageModel,
} from '../../services/cheesy-client.service';
import {ComponentBase} from '../componentbase';
import {BreakpointObserver} from '@angular/cdk/layout';
import {metadataConstants} from '../../utils/metadata-constants';
import {CommonModule} from '@angular/common';
import {HttpClientModule} from '@angular/common/http';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {CamelSpacePipe} from '../../pipes/camel-space.pipe';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatSnackBar} from '@angular/material/snack-bar';
import {MatInputModule} from '@angular/material/input';
import {PricePerKiloDirective} from '../../directives/price-per-kilo.directive';
import {MatCardModule} from '@angular/material/card';
import {environment} from '../../../environments/environment';
import {ConfirmationDialogComponent} from '../../dialogs/confirmation-dialog/confirmation-dialog.component';
enum UploadStatus {
  NotUploaded,
  Uploading,
  Uploaded,
}

@Component({
  selector: 'app-manage-cheese-product',
  standalone: true,
  providers: [CheesyProductCatalogClient, MetadataClient, ProductImageClient],
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatSelectModule,
    MatIconModule,
    HttpClientModule,
    CamelSpacePipe,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatCardModule,
    RouterModule,
    MatInputModule,
    ReactiveFormsModule,
    PricePerKiloDirective,
  ],
  templateUrl: './manage-cheese-product.component.html',
  styleUrl: './manage-cheese-product.component.scss',
})
export class ManageCheeseProductComponent extends ComponentBase implements OnInit {
  isEdit = false;
  isSaving = false;
  id?: number;
  cheeseProduct?: CheesyProductModel;
  protected readonly uploadStatus = UploadStatus;
  currentImageId?: number;
  currentSrc: string = '';

  cheeseTypes: string[] = [];
  cheeseColorTypes: string[] = [];
  imageIds: number[] = [];

  state: string = 'busy';
  logoUploadStatus?: UploadStatus;

  formGroup: FormGroup<{
    name: FormControl<string>;
    cheeseType: FormControl<string>;
    cheeseColor: FormControl<string>;
    pricePerKilo: FormControl<number>;
  }>;

  constructor(
    private dialog: MatDialog,
    private elementRef: ElementRef,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private cheesyClient: CheesyProductCatalogClient,
    private productImageClient: ProductImageClient,
    private metadataClient: MetadataClient,
    private _snackBar: MatSnackBar,
    public override responsive: BreakpointObserver,
  ) {
    super(responsive);

    // Route params
    this.id = this.route.snapshot.params['id'];
    this.isEdit = !!this.id;

    // Create the form
    this.formGroup = this.formBuilder.nonNullable.group({
      name: ['', [Validators.required]],
      cheeseType: ['', [Validators.required]],
      cheeseColor: [''],
      pricePerKilo: [0],
    });

    this.disableForm();
  }

  override ngOnInit(): void {
    super.ngOnInit();
    const initObservables = [];

    initObservables.push(
      this.metadataClient.getValuesByType(metadataConstants.CheeseType).pipe(
        tap((res) => {
          if (res.items) {
            this.cheeseTypes = res.items;
          }
        }),
      ),
      this.metadataClient.getValuesByType(metadataConstants.CheeseColor).pipe(
        tap((res) => {
          if (res.items) {
            this.cheeseColorTypes = res.items;
          }
        }),
      ),
    );

    if (this.id) {
      initObservables.push(
        this.cheesyClient.get(this.id!).pipe(
          tap((product) => {
            this.cheeseProduct = product;
          }),
        ),
      );
    }

    forkJoin(initObservables)
      .pipe(
        tap(() => {
          if (this.isEdit) this.mapForEdit(this.cheeseProduct!);
          this.isLoading = false;
          this.state = 'loaded';
          this.enableForm();
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

  mapForEdit(product: CheesyProductModel): void {
    this.imageIds = product.productImages ?? [];
    if (this.imageIds.length > 0) {
      this.currentImageId = this.imageIds[0];
      this.currentSrc = `${environment.cheesymartApiEndpoint}/api/ProductImage/${this.imageIds[0]}`;
    }

    this.formGroup.patchValue({
      name: product.name,
      cheeseType: product.cheeseType,
      cheeseColor: product.color,
      pricePerKilo: product.pricePerKilo,
    });
  }

  disableForm() {
    this.formGroup.disable();
  }

  enableForm() {
    this.formGroup.enable();
  }

  save() {
    if (!this.formGroup.valid) {
      this._snackBar.open('Please check the inputs', 'close');
      return;
    }

    this.isSaving = true;
    let request = new CheesyProductModel();
    request.name = this.formGroup.value.name;
    request.cheeseType = this.formGroup.value.cheeseType;
    request.color = this.formGroup.value.cheeseColor;
    request.pricePerKilo = this.formGroup.value.pricePerKilo;
    request.productImages = this.imageIds;

    if (this.isEdit) {
      request.id = this.id;
      this.cheesyClient
        .update(this.id!, request)
        .pipe(
          tap(() => {
            this.router.navigate(['']);
          }),
          catchError((error) => {
            this.isSaving = false;
            this._snackBar.open(error, 'close');
            return of();
          }),
          takeUntil(this.ngUnsubscribe$),
        )
        .subscribe();
    } else {
      this.cheesyClient
        .create(request)
        .pipe(
          tap(() => {
            this.router.navigate(['']);
          }),
          catchError((error) => {
            this.isSaving = false;
            this._snackBar.open(error, 'close');
            return of();
          }),
          takeUntil(this.ngUnsubscribe$),
        )
        .subscribe((res) => console.log(res));
    }
  }

  deleteImage() {
    if (this.currentImageId) {
      const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        data: {
          title: `Delete image`,
          message: `Are you sure you want to delete ?`,
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.productImageClient
            .delete(this.currentImageId!)
            .pipe(
              tap(() => {
                this.removeCurrentImageFromList();
                this.currentImageId = undefined;
                this.nextImage();
              }),
              catchError((error) => {
                this._snackBar.open(error, 'close');
                return of();
              }),
              takeUntil(this.ngUnsubscribe$),
            )
            .subscribe();
        }
      });
    }
  }

  removeCurrentImageFromList() {
    const indexToRemove = this.imageIds.indexOf(this.currentImageId!);
    if (indexToRemove > -1) {
      this.imageIds.splice(indexToRemove, 1);
    }
  }

  nextImage() {
    if (!this.currentImageId && this.imageIds.length > 0) {
      this.currentImageId = this.imageIds[0];
      this.currentSrc = `${environment.cheesymartApiEndpoint}/api/ProductImage/${this.currentImageId}`;
    } else {
      const indexToCurrent = this.imageIds.indexOf(this.currentImageId!);
      if (indexToCurrent === this.imageIds.length - 1) {
        this.currentImageId = this.imageIds[0];
        this.currentSrc = `${environment.cheesymartApiEndpoint}/api/ProductImage/${this.currentImageId}`;
      } else {
        this.currentImageId = this.imageIds[indexToCurrent + 1];
        this.currentSrc = `${environment.cheesymartApiEndpoint}/api/ProductImage/${this.currentImageId}`;
      }
    }
  }

  uploadLogo(event: any) {
    if (event.target.files.length > 0) {
      this.logoUploadStatus = UploadStatus.Uploading;
      const file: File = event.target.files[0];
      const fileReader = new FileReader();
      fileReader.onload = (e) => {
        if (file.size < 1) {
          this._snackBar.open('The size of the file uploaded must be greater than zero.', 'close');
          return;
        } else if (file.size > 1024 * 1024) {
          this._snackBar.open('The size of the file uploaded must be less than 1MB.', 'close');
          return;
        }

        const fileRegExp = /^data\:(.*);base64,(.*)/g;
        const fileRegExpResults = fileRegExp.exec(fileReader.result!.toString());

        if (fileRegExpResults) {
          const mimeType = fileRegExpResults[1];
          if (
            !(
              mimeType === 'image/gif' ||
              mimeType === 'image/jpeg' ||
              mimeType === 'image/png' ||
              mimeType === 'image/svg+xml'
            )
          ) {
            this._snackBar.open(
              'The type of the file uploaded must be GIF, JPEG, PNG or SVG.',
              'close',
            );
            return;
          }

          this.setLogoImage(fileRegExpResults[2], mimeType);
        }
      };
      fileReader.readAsDataURL(file);
    }
  }

  setLogoImage(data: string, mimeType: string) {
    let productImageRequest = new ProductImageModel();
    productImageRequest.alternateText =
      this.formGroup.value.name === '' ? 'unable to load image' : this.formGroup.value.name;
    productImageRequest.cheesyProductId = this.id;
    productImageRequest.data = data;
    productImageRequest.mimeType = mimeType;

    this.productImageClient
      .create(productImageRequest)
      .pipe(
        tap((response) => {
          this.logoUploadStatus = UploadStatus.Uploaded;
          this.imageIds.push(response.id!);
          this.currentImageId = response.id;
          this.currentSrc = `${environment.cheesymartApiEndpoint}/api/ProductImage/${response.id}`;
        }),
        catchError((error) => {
          this.logoUploadStatus = UploadStatus.NotUploaded;
          return of();
        }),
        takeUntil(this.ngUnsubscribe$),
      )
      .subscribe();
  }
}
