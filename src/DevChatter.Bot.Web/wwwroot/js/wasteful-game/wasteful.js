const wastefulBrown = '#dfd29e';
const hangryRed = '#ff0000';

class Wasteful {
  constructor(canvas) {
    this._canvas = canvas;
    this._context = canvas.getContext('2d');
    this._player = player(this._canvas, this._context);
    this._zombie = zombie(this._canvas, this._context);
  }

  startGame() {
    window.requestAnimationFrame(() => this._updateFrame());
  }

  movePlayer(direction) {
    this._player.move(direction);
    this._zombie.moveToward(this._player);
  }

  _updateFrame() {
    this._clearCanvas();
    this._drawBackground();
    this._player.draw();
    this._zombie.draw();
    window.requestAnimationFrame(() => this._updateFrame());
  }

  _clearCanvas() {
    this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
  }

  _drawBackground() {
    this._context.fillStyle = wastefulBrown;
    this._context.fillRect(0, 0, this._canvas.width, this._canvas.height);
  }
}
