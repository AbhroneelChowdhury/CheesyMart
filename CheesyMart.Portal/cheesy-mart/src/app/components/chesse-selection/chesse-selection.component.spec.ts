import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChesseSelectionComponent } from './chesse-selection.component';

describe('ChesseSelectionComponent', () => {
  let component: ChesseSelectionComponent;
  let fixture: ComponentFixture<ChesseSelectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChesseSelectionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ChesseSelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
