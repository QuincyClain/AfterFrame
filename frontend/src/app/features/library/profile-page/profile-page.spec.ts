import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ProfilePage } from './profile-page';

describe('ProfilePage', () => {
  let fixture: ComponentFixture<ProfilePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfilePage],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(ProfilePage);

    await fixture.whenStable();
  });

  it('should render rated titles sorted by rating', () => {
    const element = fixture.nativeElement as HTMLElement;
    const titles = Array.from(element.querySelectorAll('h3')).map((heading) => heading.textContent?.trim(),);

    expect(titles).toEqual(['Interstellar', 'Dark', 'Inception']);
  });
});
