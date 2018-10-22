import { Injectable } from '@angular/core';

@Injectable()
export class VersionService {

    // private _prefix: string = "memery";

    public getVersion(): string {
        return "0.2"
    }

    public getChanges(version?: string): string[] {
        var current = this.getVersion();
        return this._changes.get(version||current) || ["¯\\_(ツ)_/¯"];
    }

    private _changes: Map<string, string[]> = new Map([
        ["0.1", ["Initial release"]],
        ["0.2", [
            "Thumbnail previews! Change the size using the new Preview slider.",
            "Persists user settings a little more reliably",
            "Improvements to backend logic",
            "This changelog 🤔"
        ]]
     ]);
}