import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Project } from './project.models';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  private collection = environment.apiUrl + "/projects/";

  constructor(private http:HttpClient) { }

  getAll(): Observable<Array<Project>> {
    return this.http.get<Array<Project>>(this.collection);
  }

  getById(id: string):Observable<Project> {
    return this.http.get<Project>(this.collection + id);
  }

  add(project:Project):Observable<Project> {
    return this.http.post<Project>(this.collection, project)
  }

  update(id:string, project:Project):Observable<Project> {
    return this.http.put<Project>(this.collection + id, project);
  }

  delete(id:string):Observable<Object>{
    return this.http.delete(this.collection + id)
  }

}
