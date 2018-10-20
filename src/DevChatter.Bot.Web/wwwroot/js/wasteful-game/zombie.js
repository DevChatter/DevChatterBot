class Zombie {
  constructor(grid) {
    grid.addSprite(this);
    this._grid = grid;

    this._image = new Image();
    this._image.src = '/images/ZedChatter/Zombie-0.png';
    this._movable = new MovableEntity(grid, 7, 3);
  }

  moveToward(player) {
    let playerLocation = player.location;
    let zombieLocation = this._movable.location;

    if (playerLocation.x < zombieLocation.x) {
      this._movable.moveLeft();
    } else if (playerLocation.x > zombieLocation.x) {
      this._movable.moveRight();
    } else if (playerLocation.y < zombieLocation.y) {
      this._movable.moveUp();
    } else if (playerLocation.y > zombieLocation.y) {
      this._movable.moveDown();
    } else {
      // nom nom nom
    }
  }

  hitByPlayer(player) {
    this._grid.removeSprite(this); // kill zombie
    player.decreaseHealth(1); // get hurt in process
  }

  get location() {
    return this._movable.location;
  }

  get image() {
    return this._image;
  }
}
