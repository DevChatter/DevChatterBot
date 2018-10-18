class Player {
  constructor() {
    this._image = new Image();
    this._image.src =  '/images/ZedChatter/Hat-YellowShirt-Player-Idle-0.png';
    this._x = 2;
    this._y = 2;
  }

  move(direction) {
    switch(direction) {
      case 'left':
        this._moveLeft();
        break;
      case 'right':
        this._moveRight();
        break;
      case 'up':
        this._moveUp();
        break;
      case 'down':
        this._moveDown();
        break;
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
