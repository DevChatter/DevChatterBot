import { Obstacle } from '/js/wasteful-game/obstacle.js';
import { Info } from '/js/wasteful-game/info.js';
import { Direction } from '/js/wasteful-game/direction.js';
import { Zombie } from '/js/wasteful-game/zombie.js';
import { Player } from '/js/wasteful-game/player.js';
import { Taco } from '/js/wasteful-game/taco.js';
import { Grid } from '/js/wasteful-game/grid.js';
import { Background } from '/js/wasteful-game/background.js';

const wastefulGray = '#cccccc';
const hangryRed = '#ff0000';
const wastefulInfoWidth = 126;


export class Wasteful {
  constructor(canvas) {
    this._canvas = canvas;
    this._context = canvas.getContext('2d');
    this._grid = new Grid(this._canvas, this._context);
    this._info = new Info(this._canvas, this._context);
    this._player = new Player(this._grid);
    this._zombies = [ new Zombie(this._grid) ];
    this._items = [];
    this._items.push(new Taco(this._grid));
    this._createObstacles();
    this._isRunning = false;
  }

  startGame() {
    this._background = new Background(this._context, this._canvas.width - wastefulInfoWidth, this._canvas.height);

    this._isRunning = true;
    window.requestAnimationFrame(() => this._updateFrame());
  }

  movePlayer(direction) {
    this._player.move(new Direction(direction));
    this._zombies = this._zombies.filter(zombie => !zombie.isKilled);

    this._zombies.forEach(zombie => zombie.moveToward(this._player));
    if (this._player.health <= 0) {
      this._isRunning = false;
    }
  }

  _updateFrame() {
    if (this._isRunning) {
      this._clearCanvas();
      this._drawBackground();
      this._grid.draw();
      this._info.draw(this._player);
      window.requestAnimationFrame(() => this._updateFrame());
    } else {
      this._drawGameOver();
    }
  }

  _clearCanvas() {
    this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
  }

  _drawGameOver() {
    this._context.fillStyle = hangryRed;
    this._context.font = "128px Arial";
    this._context.fillText('Game Over', 20, this._canvas.height - 10);
  }

  _drawBackground() {
    this._background.drawBackground();

    this._context.fillStyle = wastefulGray;
    this._context.fillRect(this._canvas.width - wastefulInfoWidth, 0, wastefulInfoWidth, this._canvas.height);
  }

  _createObstacles() {
    for (let i = 0; i < 10; i++) {
      this._items.push(new Obstacle(this._grid));
    }
  }
}
