import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material.module';
import { ProcessingStatePipe } from './processing-state.pipe';
import { StateIconComponent } from './state-icon/state-icon.component';
import { DiffPipe } from './diff.pipe';
import { ChartsModule } from 'ng2-charts';


@NgModule({
  declarations: [
    ProcessingStatePipe,
    DiffPipe,
    StateIconComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ChartsModule
  ],
  exports: [
    MaterialModule,
    ProcessingStatePipe,
    DiffPipe,
    StateIconComponent,
    ChartsModule
  ]
})
export class SharedModule { }
