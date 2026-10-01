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
    fixture.componentRef.setInput('id', '4');

    await fixture.whenStable();
  });

  it('should render the selected title', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('h2')?.textContent).toContain('Spirited Away');
    expect(element.textContent).toContain('Movie');
    expect(element.textContent).toContain('2001');
    expect(element.querySelector('a')?.getAttribute('href')).toBe('/catalog');
  });

  it('should add a title to the library', async () => {
    const element = fixture.nativeElement as HTMLElement;
    const button = element.querySelector('button');

    expect(button?.textContent).toContain('Add to library');

    button?.click();
    await fixture.whenStable();

    expect(element.textContent).toContain('In your library');
    expect(element.textContent).toContain('planned');
  });
});
