import { Component, OnInit } from '@angular/core';
import { JobService } from '../job.service';
import { Job, ProcessingState } from '../job.models';
import { Project } from '../../project/project.models';

import { map } from 'rxjs/operators';

@Component({
  selector: 'app-job-list',
  templateUrl: './job-list.component.html',
  styleUrls: ['./job-list.component.scss']
})
export class JobListComponent implements OnInit {
  private allJobs:Array<Job>;

  private states:Array<any>;
  private projects:Array<Project>;

  private state:ProcessingState;
  private project:Project;

  private jobs:Array<Job>;

  constructor(private jobService:JobService) {
    this.projects = new Array<Project>();

    this.state = null;
    this.project = null;

    this.states = new Array<any>();
    this.states.push({key: ProcessingState.Created, value: "Erstellt"});
    this.states.push({key: ProcessingState.InProgress, value: "In Bearbeitung"});
    this.states.push({key: ProcessingState.Done, value: "Abgeschlossen"});
    this.states.push({key: ProcessingState.Error, value: "Fehler"});
  }

  ngOnInit() {
    this.jobService.getAll().subscribe(res => {
      this.allJobs = res;

      this.allJobs.forEach((j) => {
        if(j.project == null)
          return;

        let project = this.projects.find(p => p.id == j.project.id);
        if(project == null)
          this.projects.push(j.project);
      });

      this.updateJobs();
    })
  }
  updateJobs() {
    let tmp = this.allJobs;

    tmp = tmp.filter((j) => {
      if(this.state != null && j.state != this.state)
        return false;

      if(this.project != null && (j.project == null || j.project.id != this.project.id))
        return false;

      return true;
    });

    this.jobs = tmp.sort((a, b) => {
      if(a.created > b.created)
        return -1;
      if(b.created > a.created)
        return 1;

      return 0;
    });
  }
}
