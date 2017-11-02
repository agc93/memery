import { ImageRef, IndexResponse } from './imageRef';
import { Inject } from '@angular/core';
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

export class ImageService {
    private headers: Headers;
    constructor(
        private http: Http,
        private originUrl: string
    ) {
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

    public getAllImages(): Observable<ImageRef[]> {
        console.debug(`Fetching images from ${this.originUrl}`);
        return this.http.get(`${this.originUrl}images`)
            .map((response: Response) => response.json() as IndexResponse)
            .map(k => {
                var ks = Object.keys(k);
                return ks.map(i => k[i]);
            });
    }

    public deleteImage(id: string): Observable<Response> {
        console.debug('calling deleteImage() from service!');
        return this.http.delete(`/images/${id}`)
    }
}