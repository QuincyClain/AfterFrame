import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { CatalogTitleType } from '../../catalog/data-access/catalog-api.models';

@Component({
  selector: 'app-title-activity-card',
  imports: [],
  templateUrl: './title-activity-card.html',
  styleUrl: './title-activity-card.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleActivityCard {
  readonly titleType = input.required<CatalogTitleType>();
}
