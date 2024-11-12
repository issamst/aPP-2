import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Ticket } from '../../models/ticket.model';
import { TicketService } from '../../services/ticket.service';
import { catchError } from 'rxjs/operators'; 
import { throwError } from 'rxjs'; 
import Swal from 'sweetalert2';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent implements OnInit {
  tickets: Ticket[] = [];
  errorMessage: string = '';

  constructor(private ticketService: TicketService, private router: Router) {}

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.ticketService.getTickets().subscribe(
      (data) => {
        this.tickets = data;
      },
      (error) => {
        console.error('Failed to fetch Tickets', error);
      }
    );
  }

  deleteTicket(id: string): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'You will not be able to recover this Ticket!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it',
    }).then((result) => {
      if (result.isConfirmed) {
        this.ticketService.deleteTicket(id)
          .pipe(
            catchError(error => {
              console.error('Error deleting Ticket:', error);
              this.errorMessage = 'Failed to delete Ticket. Please try again later.';
              return throwError('Failed to delete Ticket.');
            })
          ).subscribe(() => {
            this.loadTickets();
            Swal.fire(
              'Deleted!',
              'Your Ticket has been deleted.',
              'success'
            );
          });
      }
    });
  }

  editTicket(id: string): void {
    this.router.navigate(['/Tickets/edit', id]);
  }

  showTicket(id: string): void {
    this.router.navigate(['/Tickets/', id]);
  }

  addNewTicket(): void {
    this.router.navigate(['/Tickets/add']);
  }

  formatDate(dateString: any): string {
    const date = new Date(dateString);
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: '2-digit' };
    const formattedDate = date.toLocaleDateString('en-US', options);
    const [month, day, year] = formattedDate.split(' ');
    return `${month}-${day.replace(',', '')}-${year}`;
  }
}
