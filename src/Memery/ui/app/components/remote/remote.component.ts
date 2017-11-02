import { Http, Response, URLSearchParams } from '@angular/http';
import { ImageRef } from './../../services/imageRef';
import { Component, Inject, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';

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
        private _http: Http,
        @Inject('BASE_URL') private originUrl: string
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
        let params: URLSearchParams = new URLSearchParams();
        params.set('url', this.url);
        var url = `${this.originUrl}images`;
        if (this.name == this.getFilename(this.url)) {
            // POST upload
            this._http.post(url, null, { search: params })
                .subscribe(resp => this.handleResponse(resp));
        } else {
            // named PUT upload
            url = `${url}/${encodeURIComponent(this.name)}`
            this._http.put(url, null, { search: params })
                .subscribe(resp => this.handleResponse(resp));
        }
    }

    handleResponse(response: Response) {
        var value = response.json();
        console.debug(`response: ${value}`);
        this.response = value;
        this.onUpload.next();
        this.isLoading = false;
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
}