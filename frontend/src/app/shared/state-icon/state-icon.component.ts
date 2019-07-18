import { Component, OnInit, Input } from '@angular/core';
import { ProcessingState } from 'src/app/job/job.models';

@Component({
  selector: 'app-state-icon',
  templateUrl: './state-icon.component.html',
  styleUrls: ['./state-icon.component.scss']
})
export class StateIconComponent implements OnInit {
  @Input() state: ProcessingState;

  constructor() { }

  ngOnInit() {
  }

}
