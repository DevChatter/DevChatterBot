class MovableEntity {
  constructor(x, y) {
    this._x = x;
    this._y = y;
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  moveLeft() {
    this._x -= 1;
  }

  moveRight() {
    this._x += 1;
  }

  moveUp() {
    this._y -= 1;
  }

  moveDown() {
    this._y += 1;
  }
}
