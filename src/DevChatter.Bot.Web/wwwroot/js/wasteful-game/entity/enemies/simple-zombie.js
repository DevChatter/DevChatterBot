import { Enemy } from '/js/wasteful-game/entity/abstracts/enemy.js';
import { SpawnableComponent } from '/js/wasteful-game/entity/components/spawnableComponent.js';
import { Sprite } from '/js/wasteful-game/entity/sprite.js';

export class SimpleZombie extends Enemy {
  /**
   * @param {Wasteful} game
   */
  constructor(game) {
    super(
      game,
      new Sprite('/images/ZedChatter/Zombie-0.png', 1, 1, 1),
      1, // steps
      1, // turns
      1, // health
      false, // diagonal
      1, // range
      1, // damage
    );

    this._spawnableComponent = new SpawnableComponent(game, this, [
      { sprite: new Sprite('/images/ZedChatter/Fresh-Gravestone-1.png', 1, 1, 1), attackable: false, walkable: true },
      // Zombie head is already visible, so let the player attack it even if not fully spawned yet
      { sprite: new Sprite('/images/ZedChatter/Zombie-Gravestone-1.png', 1, 1, 1), attackable: true, walkable: false },
    ]);

    this.addComponent(this._spawnableComponent);
  }
}
