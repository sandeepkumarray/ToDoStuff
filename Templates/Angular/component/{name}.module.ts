import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { IconModule } from '@coreui/icons-angular';
import { [TABLE_NAME]Component } from './[TABLE_NAME_SMALL].component';
import { View[TABLE_NAME]Component } from './view-[TABLE_NAME_SMALL]/view-[TABLE_NAME_SMALL].component';
import { Edit[TABLE_NAME]Component } from './edit-[TABLE_NAME_SMALL]/edit-[TABLE_NAME_SMALL].component';
import { [TABLE_NAME]RoutingModule } from './[TABLE_NAME_SMALL]-routing.module';

import {
  AccordionModule,
  BadgeModule,
  BreadcrumbModule,
  ButtonModule,
  CardModule,
  CarouselModule,
  CollapseModule,
  DropdownModule,
  FormModule,
  GridModule,
  ListGroupModule,
  NavModule,
  PaginationModule,
  PlaceholderModule,
  PopoverModule,
  ProgressModule,
  SharedModule,
  SpinnerModule,
  TableModule,
  TabsModule,
  TooltipModule,
  UtilitiesModule,
  ModalModule
} from '@coreui/angular';
import { QuillModule } from 'ngx-quill';

@NgModule({
  declarations: [
    [TABLE_NAME]Component,
    View[TABLE_NAME]Component,
    Edit[TABLE_NAME]Component,
  ],
  imports: [
    CommonModule,
    [TABLE_NAME]RoutingModule,AccordionModule,
    BadgeModule,
    BreadcrumbModule,
    ButtonModule,
    CardModule,
    CollapseModule,
    GridModule,
    UtilitiesModule,
    SharedModule,
    ListGroupModule,
    IconModule,
    ListGroupModule,
    PlaceholderModule,
    ProgressModule,
    SpinnerModule,
    TabsModule,
    NavModule,
    TooltipModule,
    CarouselModule,
    FormModule,
    ReactiveFormsModule,
    DropdownModule,
    PaginationModule,
    PopoverModule,
    TableModule,
    FormsModule,
    ModalModule,
    QuillModule.forRoot()
  ]
})
export class [TABLE_NAME]Module {

}
