class Grid {
  constructor(canvas, context) {
    this._canvas = canvas;
    this._context = context;
    this._sprites = [];
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
}
