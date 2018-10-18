const size = 42;

class Zombie {
  constructor(canvas, context) {
    this._canvas = canvas;
    this._context = context;
    this._image = new Image();
    this._image.src = '/images/ZedChatter/Zombie-0.png';
    this._x = 840;
    this._y = 126;
  }

  draw() {
    this._context.drawImage(this._image, this._x, this._y);
  }

  moveToward(player) {
    let location = player.getLocation();
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
      // How you get on the player?!?!?!
    }
  }

  _moveLeft() {
    this._x = Math.max(0, this._x - size);
  }

  _moveRight() {
    this._x = Math.min(this._canvas.width - size, this._x + size);
  }

  _moveUp() {
    this._y = Math.max(0, this._y - size);
  }

  _moveDown() {
    this._y = Math.min(this._canvas.height - size, this._y + size);
  }
}
