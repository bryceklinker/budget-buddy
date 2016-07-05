import 'reflect-metadata';
import { bootstrap } from '@angular/platform-browser-dynamic';
import { HTTP_PROVIDERS } from '@angular/http';
import { ROUTER_DIRECTIVES } from '@angular/router'; 

import { AppComponent } from './app.component';
import { APP_ROUTER_PROVIDERS } from './app.routes';
import { ConfigService } from './shared/config.service';

bootstrap(AppComponent, [
    HTTP_PROVIDERS,
    ROUTER_DIRECTIVES,
    APP_ROUTER_PROVIDERS,
    ConfigService
]);