import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TicketListComponent } from './components/ticket-list/ticket-list.component';
import { AddTicketComponent } from './components/add-ticket/add-ticket.component';
import { TicketDetailComponent } from './components/ticket-detail/ticket-detail.component';
import { EditTicketComponent } from './components/edit-ticket/edit-ticket.component';

const routes: Routes = [
  { path: '', redirectTo: '/Tickets', pathMatch: 'full' },
  { path: 'Tickets', component: TicketListComponent },
  { path: 'Tickets/add', component: AddTicketComponent }, 
  { path: 'Tickets/:id', component: TicketDetailComponent },
  { path: 'Tickets/edit/:id', component: EditTicketComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
