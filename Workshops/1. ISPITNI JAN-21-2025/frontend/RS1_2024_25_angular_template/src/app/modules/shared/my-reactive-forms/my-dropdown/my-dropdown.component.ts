import {Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {MyBaseFormControlComponent} from '../my-base-form-control-component';
import {ControlContainer} from '@angular/forms';

@Component({
  selector: 'app-my-dropdown',
  standalone: false,

  templateUrl: './my-dropdown.component.html',
  styleUrl: './my-dropdown.component.css'
})
export class MyDropdownComponent extends MyBaseFormControlComponent implements OnInit, OnChanges {
  @Input() myLabel!: string; // Labela za dropdown
  @Input() myId: string = ''; // ID za dropdown (koristi se u <label> for atributu)
  @Input() myPlaceholder: string = ''; // Placeholder tekst
  @Input() options: { id: number | string; name: string }[] = []; // Opcije za dropdown
  @Input() defaultValue: number | string | null = null; // Podrazumijevana vrijednost

  @Input() override customMessages: Record<string, string> = {}; // Dodano!
  @Input() override myControlName: string = "";

  constructor(protected override controlContainer: ControlContainer) {
    super(controlContainer);
  }

  ngOnInit(): void {
    // Ako nije eksplicitno postavljen id, koristi naziv kontrola
    if (!this.myId && this.formControl) {
      this.myId = this.getControlName();
    }
  }

  compareFn(option: any, value: any): boolean {
    return option == value;// Loose comparison
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['options'] || changes['defaultValue']) {
      // Provjera da li su opcije učitane i postavljanje podrazumijevane vrijednosti
      if (this.options.length > 0 && this.defaultValue !== null && !this.formControl.value) {
        this.formControl.patchValue(this.defaultValue, {emitEvent: true});
      }
    }
  }

}

