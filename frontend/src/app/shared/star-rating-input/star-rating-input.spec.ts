import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StarRatingInput } from './star-rating-input';

describe('StarRatingInput', () => {
  let fixture: ComponentFixture<StarRatingInput>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StarRatingInput],
    }).compileComponents();

    fixture = TestBed.createComponent(StarRatingInput);
    fixture.componentRef.setInput('rating', 8.5);

    await fixture.whenStable();
  });

  it('should render the selected rating', () => {
    const element = fixture.nativeElement as HTMLElement;
    const slider = element.querySelector('[role="slider"]');

    expect(slider?.getAttribute('aria-valuenow')).toBe('8.5');
    expect(slider?.getAttribute('aria-valuetext')).toBe('8.5 out of 10 stars');
    expect(element.textContent).toContain('8.5 / 10');
  });

  it('should increase the rating by half a star with the keyboard', () => {
    let selectedRating: number | undefined;

    fixture.componentInstance.ratingChange.subscribe((rating) => {
      selectedRating = rating;
    });

    const slider = fixture.nativeElement.querySelector('[role="slider"]') as HTMLElement;

    slider.dispatchEvent(
      new KeyboardEvent('keydown', {
        key: 'ArrowRight',
        bubbles: true,
      }),
    );

    expect(selectedRating).toBe(9);
  });
});
