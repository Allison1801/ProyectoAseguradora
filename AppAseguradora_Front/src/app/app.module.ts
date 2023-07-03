import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConsultadoraSegurosModule } from './ConsultadoraSeguros/consultadora-seguros.module';
import { SharedModule } from './Shared/shared.module';
import { CustomFormsModule } from 'ng2-validation';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ConsultadoraSegurosModule,
    SharedModule,
    HttpClientModule,
    CustomFormsModule,
    ReactiveFormsModule 
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
