import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ProjectService } from '../project.service';
import { Project } from '../project.models';
import { JobService } from 'src/app/job/job.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort, MatExpansionPanel } from '@angular/material';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.scss']
})
export class ProjectListComponent implements OnInit {
  private projects:MatTableDataSource<Project>;

  @ViewChild(MatSort, {static: true})
  sort: MatSort;

  @ViewChild("panel", {static:true})
  panel: MatExpansionPanel;

  @ViewChild("fNew", {static:true})
  fNew: ElementRef;

  private selected:Project;

  private formNew:FormGroup;
  private formEdit:FormGroup;

  private cols: string[] = ['name', 'actions'];

  constructor(private service:ProjectService,
              private jobService:JobService,
              private router: Router,
              private fb: FormBuilder) {

    this.formNew = this.createForm(null)
  }

  ngOnInit() {
    this.updateList();
  }

  private createForm(value:Project):FormGroup {
    return this.fb.group({
      name: [value ? value.name : '', Validators.required],
      gitRepository: [value ? value.gitRepository : '', Validators.required]
    });
  }

  private updateList() {
    this.service.getAll().subscribe(data => {
      this.projects = new MatTableDataSource<Project>(data)
      this.projects.sort = this.sort;
    });
  }

  create(form) {
    let value:Project = this.formNew.value;
    this.formNew.disable();
    this.service.add(value).subscribe((project) => {
      this.updateList();
      this.formNew.enable()
      this.fNew.nativeElement.reset();
      this.panel.close()
    })
  }

  select(project:Project) {
    this.formEdit = this.createForm(project)
    this.selected = this.selected == project ? null : project;
  }

  update() {
    if(!this.selected)
      return;

    let value:Project = this.formEdit.value;

    this.formEdit.disable();
    this.service.update(this.selected.id, value).subscribe(()=>{
      this.updateList();
      this.select(null)
    })
  }

  trigger(event:Event, project:Project) {
    event.stopPropagation();

    this.jobService.trigger(project.id).subscribe((job) => {
      this.router.navigate(['/job', job.id])
    })
  }

  delete(project:Project) {
    if(this.selected == project)
      this.selected = null;

    this.service.delete(project.id).subscribe(() => {
      this.updateList();
    })
  }

}
