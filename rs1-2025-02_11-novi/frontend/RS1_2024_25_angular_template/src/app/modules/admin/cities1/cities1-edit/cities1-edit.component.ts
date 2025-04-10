import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  CityUpdateOrInsertEndpointService
} from '../../../../endpoints/city-endpoints/city-update-or-insert-endpoint.service';
import {
  CityGetByIdEndpointService,
  CityGetByIdResponse
} from '../../../../endpoints/city-endpoints/city-get-by-id-endpoint.service';
import {
  CountryLookupEndpointService,
  CountryLookupResponse
} from '../../../../endpoints/lookup-endpoints/country-lookup-endpoint.service';
import {
  RegionLookupEndpointService,
  RegionLookupResponse
} from '../../../../endpoints/lookup-endpoints/region-lookup-endpoint.service';
import {MySnackbarHelperService} from '../../../shared/snackbars/my-snackbar-helper.service';

@Component({
  selector: 'app-cities1-edit',
  templateUrl: './cities1-edit.component.html',
  styleUrls: ['./cities1-edit.component.css'],
  standalone: false
})
export class Cities1EditComponent implements OnInit {
  cityId: number;
  city: CityGetByIdResponse = {
    name: '',
    countryId: 0,
    regionId: 0
  };
  countries: CountryLookupResponse[] = [];
  regions: RegionLookupResponse[] = [];
  regionCache: Map<number, RegionLookupResponse[]> = new Map(); // Cache za regije po countryId

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private cityGetByIdService: CityGetByIdEndpointService,
    private cityUpdateService: CityUpdateOrInsertEndpointService,
    private countryGetAllService: CountryLookupEndpointService,
    private regionGetAllService: RegionLookupEndpointService,
    private snackbarHelper: MySnackbarHelperService
  ) {
    this.cityId = 0;
  }

  ngOnInit(): void {
    this.cityId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.cityId) {
      this.loadCityData();
    }
    this.loadCountries();
  }

  loadCityData(): void {
    this.cityGetByIdService.handleAsync(this.cityId).subscribe({
      next: (city) => {
        this.city = city;
        if (this.city.countryId) {
          this.loadRegionsForCountry(this.city.countryId);
        }
      },
      error: (error) => console.error('Error loading city data', error)
    });
  }

  loadCountries(): void {
    this.countryGetAllService.handleAsync().subscribe({
      next: (countries) => {
        this.countries = countries;
        this.countries.unshift({id: 0, name: '-- Select Country --'});
      },
      error: (error) => console.error('Error loading countries', error)
    });
  }

  loadRegionsForCountry(countryId: number): void {
    if (this.regionCache.has(countryId)) {
      this.regions = this.regionCache.get(countryId)!;
    } else {
      this.regionGetAllService.handleAsync({countryId}).subscribe({
        next: (regions) => {
          this.regions = regions;
          this.regionCache.set(countryId, regions);
        },
        error: (error) => console.error('Error loading regions', error)
      });
    }
  }

  updateCity(): void {
    let errors: string[] = [];
    if (this.city.countryId == 0) {
      errors.push("Country is required.");
    }
    if (this.city.regionId == 0) {
      errors.push("Region is required.");
    }
    if (this.city.name.trim().length == 0) {
      errors.push("City name is required.");
    }

    if (errors.length > 0) {
      alert("Errors:\n" + errors.join("\n"));
      return;
    }

    this.cityUpdateService.handleAsync({
      id: this.cityId,
      ...this.city
    }).subscribe({
      next: () => {
        this.snackbarHelper.showMessage('Successfully saved changes');
        this.router.navigate(['/admin/cities1']);
      },
      error: (error) => console.error('Error updating city', error)
    });
  }
}
