import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material.module';
import { ProcessingStatePipe } from './processing-state.pipe';
import { StateIconComponent } from './state-icon/state-icon.component';
import { DiffPipe } from './diff.pipe';



@NgModule({
  declarations: [
    ProcessingStatePipe,
    DiffPipe,
    StateIconComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule
  ],
  exports: [
    MaterialModule,
    ProcessingStatePipe,
    DiffPipe,
    StateIconComponent,
  ]
})
export class SharedModule { }
