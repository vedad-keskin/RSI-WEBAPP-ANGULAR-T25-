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
import {ActivatedRoute, Router} from '@angular/router';
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
export class StudentSemestersComponent implements OnInit, AfterViewInit {

  noviSemestar() {
    alert('Ovo je ZADATAK B');
  }

  displayedColumns: string[] = ['academicYear', 'studyYear', 'enrollmentDate', 'isRenewal','tuitionFee','actions'];
  dataSource: MatTableDataSource<SemesterGetAllResponse> = new MatTableDataSource<SemesterGetAllResponse>();

  semesters: SemesterGetAllResponse[] = [];

  studentId:number = 0;
  status:string = "active";

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private searchSubject: Subject<string> = new Subject();

  constructor(
    private semesterGetAllService:SemesterGetAllEndpointService,
    private semesterDeleteService:SemesterDeleteEndpointService,
    private semesterRestoreService:SemesterRestoreEndpointService,
    private router: Router,
    private dialog: MatDialog,
    private route: ActivatedRoute,
  ) {
  }

  ngOnInit(): void {

    this.studentId = Number(this.route.snapshot.paramMap.get('id'));

    this.initSearchListener();
    this.fetchSemesters();
  }

  initSearchListener(): void {
    this.searchSubject.pipe(
      debounceTime(300), // Vrijeme čekanja (300ms)
      distinctUntilChanged(), // Emittuje samo ako je vrijednost promijenjena,
      map(q => q.toLowerCase() ),
      map(q => q.length > 3 ? q : ""),
      tap(q => console.log('Q parametar je: ',q)),
      // tap(q => console.log('Broj zapisa je: ',this.dataSource.data.length),)


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
    this.semesterGetAllService.handleAsync(this.studentId,
      {
        q: filter,
        pageNumber: page,
        pageSize: pageSize,
        status: this.status,
      }
    ).subscribe({
      next: (data) => {
        this.dataSource = new MatTableDataSource<SemesterGetAllResponse>(data.dataItems);
        this.paginator.length = data.totalCount; // Postavljanje ukupnog broja stavki
      },
      error: (err) => {
        console.error('Error fetching semesters:', err);
      },
      complete: () => {

        console.log('Broj zapisa je: ',this.dataSource.data.length);

      }
    });
  }

  // editCity(id: number): void {
  //   this.router.navigate(['/admin/cities3/edit', id]);
  // }

  deleteSemester(id: number): void {

    this.semesterDeleteService.handleAsync(id).subscribe({
      next: () => {

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

        this.fetchSemesters();


      },
      error: (err) => console.error('Error deleting city:', err)
    });
  }
}
