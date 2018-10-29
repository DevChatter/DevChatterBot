import { Entity } from '/js/wasteful-game/entity/entity.js';
import { Sprite } from '/js/wasteful-game/entity/sprite.js';

export class BarrelFire extends Entity {
  /**
   * @param {Wasteful} game
   */
  constructor(game) {
    super(game, new Sprite('/images/ZedChatter/BarrelFires-0.png', 1, 1, 1));
  }
}
