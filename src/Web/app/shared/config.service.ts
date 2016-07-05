import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';

import { Config } from './config';

@Injectable()
export class ConfigService {
    private config: Config;
    constructor(private http: Http) {

    }

    getConfig(): Observable<Config> {
        if (this.config)
            return Observable.of(this.config);
        
        return this.http.get('config.json')
            .map<Config>(res => {
                this.config = res.json();
                return this.config;
            });
    }
}