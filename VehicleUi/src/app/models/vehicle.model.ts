export interface Vehicle {
     id?: number;
     make: string;
     model: string;
     year: number;
   }

   export interface Vehicle {
  id?: number;
  make: string;
  model: string;
  year: number;
}

// Validation constants
export const VEHICLE_VALIDATION = {
  MAKE_MIN_LENGTH: 2,
  MAKE_MAX_LENGTH: 50,
  MODEL_MIN_LENGTH: 2,
  MODEL_MAX_LENGTH: 50,
  YEAR_MIN: 2000,
  YEAR_MAX: 2100
};