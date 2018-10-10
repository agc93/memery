import { MatSnackBar } from '@angular/material';
import { ImageRef } from './../../services/imageRef';
import { Component, Inject, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
    selector: 'upload-remote',
    templateUrl: './remote.component.pug',
    styleUrls: ['./remote.component.styl']
})
export class UploadRemoteComponent {
    _url: string = '';
    name: string = '';
    response: ImageRef | undefined;
    isLoading: boolean;

    @Output() onUpload = new EventEmitter();

    constructor(
        private _http: HttpClient,
        @Inject('BASE_URL') private originUrl: string,
        private _snackBar: MatSnackBar
    ) { }

    get url(): string {
        return this._url;
    }

    set url(value: string) {
        this._url = value;
        this.name = this.getFilename(value);
        this.response = undefined;
    }

    get imageSrc(): string {
        return this.response
            ? this.response.id
                ? `/${this.response.id}`
                : ''
            : ''
    }

    get imageName(): string {
        return this.response
        ? this.response.name
            ? `/${this.response.name}`
            : ''
        : ''
    }

    get fullImageLink(): string {
        return (this.response && this.response.id) ? `${this.originUrl}${this.response.id}` : '';
    }

    upload(): void {
        this.isLoading = true;
        let params: HttpParams = new HttpParams();
        params.set('url', this.url);
        var url = `${this.originUrl}images`;
        if (this.name == this.getFilename(this.url)) {
            // POST upload
            this._http.post<ImageRef>(url, null, { params: params })
                .subscribe(resp => this.handleResponse(resp));
        } else {
            // named PUT upload
            url = `${url}/${encodeURIComponent(this.name)}`
            this._http.put<ImageRef>(url, null, { params: params })
                .subscribe(resp => this.handleResponse(resp));
        }
    }

    handleResponse(response: ImageRef) {
        // var value = response.json();
        var value = response;
        console.debug(`response: ${value}`);
        this.response = value;
        this._snackBar.open('Image uploaded!', 'Dismiss', { duration: 2000 });
        this.onUpload.next();
        this.isLoading = false;
        this.url = '';
    }

    private getFilename(url: string) {
        if (url) {
            var frag = url.split('/').pop()
            if (frag) {
                return frag.split('#')[0].split('?')[0];
            }
        }
        return ''
    }

    private reset(): void {
        this.name = '';
        this.url = '';
        this.onUpload.next();
        this.isLoading = false;
    }
}