import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Ticket } from '../../models/ticket.model';
import { TicketService } from '../../services/ticket.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-edit-ticket',
  templateUrl: './edit-ticket.component.html',
  styleUrl: './edit-ticket.component.css'
})
export class EditTicketComponent implements OnInit {
  ticketForm: FormGroup;
  ticketId: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private TicketService: TicketService
  ) {
    this.ticketForm = this.fb.group({
      id: [{ value: '', disabled: true }],
      description: ['', Validators.required],
      status: ['', Validators.required],
      dateCreated: ['', Validators.required],
      dateChanged: ['', Validators.required]
    });

    this.ticketId = this.route.snapshot.paramMap.get('id') || '';
  }

  ngOnInit(): void {
    if (this.ticketId) {
      this.TicketService.getTicketById(this.ticketId).subscribe(
        (ticket: Ticket) => {
          this.ticketForm.patchValue({
            id: ticket.id,
            description: ticket.description,
            status: ticket.status,
            dateCreated: this.formatDate(ticket.dateCreated), 
          dateChanged: this.formatDate(ticket.dateChanged) 
          });
        },
        (error) => {
          console.error('Failed to fetch Ticket', error);
        }
      );
    }
  }

  updateTicket(): void {
    if (this.ticketForm.valid) {
      const updatedTicket = this.ticketForm.getRawValue();
  
      
  
      this.TicketService.updateTicket(this.ticketId, updatedTicket).subscribe(
        () => {
          Swal.fire(
            'Updated!',
            'Your Ticket has been updated successfully.',
            'success'
          );
          this.router.navigate(['/Tickets']);
        },
        (error) => {
          console.error('Failed to update Ticket', error);
          Swal.fire(
            'Error!',
            'Failed to update Ticket. Please try again later.',
            'error'
          );
        }
      );
    }
  }
  
  
  goBack() {
    this.router.navigate(['/Tickets']); 
   }


   // Format date as needed
formatDate(dateString: any): string {
  const date = new Date(dateString);
  const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: '2-digit' };
  const formattedDate = date.toLocaleDateString('en-US', options);
  
  // Split the formatted date into parts and reformat it
  const [month, day, year] = formattedDate.split(' ');
  return `${month}-${day.replace(',', '')}-${year}`; // "May-29-2024"
}
}
