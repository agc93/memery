import { Component } from "@angular/core";

@Component({
    selector: 'changelog-sheet',
    templateUrl: 'changelog-sheet.html',
  })
  export class ChangelogSheet {
    // constructor(private bottomSheetRef: MatBottomSheetRef<ChangelogSheet>) {}
  
    openLink(event: MouseEvent): void {
    //   this.bottomSheetRef.dismiss();
      event.preventDefault();
    }
  }