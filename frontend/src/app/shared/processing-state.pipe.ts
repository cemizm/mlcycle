import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'pstate'
})
export class ProcessingStatePipe implements PipeTransform {

  transform(value: number, ...args: any[]): any {
    if(value == 0)
      return "Erstellt";
    if(value == 1)
      return "In Bearbeitung";
    if(value == 2)
      return "Abgeschlossen";
    if(value == 21)
      return "Eingereiht";
    if(value == 31)
      return "Fehler";
  }

}
