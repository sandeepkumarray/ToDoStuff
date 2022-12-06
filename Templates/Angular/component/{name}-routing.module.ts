import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/utility/AuthGuard';
import { [TABLE_NAME]Component } from './[TABLE_NAME_SMALL].component';
import { View[TABLE_NAME]Component } from './view-[TABLE_NAME_SMALL]/view-[TABLE_NAME_SMALL].component';
import { Edit[TABLE_NAME]Component } from './edit-[TABLE_NAME_SMALL]/edit-[TABLE_NAME_SMALL].component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: '[TABLE_NAME_SMALL]',
    },
    children: [
      {
        path: '',
        component: [TABLE_NAME]Component,
        data: {
          title: '[TABLE_NAME_SMALL]',
        },
        canActivate: [AuthGuard]
      },
      {
        path: ':id',
        component: View[TABLE_NAME]Component,
        data: {
          title: 'View [TABLE_NAME_S]',
        },
        canActivate: [AuthGuard]
      },
      {
        path: ':id/edit',
        component: Edit[TABLE_NAME]Component,
        data: {
          title: 'Edit [TABLE_NAME_S]',
        },
        canActivate: [AuthGuard]
      },
    ]
  },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class [TABLE_NAME]RoutingModule {

}
