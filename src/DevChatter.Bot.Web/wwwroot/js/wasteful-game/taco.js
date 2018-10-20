class Taco {
  constructor(grid) {
    grid.addSprite(this);

    this._grid = grid; // for removing itself later
    this._image = new Image();
    this._image.src =  '/images/ZedChatter/Taco-0.png';
    this._x = 3;
    this._y = 3;
  }

  hitByPlayer(player) {
    this._grid.removeSprite(this);
    player.increaseHealth(1);
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  get image() {
    return this._image;
  }
}
