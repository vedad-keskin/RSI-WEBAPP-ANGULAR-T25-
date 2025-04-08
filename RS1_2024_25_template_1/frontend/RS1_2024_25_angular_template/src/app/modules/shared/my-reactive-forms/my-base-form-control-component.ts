import {ControlContainer, FormControl, FormGroup, ValidationErrors} from '@angular/forms';
import {Directive} from '@angular/core';

@Directive() // Dodaje Angular dekorator
export abstract class MyBaseFormControlComponent {
  // Input za naziv kontrole
  public myControlName!: string;
  public customMessages: Record<string, string> = {};

  constructor(protected controlContainer: ControlContainer) {
  }

  // Dobijanje formGroup-a iz ControlContainer
  get formGroup(): FormGroup {
    return this.controlContainer.control as FormGroup;
  }

  // Dobijanje formControl-a na osnovu naziva
  get formControl(): FormControl {
    return this.formGroup.get(this.myControlName) as FormControl;
  }

  // Generisanje lista grešaka
  getErrorKeys(errors: ValidationErrors | null): string[] {
    return errors ? Object.keys(errors) : [];
  }

  // Generisanje poruka grešaka
  getErrorMessage(errorKey: string, errorValue: any): string {
    if (this.customMessages[errorKey]) {
      return this.customMessages[errorKey];
    }

    const dynamicMessages: { [key: string]: (errorValue: any) => string } = {
      required: () => 'This field is required.',
      min: (value: any) => `Minimum ${value.requiredLength} characters required. You entered ${value.actualLength}.`,
      max: (value: any) => `Maximum ${value.requiredLength} characters allowed. You entered ${value.actualLength}.`,
      pattern: () => 'Invalid format.',
    };

    if (dynamicMessages[errorKey]) {
      return dynamicMessages[errorKey](errorValue);
    }

    return `Validation error: ${errorKey}`;
  }

  // Pomoćna metoda za generisanje ID-a kontrole
  protected getControlName(): string {
    return Object.keys(this.formGroup.controls).find(
      key => this.formGroup.get(key) === this.formControl
    ) || '';
  }
}
