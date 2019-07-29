import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { JobService } from '../job.service';
import { Job, ProcessingState } from '../job.models';
import { Label, SingleDataSet } from 'ng2-charts';
import { ChartType, ChartOptions } from 'chart.js';

import * as moment from 'moment';

import { timer } from 'rxjs';


interface Metric {
  step:string,
  name:string,
  value:any
}

@Component({
  selector: 'app-job',
  templateUrl: './job.component.html',
  styleUrls: ['./job.component.scss']
})
export class JobComponent implements OnInit, OnDestroy {
  private job:Job;
  private metrics:Array<Metric>

  private updateTimer;

  private time_data:SingleDataSet;
  private time_labels:Label[];
  private chartType: ChartType = 'doughnut';
  private chartOptions: ChartOptions = { maintainAspectRatio: false }
  private chartColors = [{backgroundColor: ["#e84351", "#434a54", "#3ebf9b", "#4d86dc", "#f3af37"]}]

  constructor(private route: ActivatedRoute,
              private router: Router,
              private service: JobService) { }

  ngOnInit() {
    let id = this.route.snapshot.paramMap.get('id');

    this.updateTimer = timer(0, 10000).subscribe(() => {
      this.service.getById(id).subscribe(res => this.updateData(res))
    })
  }

  updateData(res) {

    if(this.job == null) {
      this.job = res;
    } else {
      this.job.finished = res.finished;
      this.job.state = res.state;
    }

    if(this.metrics == null)
      this.metrics = new Array<Metric>()

    this.time_data = []
    this.time_labels = [];

    for(let step of res.steps) {
      let existing = this.job.steps.find(s => s.number == step.number);
      if(existing == null) {
        this.job.steps.push(step);
      }
      else {
        existing.start = step.start;
        existing.end = step.end;
        existing.state = step.state;

        if(step.fragments != null) {
          if(existing.fragments == null)
            existing.fragments = [];

          for(let fragment of step.fragments) {
            let ef = existing.fragments.find(f => f.id == fragment.id);
            if(ef == null)
              existing.fragments.push(fragment);
          }
        }
      }

      if(step.start) {
        let start = moment(step.start);
        let end = step.end ? moment(step.end) : moment();

        let time = moment.duration(end.diff(start));

        this.time_data.push(time.asSeconds())
        this.time_labels.push(step.name);
      }

      if(step.metrics != null) {
        for(let key of Object.keys(step.metrics)) {
          let metric = this.metrics.find(x => x.step == step.name && x.name == key);

          if(metric == null) {
            this.metrics.push({
              step: step.name,
              name: key,
              value: step.metrics[key]
            });
          }
        }
      }
    }

    if(this.job.state == ProcessingState.Done ||
       this.job.state == ProcessingState.Error) {
      this.updateTimer.unsubscribe();
    }
  }

  ngOnDestroy(): void {
    this.updateTimer.unsubscribe();
  }
}
