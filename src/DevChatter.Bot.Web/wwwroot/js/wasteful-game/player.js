export class Player {
  constructor(grid) {
    grid.addSprite(this);
    this._grid = grid;

    this._image = new Image();
    this._image.src =  '/images/ZedChatter/Hat-YellowShirt-Player-Idle-0.png';
    this.setNewLocation(2, 2);
    this._max_health = 5;
    this._health = 3;
    this._points = 0;
  }

  move(dir) {
    let newX = this._x + dir.xChange;
    let newY = this._y + dir.yChange;
    if (this._grid.canMoveTo(newX, newY)) {
      this.setNewLocation(newX, newY);
    } else {
      let target = this._grid.atLocation(newX, newY);
      if (target) {
        target.hitByPlayer(this);
      }
    }
  }

  increaseHealth(amount) {
    this._health = Math.min(this._health + amount, this._max_health);
  }

  increasePoints(amount) {
    this._points += amount;
  }

  decreaseHealth(amount) {
    this._health -= amount;
  }

  hitByEnemy(zombie) {
    // TODO: Add defensive items later
    this.decreaseHealth(zombie.damage);
  }

  setNewLocation(x, y) {
    this._x = x;
    this._y = y;
  }

  get health() {
    return this._health;
  }

  get points() {
    return this._points;
  }

  get location() {
    return { x: this._x , y : this._y };
  }

  get image() {
    return this._image;
  }
}
