import { MetaData } from '/js/wasteful-game/metadata.js';
import '/lib/pathfindingjs/pathfinding-browser.js';

export class Grid {
  /**
   * @param {EntityManager} entityManger entities to show on grid
   * @param {HTMLCanvasElement} canvas canvas used only fir width and height
   */
  constructor(entityManger, canvas) {
    this._entityManager = entityManger;
    this._max_x = Math.floor((canvas.width - MetaData.wastefulInfoWidth) / MetaData.tileSize);
    this._max_y = Math.ceil(canvas.height / MetaData.tileSize);
  }

  /**
   * @public
   * @returns {{x: number, y: number}} random x and y coordinates clear of other entities
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
   * @param {{x: number, y: number}} startLoc starting location
   * @param {{x: number, y: number}} endLoc ending location
   * @returns {Array<Array<number>>} path from start to end location
   */
  findPath(startLoc, endLoc) {
    let gridArray = [];
    for (let y = 0; y < this._max_y; y++) {
      gridArray[y] = [];
      for (let x = 0; x < this._max_x; x++) {
        const isEndLoc = endLoc.x === x && endLoc.y === y;
        if (this._canWalkOn(x,y) || isEndLoc) {
          gridArray[y][x] = 0;
        } else {
          gridArray[y][x] = 1;
        }
      }
    }
    let pathGrid = new PF.Grid(gridArray);
    let finder = new PF.AStarFinder();
    let path = finder.findPath(startLoc.x, startLoc.y, endLoc.x, endLoc.y, pathGrid);
    path.shift();
    return path.map(step => ({ x: step[0], y:step[1] }));
  }

  /**
   * @public
   * @param {number} x horizontal index
   * @param {number} y vertical index
   * @returns {boolean} true if location is open for movement
   */
  canMoveTo(x, y) {
    return this._isWithinBounds(x, y) && this._canWalkOn(x, y);
  }

  /**
   * @public
   * @returns {{x: number, y: number}} coordinates of the lower right corner
   */
  get lowerRightCorner() {
    return { x: this._max_x, y: this._max_y };
  }

  /**
   * @private
   * @param {number} x x index
   * @param {number} y y index
   * @returns {boolean} true if there are no entities, false if there is one or more entity
   */
  _isClearOfEntities(x, y) {
    return this._entityManager.all.every(entity => {
      let location = entity.location;
      return location.x !== x || location.y !== y;
    });
  }

  /**
   * @public
   * @param {number} x x index
   * @param {number} y y index
   * @returns {boolean} true if location is empty or otherwise walkable
   */
  _canWalkOn(x, y) {
    return this._entityManager.all.every(entity => {
      let location = entity.location;
      return entity.walkable || location.x !== x || location.y !== y;
    });
  }

  /**
   * @public
   * @param {number} x x index
   * @param {number} y y index
   * @returns {boolean} true if within bounds of grid; false if out of bounds
   */
  _isWithinBounds(x, y) {
    return x >= 0 && y >= 0 && x < this._max_x && y < this._max_y;
  }
}
