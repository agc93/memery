import { AbstractControl, FormGroup, FormArray } from "@angular/forms";

export class BaseComponent {
    setControlValue(ac: AbstractControl | null, value: any): void {
        if (ac) {
            ac.setValue(value);
        }
    }

    getControlValue(ac: AbstractControl | null): any | null {
        if (ac) {
            return ac.value;
        }
    }

    setFormValue(fg: FormGroup, key: string, value: any): void {
        var ac = fg.get(key);
        this.setControlValue(ac, value);
    }

    getFormValue(fg: FormGroup, key: string): any | null {
        var ac = fg.get(key);
        return this.getControlValue(ac);
    }

    setFormArrayValue(fa: FormArray, form: number, key: string, value: any) {
        var ac = fa.get([form]);
        var fg = ac as FormGroup;
        this.setFormValue(fg, key, value);
    }

    getFormArrayValue(fa: FormArray, form: number, key: string): any | null {
        var ac = fa.get([form]);
        var fg = ac as FormGroup;
        return this.getFormValue(fg, key);
    }
}