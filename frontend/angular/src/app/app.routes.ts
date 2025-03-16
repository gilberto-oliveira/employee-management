import { Routes } from '@angular/router';

export const routes: Routes =
    [
        {
            path: '',
            children: [
                {
                    path: '',
                    redirectTo: 'auth',
                    pathMatch: 'full',
                },
                {
                    path: 'auth',
                    loadChildren: () => import('./auth/auth.routes').then(m => m.routes),
                }
            ]
        },
        {
            path: 'modules',
            loadChildren: () => import('./modules/modules.routes').then(m => m.routes),
        },
        {
            path: '**',
            redirectTo: 'auth',
            pathMatch: 'full',
        },
    ];
