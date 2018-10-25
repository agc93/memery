import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable()
export class VersionService {

	// private _prefix: string = "memery";

	public getVersion(): string {
		return environment.VERSION || "😖"
	}

	public getChanges(version?: string): string[] {
		var current = this.getVersion();
		let changes = this._changes.get(version || current) || []; //exact version
		changes = changes.concat(this._changes.get((version || current).split('.').slice(0, -1).join('.'))); //current major version
		return this.isEmpty(changes)
			? ["¯\\_(ツ)_/¯"]
			: changes;
		// return ((changes.length == 0) || (changes.every(x => this.isEmptyOrSpaces(x))))
		// 	? ["¯\\_(ツ)_/¯"]
		// 	: changes;
	}

	private _changes: Map<string, string[]> = new Map(Object.entries(environment.CHANGELOG));
    /* private _changes: Map<string, string[]> = new Map([
        ["0.1", ["Initial release"]],
        ["0.2", [
            "Thumbnail previews! Change the size using the new Preview slider.",
            "Persists user settings a little more reliably",
            "Improvements to backend logic",
            "This changelog 🤔"
        ]]
	 ]); */
	// private isEmptyOrSpaces(str: string): boolean{
	// 	return str === undefined || str === null || str.match(/^\s*$/) !== null;
	// }
	private isEmpty(arr: string[]): boolean {
		return arr === null ||
			arr.length == 0 ||
			arr.every(str => {
				return str === undefined || str === null || str.match(/^\s*$/) !== null;
			});
	}
}