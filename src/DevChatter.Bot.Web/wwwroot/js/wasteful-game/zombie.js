import { Direction } from '/js/wasteful-game/direction.js';

export class Zombie {
  constructor(grid, x, y) {
    grid.addSprite(this);
    this._grid = grid;

    this._image = new Image();
    this._image.src = '/images/ZedChatter/Zombie-0.png';
    this._x = x || 7;
    this._y = y || 3;
    this._damage = 1;
    this.isKilled = false;
  }

  takeTurn() {
    let playerLocation = this._grid.playerLocation;

    if (playerLocation.x < this._x) {
      this._move(new Direction('left'));
    } else if (playerLocation.x > this._x) {
      this._move(new Direction('right'));
    } else if (playerLocation.y < this._y) {
      this._move(new Direction('up'));
    } else if (playerLocation.y > this._y) {
      this._move(new Direction('down'));
    } else {
      // nom nom nom
    }
  }

  _move(dir) {
    let newX = this._x + dir.xChange;
    let newY = this._y + dir.yChange;
    if (this._grid.canMoveTo(newX, newY)) {
      this._x = newX;
      this._y = newY;
    } else {
      let target = this._grid.atLocation(newX, newY);
      target.hitByEnemy(this);
    }
  }


  hitByPlayer(player) {
    this.isKilled = true;
    this._grid.removeSprite(this); // kill zombie
    if (!player.tryUseWeapon()) {
      player.decreaseHealth(1); // get hurt in process
    }
    player.increasePoints(20);
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

  get damage() {
    return this._damage;
  }
}
