import { ItemEffect } from '/js/wasteful-game/item-effect.js';

export class Consumable {
  constructor(grid, itemEffect, imageSrc) {
    grid.addSprite(this);

    this._grid = grid; // for removing itself later
    this._image = new Image();
    this._image.src = imageSrc;
    this._x = 3;
    this._y = 3;
    this._itemEffect = itemEffect;

  }

  hitByPlayer(player) {
    this._grid.removeSprite(this);
    player.increaseHealth(this._itemEffect.healthChange);
    player.increasePoints(this._itemEffect.pointsChange);
  }

  hitByEnemy() {
    // Nothing happens
  }

  get location() {
    return { x: this._x, y: this._y };
  }

  get image() {
    return this._image;
  }
}
