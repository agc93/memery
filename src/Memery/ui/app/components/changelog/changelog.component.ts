import { StorageService } from './../../services/storage.service';
import { MatSnackBar, MatSnackBarRef, MatDialog } from '@angular/material';
import { Component, Inject, Input } from '@angular/core';
import { VersionService } from '../../services/version.service';
// import { ChangelogDialog } from './changelog-dialog.component';

@Component({
    selector: 'changelog',
    templateUrl: './changelog.component.pug',
    styleUrls: ['./changelog.component.styl']
})
export class ChangelogComponent  {
    // displayed: MatSnackBarRef;
    constructor(
        private snackBar: MatSnackBar,
        // private dialog: MatDialog,
        private storage: StorageService,
        private version: VersionService
    ) { 
        var latest = version.getVersion();
        if (storage.getValue('lastVersion', '0.1') != latest) {
            let bar = this.snackBar.open(`Memery has been updated to ${latest}. Check out what's changed?`, "Changes", { duration: 5000});
            bar.onAction().subscribe(() => {
                // storage.setValue('lastVersion', latest);
                // this.dialog.open(ChangelogDialog);
            });
            bar.afterDismissed().subscribe(() => {
                storage.setValue('lastVersion', latest);
            });
        }
    }
 }