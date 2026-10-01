import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { CatalogPage } from './catalog-page';

describe('CatalogPage', () => {
  let component: CatalogPage;
  let fixture: ComponentFixture<CatalogPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CatalogPage],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(CatalogPage);
    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
