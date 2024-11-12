import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Ticket } from '../../models/ticket.model';
import { Router } from '@angular/router';
import { TicketService } from '../../services/ticket.service';
@Component({
  selector: 'app-ticket-detail',
  templateUrl: './ticket-detail.component.html',
  styleUrl: './ticket-detail.component.css'
})
export class TicketDetailComponent implements OnInit {

  ticket: Ticket | null = null;

  constructor(
    private route: ActivatedRoute,
    private TicketService: TicketService,
    private router:Router
    
  ) {}

  ngOnInit(): void {
    const ticketeId = this.route.snapshot.paramMap.get('id');
    if (ticketeId) {
      this.TicketService.getTicketById(ticketeId).subscribe(
        (ticket: Ticket) => {
          this.ticket = ticket;
        },
        (error) => {
          console.error('Failed to fetch tickete', error);
        }
      );
    }
  }
  scrollToTop() {
    this.router.navigate(['/ticketes/']);
  }

  goBack() {
    this.router.navigate(['/Tickets']);  
  }
  
  formatDate(dateString: any): string {
    const date = new Date(dateString);
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: '2-digit' };
    const formattedDate = date.toLocaleDateString('en-US', options);
    const [month, day, year] = formattedDate.split(' ');
    return `${month}-${day.replace(',', '')}-${year}`;
    }
}
