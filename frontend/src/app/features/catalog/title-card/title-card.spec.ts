import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Title } from '../title';
import { TitleCard } from './title-card';

describe('TitleCard', () => {
  let fixture: ComponentFixture<TitleCard>;

  const title: Title = {
    id: 1,
    title: 'Interstellar',
    type: 'movie',
    tags: [],
    releaseYear: 2014,
    description: 'A team of explorers travels through space.',
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TitleCard],
    }).compileComponents();

    fixture = TestBed.createComponent(TitleCard);
    fixture.componentRef.setInput('title', title);

    await fixture.whenStable();
  });

  it('should render title details', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('h3')?.textContent).toContain('Interstellar');
    expect(element.textContent).toContain('Movie');
    expect(element.textContent).toContain('2014');
  });
});
