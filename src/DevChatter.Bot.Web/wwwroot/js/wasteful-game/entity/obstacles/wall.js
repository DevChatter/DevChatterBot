import { Entity } from '/js/wasteful-game/entity/entity.js';
import { Sprite } from '/js/wasteful-game/entity/sprite.js';

const wallPieces = {
  '0': 'images/ZedChatter/Walls/InteriorWall-A-Nothing-0.png',
  '<': 'images/ZedChatter/Walls/InteriorWall-A-TopLeft-0.png',
  '_': 'images/ZedChatter/Walls/InteriorWall-A-Middle-0.png',
  '>': 'images/ZedChatter/Walls/InteriorWall-A-TopRight-0.png',
  '/': 'images/ZedChatter/Walls/InteriorWall-A-Left-0.png',
  '|': 'images/ZedChatter/Walls/InteriorWall-A-Right-0.png',
  '(': 'images/ZedChatter/Walls/InteriorWall-A-LBlock-0.png',
  ')': 'images/ZedChatter/Walls/InteriorWall-A-ReverseLBlock-0.png',
  '-': 'images/ZedChatter/Walls/InteriorWall-A-Lower-0.png',
  '[': 'images/ZedChatter/Walls/InteriorWall-A-LowerLeft-0.png',
  ']': 'images/ZedChatter/Walls/InteriorWall-A-LowerRight-0.png'
};


export class Wall extends Entity {
  /**
   * @param {Wasteful} game the game object
   * @param {string} key indicates the type of wall piece. Examples: 0,<,_,>,/,|,(,),-,[,]
   */
  constructor(game, key) {
    super(game, new Sprite(wallPieces[key], 1, 1, 1));
  }
}
