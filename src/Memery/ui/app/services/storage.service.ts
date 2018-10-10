import { Inject, Injectable } from '@angular/core';

@Injectable()
export class StorageService {

    // private _prefix: string = "memery";
    
    constructor() {
        console.debug("Initialising local storage service...");
    }

    public getValue(key: string, defaultValue: string | number = ""): string {
        return localStorage.getItem(this.prefix(key)) || defaultValue.toString();
    }

    public setValue(key: string, value: any): void {
        return localStorage.setItem(this.prefix(key), value);
    }

    private prefix(key: string | number): string {
        return `memery_${key}`;
    }

    public clear(key?: string): void {
        if (key == null) {
            localStorage.clear();
        } else {
            localStorage.removeItem(this.prefix(key));
        }
    }
}