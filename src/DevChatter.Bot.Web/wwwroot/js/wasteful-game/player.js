class Player {
  constructor(grid) {
    grid.addSprite(this);
    this._grid = grid;

    this._image = new Image();
    this._image.src =  '/images/ZedChatter/Hat-YellowShirt-Player-Idle-0.png';
    this._x = 2;
    this._y = 2;
    this._max_health = 5;
    this._health = 3;
  }

  move(dir) {
    let newX = this._x + dir.xChange;
    let newY = this._y + dir.yChange;
    if (this._grid.canMoveTo(newX, newY)) {
      this._x = newX;
      this._y = newY;
    } else {
      let target = this._grid.atLocation(newX, newY);
      target.hitByPlayer(this);
    }
  }

  increaseHealth(amount) {
    this._health = Math.min(this._health + amount, this._max_health);
  }

  decreaseHealth(amount) {
    this._health -= amount;
    // TODO: Check for death
  }

  get health() {
    return this._health;
  }

  get location() {
    return { x: this._x , y : this._y };
  }

  get image() {
    return this._image;
  }
}
