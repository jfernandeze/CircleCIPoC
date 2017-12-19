import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HeaderComponent } from './components/navmenu/header.component';

import { AppRoutingModule,routingComponents } from './app.routing';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HeaderComponent,
        routingComponents
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        AppRoutingModule
    ]
})
export class AppModuleShared {
}
