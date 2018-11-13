import { SimpleZombie } from '/js/wasteful-game/entity/enemies/simple-zombie.js';
import { BarrelFire } from '/js/wasteful-game/entity/obstacles/barrel-fire.js';
import { AutonomousComponent } from '/js/wasteful-game/entity/components/autonomousComponent.js';
import { ExitItem } from '/js/wasteful-game/entity/items/exit-item.js';
import { EscapeItem } from '/js/wasteful-game/entity/items/escape-item.js';
import { Sprite } from '/js/wasteful-game/entity/sprite.js';
import { ItemEffectType, ItemPickupType, ItemType } from '/js/wasteful-game/entity/items/item.js';

export class Level {
  /**
   * @param {Wasteful} game wasteful game object
   * @param {Player} player player playing the level
   * @param {ItemBuilder} itemBuilder item builder to build items for new levels
   */
  constructor(game, player, itemBuilder) {
    this._player = player;
    this._game = game;
    this._itemBuilder = itemBuilder;

    this._levelNumber = 0;
  }

  /**
   * @public
   * @returns {number} current level number
   */
  get levelNumber() {
    return this._levelNumber;
  }

  /**
   * @public
   */
  next() {
    this._turnNumber = 0;
    const oldExitLocation = this._game.entityManager.getFirstByClass(ExitItem);
    this._game.entityManager.clear();

    this._levelNumber++;

    if (typeof oldExitLocation !== 'undefined') {
      this._player.setLocation(0, oldExitLocation.location.y);
    }

    this._game.entityManager.add(this._player);

    if (this._levelNumber === 3) {
      this._exit = this._createEscape();
    } else {
      this._exit = this._createExit();
    }

    this._game.entityManager.add(this._exit);

    for (let i = 0; i < this._levelNumber; i++) {
      const zombie = new SimpleZombie(this._game);
      zombie.setLocation(this._game.grid.getRandomOpenLocation());
      this._game.entityManager.add(zombie);
    }

    const items = this._itemBuilder.getItemsForLevel(this._levelNumber);
    for (let i = 0; i < items.length; i++) {
      items[i].setLocation(this._game.grid.getRandomOpenLocation());
      this._game.entityManager.add(items[i]);
    }

    for (let i = 0; i < 10; i++) {
      const barrel = new BarrelFire(this._game);
      barrel.setLocation(this._game.grid.getRandomOpenLocation());
      this._game.entityManager.add(barrel);
    }

    let requiredLocation = this._exit.location;
    let path = this._game.grid.findPath(this._player.location, requiredLocation);
    if (path.length === 0) {
      // No path to exit exists. Try again.
      this._levelNumber--;
      this.next();
    }
  }

  /**
   * @public
   */
  update() {
    for (let i = 0, l = this._game.entityManager.count; i < l; i++) {
      const entity = this._game.entityManager.all[i];
      if (typeof entity !== 'undefined' && entity.hasComponent(AutonomousComponent)) {
        entity.getComponent(AutonomousComponent).takeTurn();
      }
    }

    this._spawnZombie();
    this._turnNumber++;
  }

  /**
   * @private
   */
  _spawnZombie() {
    if (this._turnNumber % 8 === 0 && this._turnNumber > 0) {
      const zombie = new SimpleZombie(this._game);
      zombie.setLocation(this._game.grid.getRandomOpenLocation());
      this._game.entityManager.add(zombie);
    }
  }

  /**
   * @private
   * @return {ExitItem} new exit item
   */
  _createExit() {
    return new ExitItem(
      this._game,
      new Sprite('/images/ZedChatter/ExitTile-0.png', 1, 1, 1),
      ItemType.CONSUMABLE,
      ItemPickupType.INSTANT,
      [ItemEffectType.HEALTH, ItemEffectType.POINTS],
      1,
      [
        { [ItemEffectType.HEALTH]: 1 },
        { [ItemEffectType.POINTS]: 20 + this._levelNumber * 3 }
      ]
    );
  }

  /**
   * @private
   * @return {EscapeItem} new escape item
   */
  _createEscape() {
    return new EscapeItem(
      this._game,
      new Sprite('images/ZedChatter/HeliPad-0.png', 1, 1, 1),
      ItemType.CONSUMABLE,
      ItemPickupType.INSTANT,
      ItemEffectType.POINTS,
      1,
      [
        { [ItemEffectType.POINTS]: 100 }
      ]
    );
  }
}
