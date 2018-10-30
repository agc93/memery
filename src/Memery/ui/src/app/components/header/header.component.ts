import { Component } from '@angular/core';

@Component({
    selector: 'header',
    templateUrl: './header.component.pug',
    styleUrls: ['./header.component.styl']
})
export class HeaderComponent {
    summary: string = 'Get it? Like a bakery for memes!'
}