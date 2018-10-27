import { Zombie } from '/js/wasteful-game/zombie.js';
import { ZombieSpawner } from '/js/wasteful-game/zombie-spawner.js';
import { Consumable } from '/js/wasteful-game/consumable.js';
import { ItemEffect } from '/js/wasteful-game/item-effect.js';
import { Obstacle } from '/js/wasteful-game/obstacle.js';
import { HeldItem } from '/js/wasteful-game/held-item.js';

export class Level {
  constructor(grid, levelNumber) {
    this._grid = grid;
    this._turnNumber = 0;

    this._actors = [];
    for (let i = 0; i < levelNumber; i++) {
      this._actors.push(new Zombie(this._grid));
    }

    this._items = [];
    // TODO: Random consumable selection
    let consumable = new Consumable(this._grid, new ItemEffect(1, 5), '/images/ZedChatter/Taco-0.png');
    this._items.push(consumable);

    let heldItem = new HeldItem(this._grid, '/images/ZedChatter/BaseballBat-1.png');
    this._items.push(heldItem);

    for (let i = 0; i < 10; i++) {
      this._items.push(new Obstacle(this._grid));
    }
  }

  update() {
    this._removeZombies();

    this._actors.forEach(actor => {
      let spawn = actor.takeTurn();
      if (spawn) {
        this._actors.push(spawn);
      }
    });

    this._addZombieSpawners();

    this._turnNumber++;
  }

  _removeZombies() {
    this._actors = this._actors.filter(zombie => !zombie.isKilled);
  }

  _addZombieSpawners() {
    if (this._turnNumber % 6 === 0 && this._turnNumber > 0) {
      let location = this._grid.getRandomOpenLocation();
      this._actors.push(new ZombieSpawner(this._grid, location.x, location.y));
    }
  }

  get actors() {
    return this._actors;
  }
}
