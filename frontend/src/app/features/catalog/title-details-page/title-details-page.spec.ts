import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TitleDetailsPage } from './title-details-page';
import { provideRouter } from '@angular/router';

describe('TitleDetailsPage', () => {
  let fixture: ComponentFixture<TitleDetailsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TitleDetailsPage],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(TitleDetailsPage);
    fixture.componentRef.setInput('id', '1');

    await fixture.whenStable();
  });

  it('should render the selected title', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('h2')?.textContent).toContain('Interstellar');
    expect(element.textContent).toContain('Movie');
    expect(element.textContent).toContain('2014');
    expect(element.querySelector('a')?.getAttribute('href')).toBe('/catalog');
  });
});
