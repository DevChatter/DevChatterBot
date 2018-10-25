import { Zombie } from '/js/wasteful-game/zombie.js';

export class ZombieSpawner {
  constructor(grid, x, y) {
    grid.addSprite(this);
    this._grid = grid;
    this._image = new Image();
    this._image.src = '/images/ZedChatter/Fresh-Gravestone-1.png';
    this._x = x || 7;
    this._y = y || 3;
    this._turnNumber = 0;
    this.isKilled = false;
  }

  takeTurn() {
    if (this._turnNumber === 0) {
      this._image.src = '/images/ZedChatter/Zombie-Gravestone-1.png';
      this._turnNumber++;
    } else {
      this.isKilled = true;
      this._grid.removeSprite(this);
      let zombie = new Zombie(this._grid, this._x, this._y);
      this._grid.addSprite(zombie);
      return zombie;
    }
  }

  hitByPlayer(player) {
    this.isKilled = true;
    this._grid.removeSprite(this);
    player.increasePoints(5);
  }

  hitByEnemy() {
    // do nothing
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  get image() {
    return this._image;
  }
}
