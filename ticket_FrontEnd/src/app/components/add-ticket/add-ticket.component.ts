import { Component } from '@angular/core';
import { Ticket } from '../../models/ticket.model';
import { TicketService } from '../../services/ticket.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-ticket',
  templateUrl: './add-ticket.component.html',
  styleUrl: './add-ticket.component.css'
})
export class AddTicketComponent {

  ticket: Ticket = {
    description: '',
    status: ''
   
  };
  tickets: any[]=[];

  constructor(private ticketService: TicketService,private router: Router
  ) {}


  addticket(): void {
    this.ticketService.addTicket(this.ticket).subscribe({
      next: () => {
        Swal.fire({
          title: 'Saved!',
          text: 'Your ticket has been saved successfully.',
          icon: 'success',
          confirmButtonText: 'OK'
        });
        this.router.navigate(['']);
        this.loadtickets();
      },
      error: (err) => {
        Swal.fire({
          title: 'Error!',
          text: 'There was an error saving your ticket. Please try again.',
          icon: 'error',
          confirmButtonText: 'OK'
        });
      }
    });
  }
  

  loadtickets(): void {
    this.ticketService.getTickets().subscribe(
      (data) => {
        this.tickets = data;
      },
      (error) => {
        console.error('Failed to fetch tickets', error);
      }
    );
  }
  goBack() {
    this.router.navigate(['/Tickets']);  
  }
}
