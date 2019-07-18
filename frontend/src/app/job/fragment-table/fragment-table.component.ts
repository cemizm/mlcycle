import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-fragment-table',
  templateUrl: './fragment-table.component.html',
  styleUrls: ['./fragment-table.component.scss']
})
export class FragmentTableComponent implements OnInit {
  private data:Array<Array<string>>;

  constructor() { }

  @Input()  
  public set content(v : string) {
    let rows = v.split('\n');
    this.data = new Array<Array<string>>();

    let regex = /(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)/g;

    rows.forEach(row => {
      if(row.length == 0)
        return;
        
      let cols = row.match(regex);

      let r = new Array<string>();

      cols.forEach(col => {
        let value = col;
        if(value)
          value = value.replace(/"/g,'');
        r.push(value);
      })

      this.data.push(r);
    });
  }
  
  ngOnInit() {
  }

}
