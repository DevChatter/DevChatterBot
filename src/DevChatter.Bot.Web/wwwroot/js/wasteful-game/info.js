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
    let lineNumber = 0;
    this._context.fillStyle = "#000000";
    this._context.font = "16px Arial";

    this._writeLine(this._playerName, ++lineNumber);
    this._writeLine('\u{1F499}'.repeat(player.health), ++lineNumber);
    this._writeLine(`\u20BF: ${player.points}`, ++lineNumber);
  }

  _writeLine(text, lineNumber) {
    this._context.fillText(text, this._leftX + 5, this._topY + 24 * lineNumber);
  }
}
