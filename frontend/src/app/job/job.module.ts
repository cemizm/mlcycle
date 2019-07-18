import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { JobRoutingModule } from './job-routing.module';
import { JobListComponent } from './job-list/job-list.component';
import { HttpClientModule } from '@angular/common/http';

import { SharedModule } from '../shared/shared.module';
import { JobComponent } from './job/job.component';
import { StepComponent } from './step/step.component';
import { FragmentComponent } from './fragment/fragment.component';
import { FragmentTableComponent } from './fragment-table/fragment-table.component';


@NgModule({
  declarations: [
    JobListComponent, 
    JobComponent, 
    StepComponent, 
    FragmentComponent,
    FragmentTableComponent
  ],
  imports: [
    CommonModule,
    JobRoutingModule,
    HttpClientModule,
    
    SharedModule
  ]
})
export class JobModule { }
