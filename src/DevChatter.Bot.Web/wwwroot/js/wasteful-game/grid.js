class Grid {
  constructor(canvas, context) {
    this._canvas = canvas;
    this._context = context;
    this._sprites = [];
    this._max_x = Math.floor(canvas.width / 42);
    this._max_y = Math.floor(canvas.height / 42);
  }

  addSprite(sprite) {
    this._sprites.push(sprite);
  }

  draw() {
    this._sprites.forEach(sprite => {
      let location = sprite.location;
      let image = sprite.image;
      this._context.drawImage(image, location.x * size, location.y * size);
    });
  }

  canMoveTo(x, y) {
    let withinBounds = x >= 0 && y >= 0 && x < this._max_x && y < this._max_y;
    return withinBounds;
  }
}
