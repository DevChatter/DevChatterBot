const wastefulInfoWidth = 126;

export class Info {
  constructor(canvas, context, displayName) {
    this._canvas = canvas;
    this._context = context;
    this._leftX = canvas.width - wastefulInfoWidth;
    this._topY = 0;
    this._playerName = displayName;
  }

  draw(player) {
    this._context.fillStyle = "#000000";
    this._context.font = "16px Arial";

    this._context.fillText(`Player:`, this._leftX + 5, this._topY + 24);

    this._context.fillText(this._playerName, this._leftX + 5, this._topY + 48);

    this._context.fillText('\u{1F499}'.repeat(player.health), this._leftX + 5, this._topY + 72);

    this._context.fillText(`\u20BF: ${player.points}`, this._leftX + 5, this._topY + 96);
  }
}
