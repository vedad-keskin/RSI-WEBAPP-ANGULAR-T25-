import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  CityUpdateOrInsertEndpointService
} from '../../../../endpoints/city-endpoints/city-update-or-insert-endpoint.service';
import {CityGetByIdEndpointService} from '../../../../endpoints/city-endpoints/city-get-by-id-endpoint.service';
import {
  CountryGetAllEndpointService,
  CountryGetAllResponse
} from '../../../../endpoints/country-endpoints/country-get-all-endpoint.service';
import {
  RegionGetAllEndpointService,
  RegionGetAllResponse
} from '../../../../endpoints/region-endpoints/region-get-all-endpoint.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-cities3-edit',
  templateUrl: './cities3-edit.component.html',
  styleUrls: ['./cities3-edit.component.css'],
  standalone: false,
})
export class Cities3EditComponent implements OnInit {
  cityForm: FormGroup;
  cityId: number;
  countries: CountryGetAllResponse[] = [];
  regions: RegionGetAllResponse[] = [];
  regionCache: Map<number, RegionGetAllResponse[]> = new Map(); // Cache for regions by countryId

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public router: Router,
    private cityGetByIdService: CityGetByIdEndpointService,
    private cityUpdateService: CityUpdateOrInsertEndpointService,
    private countryGetAllService: CountryGetAllEndpointService,
    private regionGetAllService: RegionGetAllEndpointService
  ) {
    this.cityId = 0;

    this.cityForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(10)]],
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

    // Watch for country changes to load regions dynamically
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
        this.loadRegionsForCountry(city.countryId); // Preload regions for the selected country
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
      // Use cached regions
      this.regions = this.regionCache.get(countryId)!;
    } else {
      // Fetch regions if not in cache
      this.regionGetAllService.handleAsync({countryId}).subscribe({
        next: (regions) => {
          this.regions = regions;
          this.regionCache.set(countryId, regions); // Cache the regions
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
      next: () => this.router.navigate(['/admin/cities3']),
      error: (error) => console.error('Error updating city', error),
    });
  }
}
