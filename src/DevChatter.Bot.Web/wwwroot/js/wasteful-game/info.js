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
    this._lineNumber = 1;
    this._context.fillStyle = "#000000";
    this._context.font = "16px Arial";

    this._writeLine(this._playerName);
    this._writeLine('\u{1F499}'.repeat(player.health));
    this._writeLine(`\u20BF: ${player.points}`);
  }

  _writeLine(text, lineNumber) {
    this._context.fillText(text, this._leftX + 5, this._topY + 24 * this._lineNumber);
    this._lineNumber++;
  }
}
