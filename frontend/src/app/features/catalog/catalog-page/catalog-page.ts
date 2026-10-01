import { Component, inject } from '@angular/core';
import { CatalogData } from '../catalog-data';

@Component({
  imports: [],
  selector: 'app-catalog-page',
  styleUrl: './catalog-page.css',
  templateUrl: './catalog-page.html',
})
export class CatalogPage {
  private readonly catalogData = inject(CatalogData);

  protected readonly titles = this.catalogData.getTitles();
}
