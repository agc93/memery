import { ListComponent } from './../list/list.component';
import { Http, Response } from '@angular/http';
import { Component, Inject, ViewChild } from '@angular/core';
import { FormGroup, FormArray, FormGroupDirective, FormControl, FormBuilder, Validators } from "@angular/forms";
import { DOCUMENT } from '@angular/common';

@Component({
    selector: 'home',
    templateUrl: './home.component.pug',
    styleUrls: ['./home.component.styl']
})
export class HomeComponent {

    constructor(
        private fb: FormBuilder,
        private _http: Http,
        @Inject('BASE_URL') private originUrl: string
    ) {
        // this.createForms();
    }

    @ViewChild(ListComponent) list: ListComponent;

    refresh() {
        this.list.dataSource._filterChange.next(this.list.dataSource.filter);
    }
}
