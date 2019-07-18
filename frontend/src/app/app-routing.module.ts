import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { 
    path: 'job', 
    loadChildren: () => import('./job/job.module').then(mod => mod.JobModule)
  },
  { 
    path: 'project', 
    loadChildren: () => import('./project/project.module').then(mod => mod.ProjectModule)
  },
  { 
    path: 'worker', 
    loadChildren: () => import('./worker/worker.module').then(mod => mod.WorkerModule)
  },
  { path: '', redirectTo: '/job', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class RoutingModule { }
