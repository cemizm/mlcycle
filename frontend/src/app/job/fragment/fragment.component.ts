import { Component, OnInit, Input } from '@angular/core';
import { Fragment } from 'src/app/job/job.models';
import { environment } from 'src/environments/environment';
import { FragmentService } from '../fragment.service';

@Component({
  selector: 'app-fragment',
  templateUrl: './fragment.component.html',
  styleUrls: ['./fragment.component.scss']
})
export class FragmentComponent implements OnInit {
  private _fragment:Fragment;
  private type:number;
  public content:string;
  public loading:boolean;

  constructor(private service:FragmentService) { }

  get fragment(): Fragment {
    return this._fragment;
  }

  @Input()
  set fragment(fragment:Fragment) {
    this._fragment = fragment;
    this.update();
  }

  ngOnInit() {
    this.update();
  }

  public isActive(type:number):boolean{
    return this.fragment && this.type == type;
  }

  public getUrl():string {
    return this.service.getUrl(this.fragment.id);
  }

  private update() {

    let ext = '';
    if(this.fragment && this.fragment.filename)
    {
      let index = this.fragment.filename.lastIndexOf('.');
      if(index > 0)
        ext = this.fragment.filename.substr(index)
    }

    this.content = null;

    switch (ext) {
      case '.png':
      case '.jpg':
      case '.jpeg':
        this.type = 1;
        break;
      case '.csv':
      case '.log':
      case '.txt':
        this.type = -1;
        this.loading = true;
        this.service.getContent(this.fragment.id).subscribe(content => {
          this.type = ext == ".csv" ? 2 : 3;
          this.content = content;
          this.loading = false;
        });
        break;
      default:
        this.type = 0;
        break;
    }
  }

}
