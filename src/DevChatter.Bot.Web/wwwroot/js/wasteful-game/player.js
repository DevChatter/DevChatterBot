class Player {
  constructor(canvas, context) {
    this._canvas = canvas;
    this._context = context;
    this._image = new Image();
    this._image.src =  '/images/ZedChatter/Hat-YellowShirt-Player-Idle-0.png';
    this._x = 84;
    this._y = 84;
  }
  
  draw() {
    this._context.drawImage(this._image, this._x, this._y);
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

  get location() {
    return { x: this._x, y: this._y };
  }
}
