import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { JobService } from '../job.service';
import { Job } from '../job.models';
import { Label, SingleDataSet } from 'ng2-charts';
import { ChartType, ChartOptions, ChartColor } from 'chart.js';

import * as moment from 'moment';


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
export class JobComponent implements OnInit {
  private job:Job;
  private metrics:Array<Metric>

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

    this.service.getById(id).subscribe(res => {    
      this.job = res;

      this.metrics = new Array<Metric>()
      this.time_data = []
      this.time_labels = [];

      for(let step of this.job.steps)
      {
        if(step.start)
        {
          let start = moment(step.start);
          let end = step.end ? moment(step.end) : moment();

          let time = moment.duration(end.diff(start));

          this.time_data.push(time.asSeconds())
          this.time_labels.push(step.name);
        }

        if(!step.metrics) 
          continue;
          
        for(let key of Object.keys(step.metrics))
        {
          this.metrics.push({
            step: step.name,
            name: key,
            value: step.metrics[key]
          })
        }
      }
    });
  }
}
