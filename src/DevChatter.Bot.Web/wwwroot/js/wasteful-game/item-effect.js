export class ItemEffect {
  constructor(healthChange, pointsChange) {
    this._healthChange = healthChange;
    this._pointsChange = pointsChange;
  }

  get healthChange() {
    return this._healthChange;
  }

  get pointsChange() {
    return this._pointsChange;
  }
}
