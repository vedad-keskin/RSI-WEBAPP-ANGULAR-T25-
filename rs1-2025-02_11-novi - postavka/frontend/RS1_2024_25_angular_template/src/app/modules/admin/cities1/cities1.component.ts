import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {
  CityGetAll1EndpointService,
  CityGetAll1Response
} from '../../../endpoints/city-endpoints/city-get-all1-endpoint.service';
import {CityDeleteEndpointService} from '../../../endpoints/city-endpoints/city-delete-endpoint.service';
import {MatDialog} from '@angular/material/dialog';
import {MyDialogConfirmComponent} from '../../shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import {MySnackbarHelperService} from '../../shared/snackbars/my-snackbar-helper.service';
import {MyDialogSimpleComponent} from '../../shared/dialogs/my-dialog-simple/my-dialog-simple.component';
import {tap} from 'rxjs/operators';

@Component({
  selector: 'app-cities1',
  templateUrl: './cities1.component.html',
  styleUrls: ['./cities1.component.css'],
  standalone: false
})
export class Cities1Component implements OnInit {
  //ovdje je koristeno NgModel
  cities: CityGetAll1Response[] = [];

  constructor(
    private cityGetService: CityGetAll1EndpointService,
    private cityDeleteService: CityDeleteEndpointService,
    private router: Router,
    private dialog: MatDialog,
    private snackbarHelper: MySnackbarHelperService
  ) {
  }

  ngOnInit(): void {
    this.fetchCities();
  }

  fetchCities(): void {
    this.cityGetService.handleAsync()
      .pipe(tap(x => console.log("fetched: " + x.length)))
      .subscribe({
        next: (data) => (this.cities = data),
        error: (err) => console.error('Error fetching cities1:', err)
      });
  }

  editCity(id: number): void {
    this.router.navigate(['/admin/cities1/edit', id]);
  }

  undoDeleteCity(id: number): void {

    const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
      width: '350px',
      data: {
        title: 'Vraćanje grada',
        message: `Jeste li sigurni da želite vratiti grad sa ID-jem ${id}?`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dialog.open(MyDialogSimpleComponent, {
          width: '350px',
          data: {
            title: 'Greška',
            message: 'Ova opcija nije implementirana?'
          }
        });
      }
    });
  }

  deleteCity(id: number): void {

    this.cityDeleteService.handleAsync(id).subscribe({
      next: () => {
        console.log(`City with ID ${id} deleted successfully`);
        this.cities = this.cities.filter(city => city.id !== id); // Uklanjanje iz lokalne liste
        this.snackbarHelper.showMessageWithAction(
          'Obrisan city',
          'Undo',
          () => {
            this.undoDeleteCity(id);
          }
        );
      },
      error: (err) => console.error('Error deleting city:', err)
    });
  }

  openMyConfirmDialog(id: number) {
    const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
      width: '350px',
      data: {
        title: 'Potvrda brisanja',
        message: 'Da li ste sigurni da želite obrisati ovu stavku?',
        confirmButtonText: 'Obriši',
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Korisnik je potvrdio brisanje');
        // Pozovite servis ili izvršite logiku za brisanje
        this.deleteCity(id);
      } else {
        console.log('Korisnik je otkazao brisanje');
      }
    });
  }
}
