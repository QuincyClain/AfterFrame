import { Component, inject, signal } from '@angular/core';
import { ThemeName, ThemeService } from '../../core/theme/theme.service';

@Component({
  selector: 'app-theme-switcher',
  templateUrl: './theme-switcher.html',
  styleUrl: './theme-switcher.css',
})
export class ThemeSwitcher {
  protected readonly themeService = inject(ThemeService);
  protected readonly isOpen = signal(false);

  protected toggle(): void {
    this.isOpen.update((isOpen) => !isOpen);
  }

  protected selectTheme(theme: ThemeName): void {
    this.themeService.setTheme(theme);
    this.isOpen.set(false);
  }
}
