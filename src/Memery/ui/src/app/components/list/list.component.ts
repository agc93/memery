import { ImageService } from './../../services/image.service';
import { IndexDataSource } from './../../services/index.datasource';
import { Component, Inject, ViewChild, ElementRef, OnInit, Output, EventEmitter } from '@angular/core';
import { ImageRef } from '../../services/imageRef';

import { MatPaginator, MatSort, MatSnackBar, MatSliderChange, MatTableDataSource, MatTable } from '@angular/material';
import { StorageService } from '../../services/storage.service';
import { Observable, merge } from 'rxjs';
import { startWith, switchMap, map } from 'rxjs/operators';

@Component({
    selector: 'image-list',
    templateUrl: './list.component.pug',
    styleUrls: ['./list.component.styl']
})
export class ListComponent implements OnInit {
    service: ImageService;
	dataSource: MatTableDataSource<ImageRef> = new MatTableDataSource<ImageRef>();
	// dataSource: DataSource<ImageRef>;
    private displayedColumns = ['id', 'name', 'location', 'delete'];
	previewSize: number = 50;
	dataStream: Observable<ImageRef[]>;

	@Output() onUpdate: EventEmitter<ImageRef> = new EventEmitter();

    @ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	@ViewChild(MatTable) table: MatTable<ImageRef>;

    constructor(
        private _imgService: ImageService,
        private _snackBar: MatSnackBar,
        private _storage: StorageService
    ) { this.service = _imgService}

    onSizeChanged(event: MatSliderChange) {
        console.log(`event value was ${event.value}`);
        this._storage.setValue('previewSize', new Number(event.value));
    }

    ngOnInit() {
		// this.dataStream = this.service.getAllImages();
		// this.dataSource = new IndexDataSource(this.service, this.paginator, this.sort);
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
		this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
		merge(this.sort.sortChange, this.onUpdate)
		.pipe(
			startWith([]),
			switchMap(data => {
				return this.service.getAllImages()
			}),
			map(data => {

				return data;
			})
		).subscribe(resp => {
			this.dataSource.data = resp;
			this.table.renderRows()
		});
        this.previewSize = parseInt(localStorage.getItem('memery_previewSize') || '50');
    }

    getColumns(): string[] {
        return this.displayPreview ? ['thumb'].concat(this.displayedColumns) : this.displayedColumns
    }

    updateFilter(filter: string): void {
        // console.debug(`updating filter! (to ${filter || 'empty'})`);
        this.dataSource.filter = filter;
    }

    get displayPreview():boolean {
        return this.previewSize > 0
    }

    delete(id: string) {
        this.service.deleteImage(id)
            .subscribe(
                resp => {
					this.refresh(true);
                    this._snackBar.open('Image successfully deleted!', 'Dismiss', { duration: 3000 });
                },
                err => {
                    console.warn(`err: ${JSON.stringify(err)}`);
                    this._snackBar.open('There was an error deleting the image! Is it already gone?', 'Oh no!', { duration: 6000 });
                }
            );
	}

	refresh(fetchRemote: boolean = false) {
		if (fetchRemote) {
			this.onUpdate.next();
		}
		this.table.renderRows();
	}
}