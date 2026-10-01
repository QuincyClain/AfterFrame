import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LibraryEntryForm } from './library-entry-form';

describe('LibraryEntryForm', () => {
  let fixture: ComponentFixture<LibraryEntryForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibraryEntryForm],
    }).compileComponents();

    fixture = TestBed.createComponent(LibraryEntryForm);

    fixture.componentRef.setInput('entry', {
      titleId: 1,
      status: 'completed',
      rating: 9.5,
      review: null,
      seriesProgress: null,
    });

    fixture.componentRef.setInput('titleType', 'movie');

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should display the current rating', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.textContent).toContain('9.5 / 10');
  });
});
