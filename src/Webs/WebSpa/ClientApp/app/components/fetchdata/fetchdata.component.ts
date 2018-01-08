import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent implements OnInit {

    values: Array<string> = [];
    newValue: string = "";
    //testany: any;

    constructor(private http: HttpClient) {
                
    }

    ngOnInit(): void {
        this.http.get<Array<string>>('http://localhost:5000/api/value').subscribe(result => {
            this.values = result;
        });
    }


    create() {

        console.log("create");

        this.http.post<void>('http://localhost:5000/api/value/' + this.newValue, null)
            .subscribe(result => {
                console.log(result);

                this.ngOnInit();
            }
        );
    }
}


