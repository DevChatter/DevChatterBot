const wastefulInfoWidth = 126;

export class Info {
  constructor(canvas, context) {
    this._canvas = canvas;
    this._context = context;
    this._leftX = canvas.width - wastefulInfoWidth;
    this._topY = 0;
  }

  draw(player) {
    this._context.fillStyle = "#000000";
    this._context.font = "20px Arial";
    this._context.fillText(`Health:${player.health}`, this._leftX + 5, this._topY + 30);
  }
}
