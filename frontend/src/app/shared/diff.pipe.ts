import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'diff'
})
export class DiffPipe implements PipeTransform {

  transform(start: Date, end: Date): number {
    if(start == null)
      return null;

    var created = new Date(start);
    var finished = !end ? new Date() : new Date(end);
    
    return ((finished.getTime() - created.getTime()) / 1000) | 0;
  }

}
