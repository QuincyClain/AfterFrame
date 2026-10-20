import { DOCUMENT } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  HostListener,
  computed,
  inject,
  input,
  output,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-title-trailer-dialog',
  imports: [],
  templateUrl: './title-trailer-dialog.html',
  styleUrl: './title-trailer-dialog.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleTrailerDialog {
  readonly videoKey = input.required<string>();
  readonly title = input.required<string>();
  readonly closed = output<void>();

  private readonly sanitizer = inject(DomSanitizer);
  private readonly document = inject(DOCUMENT);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly embedUrl = computed(() => {
    const key = encodeURIComponent(this.videoKey());

    return this.sanitizer.bypassSecurityTrustResourceUrl(
      `https://www.youtube-nocookie.com/embed/${key}?autoplay=1&rel=0`,
    );
  });

  constructor() {
    const previousOverflow = this.document.body.style.overflow;

    this.document.body.style.overflow = 'hidden';

    this.destroyRef.onDestroy(() => {
      this.document.body.style.overflow = previousOverflow;
    });
  }

  @HostListener('document:keydown.escape')
  protected closeOnEscape(): void {
    this.closed.emit();
  }
}
