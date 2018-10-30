import { ImageRef } from './imageRef';
import { ImageService } from './image.service';
import { DataSource } from '@angular/cdk/collections';
import { BehaviorSubject , Observable, pipe, from } from 'rxjs';
import { merge, map, startWith, switchMap } from 'rxjs/operators';
import { MatPaginator, MatSort } from '@angular/material';

/**
 * DEPRECATED.
 * This class is intended to serve as a stand-in for the MatTableDataSource
 * Currently, it looksl ike the stream does not emit correctly, so fallback to MatTableDataSource is in place.
 */
export class IndexDataSource extends DataSource<ImageRef> {
    _filterChange = new BehaviorSubject('');
    isLoadingResults = false;
    get filter(): string { return this._filterChange.value; }
    set filter(filter: string) { this._filterChange.next(filter); }

    resultsLength = 0;

    constructor(
        private _imageService: ImageService,
        private _paginator: MatPaginator,
        private _sort: MatSort
    ) {
        super();
    }

    /** Connect function called by the table to retrieve one stream containing the data to render. */
    connect(): Observable<ImageRef[]> {
        console.debug(`Connecting datasource stream!`);
        const displayDataChanges = [
            this._sort.sortChange,
            this._paginator.page,
            this._filterChange];

		this._sort.sortChange.subscribe(() => this._paginator.pageIndex = 0);
		return from(displayDataChanges).pipe(
        // return pipe(
			// merge(...displayDataChanges),
            // startWith(null),
            switchMap(() => {
                console.debug(`invoking ImageService!`);
                this.isLoadingResults = true;
                return this._imageService.getAllImages()
            }),
            map((img, idx) => {
                console.debug(`filtering ${img.length} items with ${this.filter || 'no filter'}`)
                return img.filter(i => i.name.toLowerCase().indexOf(this.filter.toLowerCase()) != -1);
            }),
            map(data => {
                this.resultsLength = data.length;
                console.debug(`slicing ${data.length} items into page ${this._paginator.pageIndex} of ${this._paginator.pageSize} items`);
                const start = this._paginator.pageIndex * this._paginator.pageSize;
                return data.splice(start, this._paginator.pageSize)
            }),
            map(data => {
                // Flip flag to show that loading has finished.
                // this.resultsLength = data.length;
                console.debug(`length: ${data.length}/${this.resultsLength}`);
                this.isLoadingResults = false;
                // console.log(`items: ${JSON.stringify(data)}`);
                return data;
			})
		);
    }

    disconnect() { }
}