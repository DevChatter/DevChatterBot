export class ExitTile {
  constructor(grid) {
    grid.addSprite(this);

    this._grid = grid;
    this._image = new Image();
    this._image.src = '/images/ZedChatter/ExitTile-0.png';

    let location = grid.lowerRightCorner;
    this._x = location.x - 1; // TODO: Track down off-by-one error
    this._y = location.y/2;
  }

  hitByPlayer() {
    // TODO: Exit Level
    debugger;
  }

  hitByEnemy() {
    // Nothing happens
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  get image() {
    return this._image;
  }
}
