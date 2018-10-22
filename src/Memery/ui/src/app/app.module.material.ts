import { NgModule } from '@angular/core';
import { MatButtonModule, MatExpansionModule, MatStepperModule, MatCardModule, MatFormFieldModule, MatInputModule, MatTableModule, MatPaginatorModule, MatSortModule, MatProgressSpinnerModule, MatTooltipModule, MatIconModule, MatSnackBarModule, MatSliderModule, MatSidenavModule } from '@angular/material';
import { CdkTableModule } from '@angular/cdk/table';

@NgModule({
    imports: [CdkTableModule, MatButtonModule, MatExpansionModule, MatStepperModule, MatCardModule, MatFormFieldModule, MatInputModule, MatTableModule, MatPaginatorModule, MatSortModule, MatProgressSpinnerModule, MatTooltipModule, MatIconModule, MatSnackBarModule, MatSliderModule, MatSidenavModule],
    exports: [CdkTableModule, MatButtonModule, MatExpansionModule, MatStepperModule, MatCardModule, MatFormFieldModule, MatInputModule, MatTableModule, MatPaginatorModule, MatSortModule, MatProgressSpinnerModule, MatTooltipModule, MatIconModule, MatSnackBarModule, MatSliderModule, MatSidenavModule]
})
export class MaterialModule { }