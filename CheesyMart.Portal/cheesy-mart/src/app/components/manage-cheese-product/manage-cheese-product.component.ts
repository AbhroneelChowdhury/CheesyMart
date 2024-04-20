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

import {MatInputModule} from '@angular/material/input';
import {PricePerKiloDirective} from '../../directives/price-per-kilo.directive';
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
          return of();
        }),
        takeUntil(this.ngUnsubscribe$),
      )
      .subscribe();
  }

  mapForEdit(product: CheesyProductModel): void {
    this.imageIds = product.productImages ?? [];
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
      //this.pageNotificationComponent.showValidationSummaryError(this.formGroup.valid, errors);
      return;
    }

    this.isSaving = true;
    let request = new CheesyProductModel();
    request.name = this.formGroup.value.name;
    request.cheeseType = this.formGroup.value.cheeseType;
    request.color = this.formGroup.value.cheeseColor;
    request.pricePerKilo = this.formGroup.value.pricePerKilo;

    request.productImages = this.imageIds ? [] : this.imageIds;

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
            //this.pageNotificationComponent!.showError(error);
            return of();
          }),
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
            //this.pageNotificationComponent!.showError(message);
            return of();
          }),
        )
        .subscribe((res) => console.log(res));
    }
  }
}

// private checkCheeseNameExists(name: string): Observable<boolean> {
// 	this.isCheckingName = true;

// 	return this.partnerServiceClient.checkPartnerServiceExists(name).pipe(
// 		tap(result => {
// 			this.isCheckingName = false;

// 			if (result.exists && result.exists === true) {
// 				console.log('Found existing partner service with name');
// 				this.formGroup.controls.partnerServiceName.setErrors({
// 					partnerServiceNameExists: 'Partner service name already exists',
// 				});
// 				this.formGroup.controls.partnerServiceName.markAsTouched();
// 			}

// 			this.isCheckingName = false;
// 		}),
// 		catchError(error => {
// 			this.isCheckingName = false;
// 			console.error(error);
// 			return EMPTY;
// 		})
// 	);
// }

// uploadLogo(event: any) {
// 	this.pageNotificationComponent.closeNotification();
// 	if (event.target.files.length > 0) {
// 		this.logoUploadStatus = UploadStatus.Uploading;
// 		const file: File = event.target.files[0];
// 		const fileReader = new FileReader();
// 		fileReader.onload = e => {
// 			if (file.size < 1) {
// 				this.showUploadError('The size of the file uploaded must be greater than zero.');
// 				return;
// 			} else if (file.size > 1024 * 1024) {
// 				this.showUploadError('The size of the file uploaded must be less than 1MB.');
// 				return;
// 			}

// 			const fileRegExp = /^data\:(.*);base64,(.*)/g;
// 			const fileRegExpResults = fileRegExp.exec(fileReader.result!.toString());

// 			if (fileRegExpResults) {
// 				const mimeType = fileRegExpResults[1];
// 				if (
// 					!(
// 						mimeType === 'image/gif' ||
// 						mimeType === 'image/jpeg' ||
// 						mimeType === 'image/png' ||
// 						mimeType === 'image/svg+xml'
// 					)
// 				) {
// 					this.showUploadError('The type of the file uploaded must be GIF, JPEG, PNG or SVG.');
// 					return;
// 				}
// 				if (mimeType === 'image/svg+xml') {
// 					this.setLogoImage(fileRegExpResults[2], mimeType);
// 				} else {
// 					this.validateImageSize(fileReader.result!.toString())
// 						.pipe(
// 							tap(() => {
// 								this.setLogoImage(fileRegExpResults[2], mimeType);
// 							}),
// 							catchError(error => {
// 								this.showUploadError(error);
// 								return of();
// 							})
// 						)
// 						.subscribe();
// 				}
// 			}
// 		};
// 		fileReader.readAsDataURL(file);
// 	}
// }
