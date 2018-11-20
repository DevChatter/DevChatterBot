import { Entity } from '/js/wasteful-game/entity/entity.js';
import { Sprite } from '/js/wasteful-game/entity/sprite.js';
import { AttackingComponent } from '/js/wasteful-game/entity/components/attackingComponent.js';

export class CampFire extends Entity {
  /**
   * @param {Wasteful} game
   */
  constructor(game) {
    super(game, new Sprite('/images/ZedChatter/Campfire-0.png', 1, 1, 1));

    this._attackingComponent = new AttackingComponent(game, this, false, 1, 1);

    this.addComponent(this._attackingComponent);
  }
}
