import { Component, input } from '@angular/core';
import { Title } from '../title';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-title-card',
  imports: [RouterLink],
  templateUrl: './title-card.html',
  styleUrl: './title-card.css',
})
export class TitleCard {
  readonly title = input.required<Title>();
}
