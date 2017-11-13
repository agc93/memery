import { MatSnackBar } from '@angular/material';
import { BaseComponent } from './../base.component';
import { Http, Response } from '@angular/http';
import { Component, Inject, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormArray, FormGroupDirective, FormControl, FormBuilder, Validators } from "@angular/forms";
import { DOCUMENT } from '@angular/common';

@Component({
    selector: 'upload',
    templateUrl: './upload.component.pug',
    styleUrls: ['./upload.component.styl']
})
export class UploadComponent extends BaseComponent {
    formArray: FormArray;
    private file: any;
    private response: any;
    code: string;
    isLoading: boolean;

    @Output() onUpload = new EventEmitter();

    constructor(
        private fb: FormBuilder,
        private _http: Http,
        @Inject('BASE_URL') private originUrl: string,
        private _snackBar: MatSnackBar
    ) {
        super();
        this.createForms();
    }

    get imageSrc(): string {
        return this.code ? `/${this.code}` : '';
    }

    get imageName(): string {
        return this.code ? `/${super.getFormArrayValue(this.formArray, 2, 'name')}` : '';
    }

    get fullImageLink(): string {
        return this.code ? `${this.originUrl}${this.code}` : '';
    }

    onFileChange($event: any) {
        console.debug('change event triggered! Updating form values.')
        let file = $event.target.files[0];
        this.file = file;
        var ac = this.formArray.get([0]);
        super.setFormArrayValue(this.formArray, 0, 'file', file.name);
        try {
            super.setFormArrayValue(this.formArray, 1, 'name', file.name);
        } catch (error) {
            console.debug(`err: ${error}`);
        }
        console.debug(`Updated formGroup to ${file.name}`);
    }

    upload(): void {
        console.debug('calling upload() from form!');
        this.isLoading = true;
        let _formData = new FormData();
        let name = super.getFormArrayValue(this.formArray, 1, 'name');
        console.debug(`retrieved name: ${name}`);
        _formData.append("file", this.file);
        _formData.append("name", name);
        let body = _formData;
        if (name == this.file.name) {
            this._http.post("/images", body)
                .subscribe(resp => this.handleResponse(resp));
        } else {
            this._http.put(`/images/${encodeURIComponent(name)}`, body)
                .subscribe(resp => this.handleResponse(resp));
        }
    }

    handleResponse(response: Response) {
        console.log(response.json());
        var value = response.json();
        console.log(`response: ${value}`);
        var fg = this.formArray.get([2]) as FormGroup;
        if (fg) {
            super.setFormValue(fg, 'code', value.id);
            super.setFormValue(fg, 'name', value.name);
        }
        this.code = value.id;
        console.debug(`uploaded response parsed`);
        this.onUpload.next();
        this._snackBar.open('Image uploaded!', 'Dismiss', { duration: 2000 });
        this.isLoading = false;
    }

    createForms(): any {
        /* this.formGroup = this.fb.group({
            name: this.fb.array(['', Validators.required])
        }) */
        this.formArray = this.fb.array([
            this.fb.group({file: ['', Validators.required]}),
            this.fb.group({name: ['', Validators.required]}),
            this.fb.group({code: ['', Validators.required], name: ['', Validators.required]}),
            this.fb.group({})
        ]);
    }

    reset(): void {
        this.createForms();
        this.file = null;
        this.response = null;
        this.code = '';
    }
}