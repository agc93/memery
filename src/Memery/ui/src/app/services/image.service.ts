import { ImageRef, IndexResponse } from './imageRef';
import { Inject, Injectable, EventEmitter, Output } from '@angular/core';
// import { Http, Response, Headers } from "@angular/http";
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class ImageService {
	private headers: HttpHeaders;
	// @Output() onUpdate: EventEmitter<ImageRef> = new EventEmitter();

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private originUrl: string
    ) {
        this.headers = new HttpHeaders();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
	}

	public uploadImage(name: string, file: any): Observable<ImageRef> {
		let _formData = new FormData();
		_formData.append("file", file);
        _formData.append("name", name);
		let body = _formData;
		if (name == file.name) {
            return this.http.post<ImageRef>("/images", body);
        } else {
            return this.http.put<ImageRef>(`/images/${encodeURIComponent(name)}`, body);
        }
	}

	public addImage(name: string, url: string, urlName?: string): Observable<ImageRef> {
		let params: HttpParams = new HttpParams().set('url', url);
        // params.set('url', url);
		var url = `${this.originUrl}images`;
		console.log(`Using ${JSON.stringify(params)}`);
        if ((urlName == null) || (name == urlName)) {
            // POST upload
            return this.http.post<ImageRef>(url, null, { params: params });
        } else {
            // named PUT upload
            url = `${url}/${encodeURIComponent(name)}`
            return this.http.put<ImageRef>(url, null, { params: params });
        }
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