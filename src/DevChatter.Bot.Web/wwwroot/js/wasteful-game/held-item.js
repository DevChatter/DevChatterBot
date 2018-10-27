export class HeldItem {
  constructor(grid, image) {
    this._grid = grid;
    this._grid.addSprite(this);
    this._image = new Image();
    this._image.src = image;

    this._x = 1;
    this._y = 1;
  }

  hitByPlayer(player) {
    player.inventory.addItem(this);
    this._grid.removeSprite(this);
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  get image() {
    return this._image;
  }
}
