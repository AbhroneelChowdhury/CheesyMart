import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BannerHeaderComponent } from './banner-header.component';

describe('BannerHeaderComponent', () => {
  let component: BannerHeaderComponent;
  let fixture: ComponentFixture<BannerHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BannerHeaderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BannerHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
