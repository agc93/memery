import { Component, Inject, Input } from '@angular/core';
import { VersionService } from '../../services/version.service';

@Component({
    selector: 'changelog',
    templateUrl: './changelog.component.pug',
    styleUrls: ['./changelog.component.styl']
})
export class ChangelogComponent  {
    constructor(
        private versionService: VersionService
    ) { }

    showChanges() {

    }
 }