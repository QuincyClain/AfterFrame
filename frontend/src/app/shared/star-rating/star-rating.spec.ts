import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StarRating } from './star-rating';

describe('StarRating', () => {
  let fixture: ComponentFixture<StarRating>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StarRating],
    }).compileComponents();

    fixture = TestBed.createComponent(StarRating);
    fixture.componentRef.setInput('rating', 8.5);

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should expose an accessible rating label', () => {
    const element = fixture.nativeElement as HTMLElement;
    const rating = element.querySelector('[role="img"]');

    expect(rating?.getAttribute('aria-label')).toBe('8.5 out of 10 stars');
    expect(element.textContent).toContain('8.5 / 10');
  });
});
