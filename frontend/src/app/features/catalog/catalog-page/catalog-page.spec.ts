import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { vi } from 'vitest';
import { CatalogApi } from '../data-access/catalog-api';
import { CatalogPage } from './catalog-page';

describe('CatalogPage', () => {
  let component: CatalogPage;
  let fixture: ComponentFixture<CatalogPage>;
  const catalogResult = {
    items: [],
    page: 1,
    pageSize: 24,
    totalCount: 0,
    totalPages: 0,
  };
  const getCatalog = vi.fn(() => of(catalogResult));

  beforeEach(async () => {
    getCatalog.mockClear();

    await TestBed.configureTestingModule({
      imports: [CatalogPage],
      providers: [
        provideRouter([]),
        {
          provide: CatalogApi,
          useValue: {
            getCatalog,
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CatalogPage);
    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should reload the full catalog when an applied search is cleared', () => {
    const element = fixture.nativeElement as HTMLElement;
    const input = element.querySelector<HTMLInputElement>('input[type="search"]')!;
    const form = element.querySelector<HTMLFormElement>('form')!;

    input.value = 'mentalist';
    input.dispatchEvent(new Event('input'));
    form.dispatchEvent(new Event('submit'));

    expect(getCatalog).toHaveBeenLastCalledWith(expect.objectContaining({ search: 'mentalist' }));

    input.value = '';
    input.dispatchEvent(new Event('input'));

    expect(getCatalog).toHaveBeenLastCalledWith(expect.objectContaining({ search: undefined }));
  });
});
