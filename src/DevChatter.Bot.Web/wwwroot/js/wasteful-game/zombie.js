class Zombie {
  constructor() {
    this._image = new Image();
    this._image.src = '/images/ZedChatter/Zombie-0.png';
    this._x = 20;
    this._y = 3;
  }

  moveToward(player) {
    let location = player.location;
    if (location.x < this._x) {
      this._moveLeft();
    } else if (location.x > this._x) {
      this._moveRight();
    } else if (location.y < this._y) {
      this._moveUp();
    } else if (location.y > this._y) {
      this._moveDown();
    } else {
      // nom nom nom
    }
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  get image() {
    return this._image;
  }

  _moveLeft() {
    this._x -= 1;
  }

  _moveRight() {
    this._x += 1;
  }

  _moveUp() {
    this._y -= 1;
  }

  _moveDown() {
    this._y += 1;
  }
}
