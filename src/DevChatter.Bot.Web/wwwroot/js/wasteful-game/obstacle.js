class Obstacle {
  constructor(grid) {
    grid.addSprite(this);

    this._grid = grid; // for removing itself later
    this._image = new Image();
    this._image.src =  '/images/ZedChatter/BarrelFires-0.png';
    this._x = 8;
    this._y = 4;
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  get image() {
    return this._image;
  }
}
