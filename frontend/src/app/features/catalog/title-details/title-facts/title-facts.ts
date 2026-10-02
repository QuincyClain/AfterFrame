import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { CatalogTitleDetails } from '../../data-access/catalog-api.models';

@Component({
  selector: 'app-title-facts',
  imports: [],
  templateUrl: './title-facts.html',
  styleUrl: './title-facts.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleFacts {
  readonly title = input.required<CatalogTitleDetails>();
}
