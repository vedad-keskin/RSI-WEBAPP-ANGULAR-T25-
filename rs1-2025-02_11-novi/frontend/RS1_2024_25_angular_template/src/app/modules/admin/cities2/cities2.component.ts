import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {
  CityGetAll1EndpointService,
  CityGetAll1Response
} from '../../../endpoints/city-endpoints/city-get-all1-endpoint.service';
import {CityDeleteEndpointService} from '../../../endpoints/city-endpoints/city-delete-endpoint.service';
import {MyDialogConfirmComponent} from '../../shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import {MatDialog} from '@angular/material/dialog';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';

@Component({
  selector: 'app-cities2',
  templateUrl: './cities2.component.html',
  styleUrls: ['./cities2.component.css'],
  standalone: false
})
export class Cities2Component implements OnInit {
  //ovdje je koristeno Angular Reactive forms
  displayedColumns: string[] = ['name', 'regionName', 'countryName', 'actions'];
  dataSource: MatTableDataSource<CityGetAll1Response> = new MatTableDataSource<CityGetAll1Response>();

  cities: CityGetAll1Response[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  constructor(
    private cityGetService: CityGetAll1EndpointService,
    private cityDeleteService: CityDeleteEndpointService,
    private router: Router,
    private dialog: MatDialog
  ) {
  }

  ngOnInit(): void {
    this.fetchCities();
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;

    if (this.dataSource) {
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }
  }

  fetchCities(): void {
    this.cityGetService.handleAsync().subscribe({
      next: (data) => {
        this.cities = data;
        this.dataSource = new MatTableDataSource<CityGetAll1Response>(this.cities);

        // Ponovno postavljanje paginatora i sorta
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (err) => console.error('Error fetching cities1:', err)
    });
  }

  editCity(id: number): void {
    this.router.navigate(['/admin/cities2/edit', id]);
  }

  deleteCity(id: number): void {

    this.cityDeleteService.handleAsync(id).subscribe({
      next: () => {
        console.log(`City with ID ${id} deleted successfully`);
        this.cities = this.cities.filter(city => city.id !== id); // Uklanjanje iz lokalne liste
      },
      error: (err) => console.error('Error deleting city:', err)
    });
  }

  openMyConfirmDialog(id: number) {
    const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
      width: '350px',
      data: {
        title: 'Potvrda brisanja',
        message: 'Da li ste sigurni da želite obrisati ovu stavku?'
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
