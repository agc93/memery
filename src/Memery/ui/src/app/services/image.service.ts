import { ImageRef, IndexResponse } from './imageRef';
import { Inject, Injectable } from '@angular/core';
// import { Http, Response, Headers } from "@angular/http";
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class ImageService {
    private headers: HttpHeaders;
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private originUrl: string
    ) {
        this.headers = new HttpHeaders();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

    public getAllImages(): Observable<ImageRef[]> {
        console.debug(`Fetching images from ${this.originUrl}`);
		return this.http.get<IndexResponse>(`${this.originUrl}images`)
			.pipe(
				map(k => {
					var ks = Object.keys(k);
					return ks.map(i => k[i]);
				})
			);
    }

    public deleteImage(id: string): Observable<Object> {
        console.debug('calling deleteImage() from service!');
        return this.http.delete(`/images/${id}`)
    }
}