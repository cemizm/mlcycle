import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FragmentService {
  private collection = environment.apiUrl + "/fragments/";

  constructor(private http:HttpClient) { }

  getUrl(id:string):string {
    return this.collection + id;
  }

  getContent(id:string):Observable<string> {
    return this.http.get(this.getUrl(id), { responseType: 'text' });
  }
}
