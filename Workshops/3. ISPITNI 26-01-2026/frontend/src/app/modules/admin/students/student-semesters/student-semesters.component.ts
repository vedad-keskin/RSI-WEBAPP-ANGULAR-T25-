import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import {
  CityGetAll3EndpointService,
  CityGetAll3Response
} from '../../../../endpoints/city-endpoints/city-get-all3-endpoint.service';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {debounceTime, distinctUntilChanged, filter, Subject} from 'rxjs';
import {CityDeleteEndpointService} from '../../../../endpoints/city-endpoints/city-delete-endpoint.service';
import {Router} from '@angular/router';
import {MatDialog} from '@angular/material/dialog';
import {MyDialogConfirmComponent} from '../../../shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import {
  SemesterGetAllEndpointService,
  SemesterGetAllResponse
} from '../../../../endpoints/semester-endpoints/semester-get-all-endpoint.service';
import {SemesterDeleteEndpointService} from '../../../../endpoints/semester-endpoints/semester-delete-endpoint.service';
import {
  SemesterRestoreEndpointService
} from '../../../../endpoints/semester-endpoints/semester-restore-endpoint.service';
import {map, tap} from 'rxjs/operators';

@Component({
  selector: 'app-student-semesters',
  standalone: false,

  templateUrl: './student-semesters.component.html',
  styleUrl: './student-semesters.component.css'
})
export class StudentSemestersComponent implements OnInit, AfterViewInit  {

  noviSemestar() {

    this.router.navigate(['/admin/students', 1, 'semesters' , 'new']);
  }

  displayedColumns: string[] = ['academicYearDescription', 'studyYear', 'enrollmentDate', 'isRenewal','tuitionFee','actions'];
  dataSource: MatTableDataSource<SemesterGetAllResponse> = new MatTableDataSource<SemesterGetAllResponse>();

  semesters: SemesterGetAllResponse[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private searchSubject: Subject<string> = new Subject();
  status = 'active';

  constructor(
    private semesterGetService: SemesterGetAllEndpointService,
    private semesterDeleteService: SemesterDeleteEndpointService,
    private semesterRestoreService:SemesterRestoreEndpointService,
    private router: Router,
    private dialog: MatDialog
  ) {
  }

  ngOnInit(): void {
    this.initSearchListener();
    this.fetchSemesters();
  }

  initSearchListener(): void {
    this.searchSubject.pipe(
      debounceTime(300), // Vrijeme čekanja (300ms)
      distinctUntilChanged(), // Emittuje samo ako je vrijednost promijenjena,
      map(q => q.length > 3 ? q : ''),
      map(q => q.trim().toLowerCase()),
      tap(q => console.log(q))

    ).subscribe((filterValue) => {
      this.fetchSemesters(filterValue, this.paginator.pageIndex + 1, this.paginator.pageSize);
    });
  }

  ngAfterViewInit(): void {
    this.paginator.page.subscribe(() => {
      const filterValue = this.dataSource.filter || '';
      this.fetchSemesters(filterValue, this.paginator.pageIndex + 1, this.paginator.pageSize);
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.searchSubject.next(filterValue); // Prosljeđuje vrijednost Subject-u
  }

  fetchSemesters(filter: string = '', page: number = 1, pageSize: number = 5): void {
    this.semesterGetService.handleAsync(1, {
      q: filter,
      pageNumber: page,
      pageSize: pageSize,
      status: this.status
    })

      .subscribe({
      next: (data) => {
        this.dataSource = new MatTableDataSource<SemesterGetAllResponse>(data.dataItems);
        this.paginator.length = data.totalCount; // Postavljanje ukupnog broja stavki
      },
      error: (err) => {
        console.error('Error fetching semesters:', err);
      },
        complete: () => {

        console.log(this.dataSource.data.length)

        }
    });
  }

  editCity(id: number): void {
  }


  deleteSemester(id: number): void {

    this.semesterDeleteService.handleAsync(id).subscribe({
      next: () => {
        console.log(`City with ID ${id} deleted successfully`);
        this.fetchSemesters();


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
        this.deleteSemester(id);
      } else {
        console.log('Korisnik je otkazao brisanje');
      }
    });
  }


  openMyConfirmDialogForRestore(id:number) {

    const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
      width: '350px',
      data: {
        title: 'Potvrda vraćanja',
        message: 'Da li ste sigurni da želite vratiti ovu stavku?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Korisnik je potvrdio vraćanje');
        // Pozovite servis ili izvršite logiku za brisanje
        this.restoreSemester(id);
      } else {
        console.log('Korisnik je otkazao vraćanje');
      }
    });

  }

  private restoreSemester(id: number) {


    this.semesterRestoreService.handleAsync(id).subscribe({
      next: () => {
        console.log(`Semester with ID ${id} restored successfully`);
        this.fetchSemesters();

      },
      error: (err) => console.error('Error restoring city:', err)
    });

  }
}
