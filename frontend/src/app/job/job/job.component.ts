import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { JobService } from '../job.service';
import { Job, Fragment } from '../job.models';

@Component({
  selector: 'app-job',
  templateUrl: './job.component.html',
  styleUrls: ['./job.component.scss']
})
export class JobComponent implements OnInit {
  private job:Job;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private service: JobService) { }

  ngOnInit() {
    let id = this.route.snapshot.paramMap.get('id');

    this.service.getById(id).subscribe(res => {
      this.job = res;
    });
  }
}
