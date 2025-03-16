import { Injectable } from '@angular/core';
import { Employee, EmployeePaginatedCollection, EmployeeCreateRequest, Operation } from './models';
import apiClient from './api-client';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {

  constructor() {}

  listEmployees(search: string | null, page: number, limit: number): Promise<EmployeePaginatedCollection> {
    return apiClient
      .get('/employees', { params: { search, page, limit } })
      .then((response) => response.data)
      .catch((error) => {
        throw error;
      });
  }

  createEmployee(employeeData: EmployeeCreateRequest): Promise<void> {
    return apiClient
      .post('/employees', employeeData)
      .then(() => {})
      .catch((error) => {
        throw error;
      });
  }

  getEmployee(id: string): Promise<Employee> {
    return apiClient
      .get(`/employees/${id}`)
      .then((response) => response.data)
      .catch((error) => {
        throw error;
      });
  }

  patchEmployee(id: string, operations: Operation[]): Promise<void> {
    return apiClient
      .patch(`/employees/${id}`, operations)
      .then(() => {})
      .catch((error) => {
        throw error;
      });
  }

  deleteEmployee(id: string): Promise<void> {
    return apiClient
      .delete(`/employees/${id}`)
      .then(() => {})
      .catch((error) => {
        throw error;
      });
  }
}
