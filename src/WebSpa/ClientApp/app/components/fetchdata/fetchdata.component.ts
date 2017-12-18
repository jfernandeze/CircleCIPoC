import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {

    values: Array<string> = [];

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        
        http.get<Array<string>>('http://localhost:5000/api/value').subscribe(result => {
            this.values = result;
        });

    }
}


