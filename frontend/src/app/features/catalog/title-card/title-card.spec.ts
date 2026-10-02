import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CatalogTitleSummary } from '../data-access/catalog-api.models';
import { TitleCard } from './title-card';
import { provideRouter } from '@angular/router';

describe('TitleCard', () => {
  let fixture: ComponentFixture<TitleCard>;

  const title: CatalogTitleSummary = {
    id: '0199f3f4-7c00-7000-8000-000000000101',
    name: 'Interstellar',
    type: 'Movie',
    tags: 'None',
    releaseYear: 2014,
    posterPath: '/poster.jpg',
    externalRating: 8.7,
    externalVoteCount: 42000,
    publicationStatus: 'Published',
    genres: ['Adventure', 'Science Fiction'],
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TitleCard],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(TitleCard);
    fixture.componentRef.setInput('title', title);

    await fixture.whenStable();
  });

  it('should render title details', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('a')?.getAttribute('href')).toBe(
      '/catalog/0199f3f4-7c00-7000-8000-000000000101',
    );
  });
});
