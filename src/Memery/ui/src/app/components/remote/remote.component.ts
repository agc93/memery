import { ImageService } from './../../services/image.service';
import { MatSnackBar, MatHorizontalStepper } from '@angular/material';
import { ImageRef } from './../../services/imageRef';
import { Component, Inject, Input, Output, EventEmitter, ViewChild } from '@angular/core';

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

	@ViewChild(MatHorizontalStepper) stepper: MatHorizontalStepper;

    constructor(
		private imageService: ImageService,
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
		this.imageService
			.addImage(this.name, this.url, this.getFilename(this.name))
			.subscribe(resp => this.handleResponse(resp));
    }

    handleResponse(response: ImageRef) {
        var value = response;
        console.debug(`response: ${value}`);
        this.response = value;
        this._snackBar.open('Image uploaded!', 'Dismiss', { duration: 2000 });
        this.onUpload.next();
        this.isLoading = false;
        // this.url = '';
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

    reset(): void {
        this.name = '';
        this.url = '';
		this.isLoading = false;
		this.response = undefined;
		this.stepper.selectedIndex = 0;
    }
}