import { Component, OnInit, Input } from '@angular/core';
import { Job } from '../job.models';

@Component({
  selector: 'app-job-header',
  templateUrl: './job-header.component.html',
  styleUrls: ['./job-header.component.scss']
})
export class JobHeaderComponent implements OnInit {
  @Input() job:Job;

  constructor() { }

  ngOnInit() {
  }

}
