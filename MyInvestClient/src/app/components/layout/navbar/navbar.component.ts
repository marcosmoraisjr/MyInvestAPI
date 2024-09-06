import { Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  @ViewChild('hamburger', { static: false }) hamburger!: ElementRef;
  @ViewChild('nav', { static: false }) nav!: ElementRef;

  ngAfterViewInit(): void {
    this.hamburger.nativeElement.addEventListener('click', () => {
      this.nav.nativeElement.classList.toggle('active');
    });
  }
}
