import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject} from 'rxjs';

@Component({
  template: '',
})
export abstract class ComponentBase implements OnDestroy, OnInit {
  // Subscription Un-subscribe trigger
  ngUnsubscribe$ = new Subject<void>();

  isLoading: boolean = false;
  private _breakpoints?: {[p: string]: boolean};
  get breakpoints() {
    return this._breakpoints!;
  }
  protected readonly Breakpoints = Breakpoints;

  constructor(public responsive: BreakpointObserver) {}

  /** Angular Lifecycle Hook - Destroy all angular subscriptions. */
  ngOnDestroy() {
    this.ngUnsubscribe$.next();
    this.ngUnsubscribe$.complete();
  }

  ngOnInit(): void {
    this.responsive
      .observe([Breakpoints.Medium, Breakpoints.Small, Breakpoints.XSmall])
      .subscribe((result) => {
        this._breakpoints = result.breakpoints;

        if (this._breakpoints[Breakpoints.Small]) {
          console.log('screens matches small');
        } else if (this._breakpoints[Breakpoints.XSmall]) {
          console.log('screens matches extra small');
        }
      });
  }
}
