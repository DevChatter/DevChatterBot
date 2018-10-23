import { Obstacle } from '/js/wasteful-game/obstacle.js';
import { Info } from '/js/wasteful-game/info.js';
import { Direction } from '/js/wasteful-game/direction.js';
import { Zombie } from '/js/wasteful-game/zombie.js';
import { Player } from '/js/wasteful-game/player.js';
import { Taco } from '/js/wasteful-game/taco.js';
import { Grid } from '/js/wasteful-game/grid.js';

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
    this._isRunning = false;
    this._backgroundImages = this._getBackgroundImages();
    this._tileGrid = [];
  }

  startGame() {
    this._tileGrid = this._createRandomGrid(this._canvas.width - wastefulInfoWidth, this._canvas.height, this._backgroundImages.length);

    this._isRunning = true;
    window.requestAnimationFrame(() => this._updateFrame());
  }

  movePlayer(direction) {
    this._player.move(new Direction(direction));
    this._zombie.moveToward(this._player);

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
    this._tileGrid.forEach(
      (col, colIndex) => col.forEach(
        (imgIndex, rowIndex) => {
          let image = this._backgroundImages[imgIndex];
          this._context.drawImage(image, size * colIndex, size * rowIndex);
        }));

    this._context.fillStyle = wastefulGray;
    this._context.fillRect(this._canvas.width - wastefulInfoWidth, 0, wastefulInfoWidth, this._canvas.height);
  }

  _createObstacles() {
    for (let i = 0; i < 10; i++) {
      this._items.push(new Obstacle(this._grid));
    }
  }

  _getBackgroundImages() {
    let img1 = new Image();
    img1.src = '/images/ZedChatter/RockyGroundTile-0.png';
    let img2 = new Image();
    img2.src = '/images/ZedChatter/RockyGroundTile-1.png';
    let img3 = new Image();
    img3.src = '/images/ZedChatter/RockyGroundTile-2.png';
    return [img1, img2, img2, img3, img3, img3];
  }

  _createRandomGrid(width, height, choiceCount) {
    let w = width / size;
    let h = height / size;
    var result = [];
    for (var i = 0; i < w; i++) {
      result[i] = [];
      for (var j = 0; j < h; j++) {
        result[i][j] = Math.floor(Math.random() * choiceCount);
      }
    }
    return result;
  }
}
