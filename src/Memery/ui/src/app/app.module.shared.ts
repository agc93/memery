import { MaterialModule } from './app.module.material';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FlexLayoutModule } from "@angular/flex-layout";
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HeaderComponent } from "./components/header/header.component";
import { HomeComponent } from './components/home/home.component';
import { UploadComponent } from "./components/upload/upload.component";
import { ListComponent } from "./components/list/list.component";
import { ImageLinkComponent } from "./components/imagelink/imagelink.component";
import { UploadRemoteComponent } from "./components/remote/remote.component";
import { PreviewComponent } from "./components/preview/preview.component";
import { ChangelogComponent, ChangelogSheet } from './components/changelog/changelog.component';

import { ServicesModule } from "./services/services.module";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        ImageLinkComponent,
        HomeComponent,
        HeaderComponent,
        UploadComponent,
        ListComponent,
        UploadRemoteComponent,
		PreviewComponent,
		ChangelogSheet,
		ChangelogComponent,
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        MaterialModule,
        // CompatibilityModule,
        FlexLayoutModule,
        ServicesModule.forRoot(),
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: '**', redirectTo: 'home' }
        ])
	],
	entryComponents: [
		ChangelogSheet
	]
})
export class AppModuleShared {
}
