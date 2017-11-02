import { CompatibilityModule } from './app.module.compat';
import { MaterialModule } from './app.module.material';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { FlexLayoutModule } from "@angular/flex-layout";

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HeaderComponent } from "./components/header/header.component";
import { HomeComponent } from './components/home/home.component';
import { UploadComponent } from "./components/upload/upload.component";
import { ListComponent } from "./components/list/list.component";
import { ImageLinkComponent } from "./components/imagelink/imagelink.component";
import { UploadRemoteComponent } from "./components/remote/remote.component";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        ImageLinkComponent,
        HomeComponent,
        HeaderComponent,
        UploadComponent,
        ListComponent,
        UploadRemoteComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        MaterialModule,
        CompatibilityModule,
        FlexLayoutModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
})
export class AppModuleShared {
}
