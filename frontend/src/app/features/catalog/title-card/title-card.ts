import { Component, input } from '@angular/core';
import { Title } from '../title';

@Component({
  selector: 'app-title-card',
  imports: [],
  templateUrl: './title-card.html',
  styleUrl: './title-card.css',
})
export class TitleCard {
  readonly title = input.required<Title>();
}
