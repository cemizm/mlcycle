import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProjectRoutingModule } from './project-routing.module';
import { ProjectListComponent } from './project-list/project-list.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [ProjectListComponent],
  imports: [
    CommonModule,
    SharedModule,
    ProjectRoutingModule
  ]
})
export class ProjectModule { }
