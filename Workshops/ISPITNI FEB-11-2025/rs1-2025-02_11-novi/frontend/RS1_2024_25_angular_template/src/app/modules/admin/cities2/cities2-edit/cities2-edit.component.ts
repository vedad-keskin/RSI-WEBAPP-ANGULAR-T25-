import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  CityUpdateOrInsertEndpointService
} from '../../../../endpoints/city-endpoints/city-update-or-insert-endpoint.service';
import {CityGetByIdEndpointService} from '../../../../endpoints/city-endpoints/city-get-by-id-endpoint.service';

import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  RegionLookupEndpointService,
  RegionLookupResponse
} from '../../../../endpoints/lookup-endpoints/region-lookup-endpoint.service';
import {
  CountryLookupEndpointService,
  CountryLookupResponse
} from '../../../../endpoints/lookup-endpoints/country-lookup-endpoint.service';


@Component({
  selector: 'app-cities2-edit',
  templateUrl: './cities2-edit.component.html',
  styleUrls: ['./cities2-edit.component.css'],
  standalone: false
})
export class Cities2EditComponent implements OnInit {
  cityForm: FormGroup;
  cityId: number;
  countries: CountryLookupResponse[] = [];
  regions: RegionLookupResponse[] = [];
  regionCache: Map<number, RegionLookupResponse[]> = new Map();

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public router: Router,
    private cityGetByIdService: CityGetByIdEndpointService,
    private cityUpdateService: CityUpdateOrInsertEndpointService,
    private countryGetAllService: CountryLookupEndpointService,
    private regionGetAllService: RegionLookupEndpointService
  ) {
    this.cityId = 0;

    this.cityForm = this.fb.group({
      name: ['', [Validators.required, Validators.min(2), Validators.max(10)]],
      countryId: [null, [Validators.required]],
      regionId: [null, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.cityId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.cityId) {
      this.loadCityData();
    }
    this.loadCountries();

    // Listen for country changes and load regions
    this.cityForm.get('countryId')?.valueChanges.subscribe((countryId: number) => {
      this.loadRegionsForCountry(countryId);
    });
  }

  loadCityData(): void {
    this.cityGetByIdService.handleAsync(this.cityId).subscribe({
      next: (city) => {
        this.cityForm.patchValue({
          name: city.name,
          countryId: city.countryId,
          regionId: city.regionId,
        });
        this.loadRegionsForCountry(city.countryId);
      },
      error: (error) => console.error('Error loading city data', error),
    });
  }

  loadCountries(): void {
    this.countryGetAllService.handleAsync().subscribe({
      next: (countries) => (this.countries = countries),
      error: (error) => console.error('Error loading countries', error),
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
        error: (error) => console.error('Error loading regions', error),
      });
    }
  }

  updateCity(): void {
    if (this.cityForm.invalid) return;

    this.cityUpdateService.handleAsync({
      id: this.cityId,
      ...this.cityForm.value,
    }).subscribe({
      next: () => this.router.navigate(['/admin/cities2']),
      error: (error) => console.error('Error updating city', error),
    });
  }
}
