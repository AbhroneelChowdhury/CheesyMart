import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageCheeseProductComponent } from './manage-cheese-product.component';

describe('ManageCheeseProductComponent', () => {
  let component: ManageCheeseProductComponent;
  let fixture: ComponentFixture<ManageCheeseProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageCheeseProductComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ManageCheeseProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
