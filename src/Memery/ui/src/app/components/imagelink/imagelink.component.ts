import { Component, Inject, Input } from '@angular/core';

@Component({
    selector: 'image-link',
    templateUrl: './imagelink.component.pug',
    styleUrls: ['./imagelink.component.styl']
})
export class ImageLinkComponent  {
    constructor(
        @Inject('BASE_URL') private originUrl: string
    ) { }

    displayPreview: boolean = false;
    @Input() text: string;
    @Input() target: string;

    getTarget(): string {
        if (this.target) {
            return this.target;
        }
        return `${this.originUrl}${this.text}`
    }
 }