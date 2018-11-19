import { Entity } from '/js/wasteful-game/entity/entity.js';
import { Sprite } from '/js/wasteful-game/entity/sprite.js';

const bluePieces = {
  door: 'images/ZedChatter/Walls/ExteriorWall-Blue-Door-0.png',
  left: 'images/ZedChatter/Walls/ExteriorWall-Blue-Left-0.png',
  middle: 'images/ZedChatter/Walls/ExteriorWall-Blue-Middle-0.png',
  window: 'images/ZedChatter/Walls/ExteriorWall-Blue-MiddleWindow-0.png',
  right: 'images/ZedChatter/Walls/ExteriorWall-Blue-Right-0.png'
};


export class ExWall extends Entity {
  /**
   * @param {Wasteful} game the game object
   * @param {string} imgSrc get source from static properties like: ExWall.blue.window
   */
  constructor(game, imgSrc, x, y) {
    super(game, new Sprite(imgSrc, 1, 1, 1));
    this.setLocation(x, y);
  }

  /**
  * @public
  * @returns {Object} object with fields of image sources for pieces
  */
  static get blue() {
    return bluePieces;
  }
}

