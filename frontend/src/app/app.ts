import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ThemeSwitcher } from './shared/theme-switcher/theme-switcher';

@Component({
  imports: [RouterLink, RouterLinkActive, RouterOutlet, ThemeSwitcher],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {}
