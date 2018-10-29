import { MetaData } from '/js/wasteful-game/metadata.js';

export class Grid {
  /**
   * @param {EntityManager} entityManger
   * @param {HTMLCanvasElement} canvas
   */
  constructor(entityManger, canvas) {
    this._entityManager = entityManger;
    this._max_x = Math.floor((canvas.width - MetaData.wastefulInfoWidth) / MetaData.tileSize);
    this._max_y = Math.ceil(canvas.height / MetaData.tileSize);
  }

  /**
   * @public
   * @returns {{x: number, y: number}}
   */
  getRandomOpenLocation() {
    let randomX;
    let randomY;
    do {
      randomX = Math.floor(Math.random() * this._max_x);
      randomY = Math.floor(Math.random() * this._max_y);
    } while (!this._isClearOfEntities(randomX, randomY));

    return { x: randomX, y: randomY };
  }

  /**
   * @public
   * @param {number} x
   * @param {number} y
   * @returns {boolean}
   */
  canMoveTo(x, y) {
    return this._isWithinBounds(x, y) && this._canWalkOn(x, y);
  }

  /**
   * @public
   * @returns {{x: number, y: number}}
   */
  get lowerRightCorner() {
    return { x: this._max_x, y: this._max_y };
  }

  /**
   * @private
   * @param {number} x
   * @param {number} y
   * @returns {boolean}
   */
  _isClearOfEntities(x, y) {
    return this._entityManager.all.every(entity => {
      let location = entity.location;
      return location.x !== x || location.y !== y;
    });
  }

  /**
   * @public
   * @param {number} x
   * @param {number} y
   * @returns {boolean}
   */
  _canWalkOn(x, y) {
    return this._entityManager.all.every(entity => {
      let location = entity.location;
      return entity.walkable || location.x !== x || location.y !== y;
    });
  }

  /**
   * @public
   * @param {number} x
   * @param {number} y
   * @returns {boolean}
   */
  _isWithinBounds(x, y) {
    return x >= 0 && y >= 0 && x < this._max_x && y < this._max_y;
  }
}
