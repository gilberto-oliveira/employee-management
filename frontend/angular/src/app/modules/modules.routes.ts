import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        children: [
            {
                path: '',
                redirectTo: 'employees',
                pathMatch: 'full',
            },
            {
                path: 'employees',
                loadComponent: () => import('./employees/employees.component').then(m => m.EmployeesComponent),
            }
        ]
    }
];