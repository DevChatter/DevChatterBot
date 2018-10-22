import { Obstacle } from '/js/wasteful-game/obstacle.js';
import { Info } from '/js/wasteful-game/info.js';
import { Direction } from '/js/wasteful-game/direction.js';
import { Zombie } from '/js/wasteful-game/zombie.js';

const wastefulBrown = '#dfd29e';
const wastefulGray = '#cccccc';
const hangryRed = '#ff0000';
const size = 42;
const wastefulInfoWidth = 126;


export class Wasteful {
  constructor(canvas) {
    this._canvas = canvas;
    this._context = canvas.getContext('2d');
    this._grid = new Grid(this._canvas, this._context);
    this._info = new Info(this._canvas, this._context);
    this._player = new Player(this._grid);
    this._zombie = new Zombie(this._grid);
    this._items = [];
    this._items.push(new Taco(this._grid));
    this._createObstacles();
  }

  startGame() {
    window.requestAnimationFrame(() => this._updateFrame());
  }

  movePlayer(direction) {
    this._player.move(new Direction(direction));
    this._zombie.moveToward(this._player);
  }

  _updateFrame() {
    this._clearCanvas();
    this._drawBackground();
    this._grid.draw();
    this._info.draw(this._player);
    window.requestAnimationFrame(() => this._updateFrame());
  }

  _clearCanvas() {
    this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
  }

  _drawBackground() {
    this._context.fillStyle = wastefulBrown;
    this._context.fillRect(0, 0, this._canvas.width - wastefulInfoWidth, this._canvas.height);

    this._context.fillStyle = wastefulGray;
    this._context.fillRect(this._canvas.width - wastefulInfoWidth, 0, wastefulInfoWidth, this._canvas.height);
  }

  _createObstacles() {
    for (let i = 0; i < 10; i++) {
      this._items.push(new Obstacle(this._grid));
    }
  }
}
