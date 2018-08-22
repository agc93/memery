import { Component, Inject, Input } from '@angular/core';

@Component({
    selector: 'image-preview',
    templateUrl: './preview.component.pug',
    styleUrls: ['./preview.component.styl']
})
export class PreviewComponent  {
    constructor(
        @Inject('BASE_URL') private originUrl: string
    ) { }

    @Input() path: string = '';
    @Input() size: number = 50;

    getTarget(): string {
        return `${this.originUrl}${this.path}`
    }

    handleError(event: any) {
        console.debug(event);
        this.size = 0;
    }
 }