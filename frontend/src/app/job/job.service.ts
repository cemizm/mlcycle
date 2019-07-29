import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Job, Fragment } from './job.models';

@Injectable({
  providedIn: 'root'
})
export class JobService {

  private collection = environment.apiUrl + "/jobs/";

  constructor(private http:HttpClient) { }

  getAll(): Observable<Array<Job>>
  {
    return this.http.get<Array<Job>>(this.collection);
  }

  getById(id: string):Observable<Job>
  {
    return this.http.get<Job>(this.collection + id);
  }

  trigger(id: string):Observable<Job>
  {
    return this.http.post<Job>(this.collection + 'project/' + id + '/trigger', null);
  }
}
