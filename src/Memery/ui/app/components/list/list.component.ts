import { ImageRef } from './../../services/imageRef';
import { ImageService } from './../../services/image.service';
import { IndexDataSource } from './../../services/index.datasource';
import { Http } from '@angular/http';
import { Component, Inject, ViewChild, ElementRef, OnInit } from '@angular/core';
import {DataSource} from '@angular/cdk/collections';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/observable/fromEvent';
import { Observable } from 'rxjs/Observable';
import { MatPaginator, MatSort, MatSnackBar } from '@angular/material';

@Component({
    selector: 'image-list',
    templateUrl: './list.component.pug',
    styleUrls: ['./list.component.styl']
})
export class ListComponent implements OnInit {
    service: ImageService;
    dataSource: IndexDataSource;
    displayedColumns = ['id', 'name', 'location', 'delete'];

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(
        private _http: Http,
        @Inject('BASE_URL') private originUrl: string,
        private _snackBar: MatSnackBar
    ) { }

    ngOnInit() {
        this.service = new ImageService(this._http, this.originUrl)
        this.dataSource = new IndexDataSource(this.service, this.paginator, this.sort);
    }

    updateFilter(filter: string): void {
        console.debug(`updating filter! (to ${filter || 'empty'})`);
        this.dataSource.filter = filter;
    }

    delete(id: string) {
        this.service.deleteImage(id)
            .subscribe(
                resp => {
                    this.dataSource._filterChange.next(this.dataSource.filter);
                    this._snackBar.open('Image successfully deleted!', 'Dismiss', { duration: 3000 });
                },
                err => {
                    console.warn(`err: ${JSON.stringify(err)}`);
                    this._snackBar.open('There was an error deleting the image! Is it already gone?', 'Oh no!', { duration: 6000 });
                }
            );
    }
}