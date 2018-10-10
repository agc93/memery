import { StorageService } from "./storage.service";
import { ImageService } from "./image.service";
import { NgModule, ModuleWithProviders } from '@angular/core';
import { VersionService } from "./version.service";

@NgModule({})
export class ServicesModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ServicesModule,
      providers: [ ImageService, StorageService, VersionService ]
    };
  }
}