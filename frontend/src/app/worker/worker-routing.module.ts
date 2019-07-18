import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkerListComponent } from './worker-list/worker-list.component';


const routes: Routes = [
  { path:'', component: WorkerListComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkerRoutingModule { }
