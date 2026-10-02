import { DOCUMENT } from '@angular/common';
import { effect, inject, Injectable, signal } from '@angular/core';

export type ThemeName = 'midnight' | 'velvet' | 'indigo';

export interface ThemeOption {
  readonly id: ThemeName;
  readonly label: string;
}

const DEFAULT_THEME: ThemeName = 'midnight';
const STORAGE_KEY = 'afterframe-theme';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly document = inject(DOCUMENT);

  readonly themes: readonly ThemeOption[] = [
    { id: 'midnight', label: 'Midnight' },
    { id: 'velvet', label: 'Velvet' },
    { id: 'indigo', label: 'Indigo' },
  ];

  readonly activeTheme = signal<ThemeName>(this.readStoredTheme());

  constructor() {
    effect(() => {
      const theme = this.activeTheme();
      this.document.documentElement.dataset['theme'] = theme;

      try {
        this.document.defaultView?.localStorage.setItem(STORAGE_KEY, theme);
      } catch {
        // The theme still works when browser storage is unavailable.
      }
    });
  }

  setTheme(theme: ThemeName): void {
    this.activeTheme.set(theme);
  }

  private readStoredTheme(): ThemeName {
    try {
      const storedTheme = this.document.defaultView?.localStorage.getItem(STORAGE_KEY);

      if (storedTheme && this.isThemeName(storedTheme)) {
        return storedTheme;
      }
    } catch {
      // Fall back to the default theme when browser storage is unavailable.
    }

    return DEFAULT_THEME;
  }

  private isThemeName(value: string): value is ThemeName {
    return this.themes.some((theme) => theme.id === value);
  }
}
