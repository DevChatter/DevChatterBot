export class Obstacle {
  constructor(grid) {
    grid.addSprite(this);

    this._grid = grid; // for removing itself later
    this._image = new Image();
    this._image.src = '/images/ZedChatter/BarrelFires-0.png';

    var location = this._grid.getRandomOpenLocation();

    this._x = location.x;
    this._y = location.y;
  }

  hitByPlayer() {
    // Nothing happens
  }

  hitByZombie() {
    // Nothing happens
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  get image() {
    return this._image;
  }
}
