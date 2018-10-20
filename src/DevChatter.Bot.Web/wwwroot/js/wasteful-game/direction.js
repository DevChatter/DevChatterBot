export class Direction {
  constructor(word) {
    this._x = 0;
    this._y = 0;

    switch (word) {
      case "left":
        this._x = -1;
      break;
      case "right":
        this._x = +1;
      break;
      case "up":
        this._y = -1;
      break;
      case "down":
        this._y = +1;
      break;
    default:
    }
  }

  get xChange() {
    return this._x;
  }
  get yChange() {
    return this._y;
  }
}
