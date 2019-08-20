import { Component, OnInit, Input } from '@angular/core';
import { Step, Fragment } from '../job.models';

@Component({
  selector: 'app-step',
  templateUrl: './step.component.html',
  styleUrls: ['./step.component.scss']
})
export class StepComponent implements OnInit {
  @Input() step:Step;
  public selectedFragment:Fragment;

  constructor() { }

  ngOnInit() {
  }

  showFragment(fragment:Fragment) {
    this.selectedFragment = fragment;
  }

}
