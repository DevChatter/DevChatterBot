import { Player } from '/js/wasteful-game/player.js';
import { MetaData } from '/js/wasteful-game/metadata.js';
const size = 42;

export class Grid {
  constructor(canvas, context) {
    this._canvas = canvas;
    this._context = context;
    this._sprites = [];
    this._max_x = Math.floor((canvas.width - MetaData.wastefulInfoWidth) / 42);
    this._max_y = Math.floor(canvas.height / 42);
  }

  getRandomOpenLocation() {
    let randomX;
    let randomY;
    do {
      randomX = Math.floor(Math.random() * this._max_x);
      randomY = Math.floor(Math.random() * this._max_y);
    } while (!this._isClearOfObstacles(randomX, randomY));

    return { x: randomX, y: randomY };
  }

  addSprite(sprite) {
    this._sprites.push(sprite);
  }

  removeSprite(sprite) {
    var index = this._sprites.indexOf(sprite);
    if (index > -1) {
      this._sprites.splice(index, 1);
    }
  }

  clearSprites() {
    this._sprites = [];
  }

  draw() {
    this._sprites.forEach(sprite => {
      let location = sprite.location;
      let image = sprite.image;
      this._context.drawImage(image, location.x * size, location.y * size);
    });
  }

  canMoveTo(x, y) {
    return this._isWithinBounds(x, y) && this._isClearOfObstacles(x, y);
  }

  atLocation(x, y) {
    return this._sprites.filter(sprite => {
      let location = sprite.location;
      return location.x === x && location.y === y;
    })[0];
  }

  get playerLocation() {
    let player = this._sprites.filter(sprite => sprite instanceof Player)[0];
    return player.location;
  }

  get lowerRightCorner() {
    return { x: this._max_x, y: this._max_y };
  }

  _isClearOfObstacles(x, y) {
    return this._sprites.every(sprite => {
      let location = sprite.location;
      return location.x !== x || location.y !== y;
    });
  }

  _isWithinBounds(x, y) {
    return x >= 0 && y >= 0 && x < this._max_x && y < this._max_y;
  }
}
