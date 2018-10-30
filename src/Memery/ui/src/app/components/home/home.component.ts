import { ListComponent } from './../list/list.component';
import { Component, Inject, ViewChild } from '@angular/core';
// import { FormGroup, FormArray, FormGroupDirective, FormControl, FormBuilder, Validators } from "@angular/forms";

@Component({
    selector: 'home',
    templateUrl: './home.component.pug',
    styleUrls: ['./home.component.styl']
})
export class HomeComponent {

    constructor(
        // private fb: FormBuilder,
        @Inject('BASE_URL') private originUrl: string
    ) { }

    @ViewChild(ListComponent) list: ListComponent;

    refresh() {
		this.list.refresh(true);
    }
}
