import { StorageService } from './../../services/storage.service';
import { MatSnackBar, MatSnackBarRef, MatDialog, MatDialogRef } from '@angular/material';
import { Component, Inject, Input } from '@angular/core';
import { VersionService } from '../../services/version.service';

@Component({
    selector: 'changelog',
    templateUrl: './changelog.component.pug',
	styleUrls: ['./changelog.component.styl']
})
export class ChangelogComponent  {
    // displayed: MatSnackBarRef;
    constructor(
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
        private storage: StorageService,
        private version: VersionService
    ) {
        var latest = version.getVersion();
        if (storage.getValue('lastVersion', '0.1') != latest) {
            let bar = this.snackBar.open(`Memery has been updated to ${latest}. Check out what's changed?`, "Changes", { duration: 8000});
            bar.onAction().subscribe(() => {
                // storage.setValue('lastVersion', latest);
                this.dialog.open(ChangelogSheet);
            });
            bar.afterDismissed().subscribe(() => {
                storage.setValue('lastVersion', latest);
            });
        }
    }
 }

@Component({
    selector: 'changelog-sheet',
	templateUrl: './changelog-sheet.component.pug',
	styles: ['button { float: right; }']
  })
  export class ChangelogSheet {
	changes: string[];
	// constructor(private bottomSheetRef: MatBottomSheetRef<ChangelogSheet>) {}
	constructor(private version: VersionService, private matDialog: MatDialogRef<ChangelogSheet>) {
		this.changes = version.getChanges();
	}

    openLink(event: MouseEvent): void {
    //   this.bottomSheetRef.dismiss();
      event.preventDefault();
	}

	close() {
		this.matDialog.close();
	}
  }