import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import { Ripple } from 'primeng/ripple';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { FileUpload } from 'primeng/fileupload';
import { Tag } from 'primeng/tag';
import { RadioButton } from 'primeng/radiobutton';
import { Rating } from 'primeng/rating';
import { InputNumber } from 'primeng/inputnumber';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { ImportsModule } from './imports.module';
import { EmployeeService } from './service/employee.service';
import { AuthService } from '../../auth/auth.service';
import { convertEmployeeCreateRequestToUpdateOperations, Employee, EmployeeRole } from './service/models';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';

interface Column {
  field: string;
  header: string;
  customExportHeader?: string;
}

interface ExportColumn {
  title: string;
  dataKey: string;
}

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './employees.component.html',
  providers: [MessageService, ConfirmationService, EmployeeService, DatePipe],
  styles: [
    `:host ::ng-deep .p-dialog .employee-image {
          width: 150px;
          margin: 0 auto 2rem auto;
          display: block;
      }`
  ]
})
export class EmployeesComponent {

  isAuthenticated: boolean = false;

  employeeDialog: boolean = false;

  employees!: Employee[];

  employee!: Employee;

  submitted: boolean = false;

  employeeRoles!: any[];

  @ViewChild('dt') dt!: Table;

  cols!: Column[];

  loggedEmployee: any;

  first?: number = 1;

  rows?: number = 10;

  searchTerm: string | null = null;

  loading: boolean = false;

  timeout: any;

  passwordRepeated: string = '';

  phones: string[] = [];

  dateOfBirth: Date | null = null;

  constructor(
    private authService: AuthService,
    private employeeService: EmployeeService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router
  ) {
    this.isAuthenticated = authService.isAuthenticated();
    if (!this.isAuthenticated) {
      this.router.navigate(['/auth/login']);
    }
    this.loggedEmployee = authService.getDecodedToken();

    this.employeeRoles = [
      { label: EmployeeRole.Employee.toString(), value: EmployeeRole.Employee },
      { label: EmployeeRole.Leader.toString(), value: EmployeeRole.Leader },
      { label: EmployeeRole.Director.toString(), value: EmployeeRole.Director }
    ];

    this.cols = [
      { field: 'firstname', header: 'FirstName' },
      { field: 'lastname', header: 'LastName' },
      { field: 'email', header: 'Email' },
      { field: 'docNumber', header: 'Doc' },
      { field: 'phones', header: 'Phones' },
      { field: 'manager', header: 'Manager' },
      { field: 'dateOfBirth', header: 'Date of Birth' },
      { field: 'role', header: 'Role' },
    ];
  }

  lazyLoad(event: TableLazyLoadEvent) {
    this.first = event.first! == 0 ? 1 : event.first!;
    this.rows = event.rows!;
    this.loadEmployeeData();
  }

  exportCSV() {
    this.dt.exportCSV();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }

  addPhoneNumber(): void {
    if (this.phones)
      this.phones.push('');
  }

  removePhoneNumber(index: number): void {
    if (this.phones.length > 0) {
      this.phones.splice(index, 1);
    }
  }

  trackByFn(index: number, item: any): number {
    return index;
  }

  loadEmployeeData() {
    this.loading = true;
    this.employeeService.listEmployees(this.searchTerm, this.first ?? 0, this.rows ?? 0)
      .then((data) => {
        this.dt.totalRecords = data.total;
        this.employees = data.items as Employee[];
        this.loading = false;
      }).finally(() => { this.loading = false; });
  }

  onSearchChange() {
    if (this.timeout) {
      clearTimeout(this.timeout);
    }

    this.timeout = setTimeout(() => {
      this.loadEmployeeData();
    }, 500);
  }

  private createEmptyEmployee() {
    return {
      id: '',
      firstName: '',
      lastName: '',
      email: '',
      docNumber: '',
      phones: [],
      managerId: null,
      manager: null,
      password: '',
      dateOfBirth: '',
      role: ''
    };
  }

  openNew() {
    this.employee = this.createEmptyEmployee();
    this.submitted = false;
    this.employeeDialog = true;
    this.passwordRepeated = '';
    this.phones = [];
    this.dateOfBirth = null;
  }

  editEmployee(employee: Employee) {
    this.dateOfBirth = new Date(employee.dateOfBirth);
    this.employee = { ...employee };
    this.passwordRepeated = employee.password!;
    this.employeeDialog = true;
    this.phones = employee.phones;
  }

  hideDialog() {
    this.employeeDialog = false;
    this.submitted = false;
  }

  deleteEmployee(employee: Employee) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + employee.firstName + ' ' + employee.lastName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.employeeService.deleteEmployee(employee.id)
          .then(() => {
            this.loadEmployeeData();
            this.messageService.add({
              severity: 'success',
              summary: 'Successful',
              detail: 'Employee Deleted',
              life: 3000
            });
          })
          .catch(() => {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: 'An unexpected error occurred.',
                life: 5000
              });
          });
      }
    });
  }

  saveEmployee() {
    this.submitted = true;

    if (this.dateOfBirth)
      this.employee.dateOfBirth = this.dateOfBirth.toISOString();
    else {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Setup the Date Of Birth',
        life: 5000
      });
      return;
    }

    if (this.phones.length > 0)
      this.employee.phones = this.phones;

    if (this.employee.password != this.passwordRepeated) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'The passwords are not the same',
        life: 5000
      });
      return;
    }

    this.employee.managerId = this.loggedEmployee.id;

    if (this.employee.id == '') {
      this.employeeService.createEmployee(this.employee)
        .then(() => {
          this.loadEmployeeData();
          this.employeeDialog = false;
          this.messageService.add({
            severity: 'success',
            summary: 'Successful',
            detail: 'Employee Created',
            life: 3000
          });
          this.employee = this.createEmptyEmployee();
          this.dateOfBirth = null;
          this.passwordRepeated = '';
        })
        .catch(error => {
          if (error && error.response && error.response.data && error.response.data.errors && 
            error.response.data.errors?.operations?.length > 0
          ) {
            error.response.data.errors.operations.forEach((op: string) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Validation Error',
                detail: op,
                life: 5000
              });
            });
          }
          if (error && error.response && error.response.data && error.response.data.errors.length > 0) {
            error.response.data.errors.forEach((err: { propertyName: any; errorMessage: any; }) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Validation Error',
                detail: `${err.propertyName}: ${err.errorMessage}`,
                life: 5000 // Display toast for 5 seconds
              });
            });
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'An unexpected error occurred.',
              life: 5000
            });
          }
        });
    } else {
      const operations = convertEmployeeCreateRequestToUpdateOperations(this.employee);
      console.log(this.employee.id);
      this.employeeService.patchEmployee(this.employee.id, operations)
        .then(() => {
          this.loadEmployeeData();
          this.employeeDialog = false;

          this.messageService.add({
            severity: 'success',
            summary: 'Successful',
            detail: 'Employee Updated',
            life: 3000
          });
    
          this.employee = this.createEmptyEmployee();
          this.dateOfBirth = null;
          this.passwordRepeated = '';
        })

    }

  }

}