import { NgModule } from '@angular/core';
import { MatButtonModule, MatExpansionModule, MatStepperModule, MatCardModule, MatFormFieldModule, MatInputModule, MatTableModule, MatPaginatorModule, MatSortModule, MatProgressSpinnerModule, MatTooltipModule, MatIconModule, MatSnackBarModule, MatSliderModule, MatSidenavModule, MatListModule, MatDialogModule } from '@angular/material';
import { CdkTableModule } from '@angular/cdk/table';

@NgModule({
    imports: [CdkTableModule, MatButtonModule, MatExpansionModule, MatStepperModule, MatCardModule, MatFormFieldModule, MatInputModule, MatTableModule, MatPaginatorModule, MatSortModule, MatProgressSpinnerModule, MatTooltipModule, MatIconModule, MatSnackBarModule, MatSliderModule, MatSidenavModule, MatListModule, MatDialogModule],
    exports: [CdkTableModule, MatButtonModule, MatExpansionModule, MatStepperModule, MatCardModule, MatFormFieldModule, MatInputModule, MatTableModule, MatPaginatorModule, MatSortModule, MatProgressSpinnerModule, MatTooltipModule, MatIconModule, MatSnackBarModule, MatSliderModule, MatSidenavModule, MatListModule, MatDialogModule]
})
export class MaterialModule { }