class MovableEntity {
  constructor(grid, x, y) {
    this._grid = grid;
    this._x = x;
    this._y = y;
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  moveLeft() {
    if (this._grid.canMoveTo(this._x - 1, this._y)) {
      this._x -= 1;
    }
  }

  moveRight() {
    if (this._grid.canMoveTo(this._x + 1, this._y)) {
      this._x += 1;
    }
  }

  moveUp() {
    if (this._grid.canMoveTo(this._x, this._y - 1)) {
      this._y -= 1;
    }
  }

  moveDown() {
    if (this._grid.canMoveTo(this._x, this._y + 1)) {
      this._y += 1;
    }
  }
}
