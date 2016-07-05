import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ConfigService } from '../../shared/config.service';
import { Budget } from '../models/budget';

@Injectable()
export class BudgetService {
    constructor(private http: Http,
        private configService: ConfigService) {
        
    }

    getBudget(): Observable<Budget> {
        return this.configService.getConfig()
            .flatMap(c => this.http.get(`${c.budgetApiUrl}/budgets/current`))
            .map<Budget>(res => res.json());
    }
}