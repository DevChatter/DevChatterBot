class Player {
  constructor(grid) {
    grid.addSprite(this);

    this._image = new Image();
    this._image.src =  '/images/ZedChatter/Hat-YellowShirt-Player-Idle-0.png';
    this._movable = new MovableEntity(grid, 2, 2);
  }

  move(direction) {
    switch(direction) {
      case 'left':
        this._movable.moveLeft();
        break;
      case 'right':
        this._movable.moveRight();
        break;
      case 'up':
        this._movable.moveUp();
        break;
      case 'down':
        this._movable.moveDown();
        break;
    }
  }

  get location() {
    return this._movable.location;
  }

  get image() {
    return this._image;
  }
}
