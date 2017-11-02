import {MATERIAL_COMPATIBILITY_MODE} from '@angular/material';
import { NgModule } from '@angular/core';

@NgModule({
 providers: [
   {provide: MATERIAL_COMPATIBILITY_MODE, useValue: true},
   // ...
 ],
})
export class CompatibilityModule { }