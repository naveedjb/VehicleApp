import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { VehicleService } from './services/vehicle.service';
import { Vehicle, VEHICLE_VALIDATION } from './models/vehicle.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Vehicle Management';
  vehicles: Vehicle[] = [];
  vehicleForm: FormGroup;
  errorMessage: string = '';
  successMessage: string = '';
  
  // Pagination
  currentPage: number = 1;
  pageSize: number = 5;
  totalCount: number = 0;
  totalPages: number = 0;
  
  // Loading states
  isLoading: boolean = false;
  isSubmitting: boolean = false;

  // Expose validation constants to template
  readonly validation = VEHICLE_VALIDATION;

  constructor(
    private vehicleService: VehicleService,
    private fb: FormBuilder,
     private cdr: ChangeDetectorRef 
  ) {
    this.vehicleForm = this.fb.group({
      make: ['', [
        Validators.required,
        Validators.minLength(VEHICLE_VALIDATION.MAKE_MIN_LENGTH),
        Validators.maxLength(VEHICLE_VALIDATION.MAKE_MAX_LENGTH),
        this.alphanumericValidator
      ]],
      model: ['', [
        Validators.required,
        Validators.minLength(VEHICLE_VALIDATION.MODEL_MIN_LENGTH),
        Validators.maxLength(VEHICLE_VALIDATION.MODEL_MAX_LENGTH),
        this.alphanumericValidator
      ]],
      year: [new Date().getFullYear(), [
        Validators.required,
        Validators.min(VEHICLE_VALIDATION.YEAR_MIN),
        Validators.max(VEHICLE_VALIDATION.YEAR_MAX)
      ]]
    });
  }

  ngOnInit(): void {
    this.loadVehicles();
  }

  /**
   * Custom validator: Only alphanumeric, spaces, and hyphens
   * This prevents XSS attempts at input level
   */
  alphanumericValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;
    
    const pattern = /^[a-zA-Z0-9\s\-]+$/;
    return pattern.test(control.value) ? null : { invalidCharacters: true };
  }

  loadVehicles(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.vehicleService.getVehicles(this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.vehicles = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.currentPage = response.pageNumber;
        this.isLoading = false;
         this.cdr.detectChanges();  
      },
      error: (error) => {
        this.errorMessage = 'Error loading vehicles';
        console.error('Error:', error);
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.vehicleForm.invalid) {
      this.errorMessage = 'Please fill all fields correctly';
      this.vehicleForm.markAllAsTouched();
      return;
    }

    if (this.isSubmitting) return;

    this.isSubmitting = true;

    const newVehicle: Vehicle = {
      make: this.vehicleForm.value.make.trim(),
      model: this.vehicleForm.value.model.trim(),
      year: parseInt(this.vehicleForm.value.year, 10)
    };

    this.vehicleService.addVehicle(newVehicle).subscribe({
      next: (vehicle) => {
        this.successMessage = 'Vehicle added successfully!';
        this.loadVehicles();
        
        this.vehicleForm.reset({
          make: '',
          model: '',
          year: new Date().getFullYear()
        });
        
        setTimeout(() => this.successMessage = '', 3000);
        this.isSubmitting = false;
      },
      error: (error) => {
        this.errorMessage = 'Error adding vehicle';
        console.error('Error:', error);
        this.isSubmitting = false;
      }
    });
  }

  // Pagination methods
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadVehicles();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadVehicles();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadVehicles();
    }
  }

  // Form control getters
  get make() {
    return this.vehicleForm.get('make');
  }

  get model() {
    return this.vehicleForm.get('model');
  }

  get year() {
    return this.vehicleForm.get('year');
  }

  // Track by for performance
  trackByVehicleId(index: number, vehicle: Vehicle): number {
    return vehicle.id || index;
  }
}