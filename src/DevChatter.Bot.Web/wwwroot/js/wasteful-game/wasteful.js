import { Obstacle } from '/js/wasteful-game/obstacle.js';
import { Info } from '/js/wasteful-game/info.js';
import { Direction } from '/js/wasteful-game/direction.js';
import { Zombie } from '/js/wasteful-game/zombie.js';
import { ZombieSpawner } from '/js/wasteful-game/zombie-spawner.js';
import { Player } from '/js/wasteful-game/player.js';
import { Consumable } from '/js/wasteful-game/consumable.js';
import { Grid } from '/js/wasteful-game/grid.js';
import { Background } from '/js/wasteful-game/background.js';
import { ItemEffect } from '/js/wasteful-game/item-effect.js';
import { ExitTile } from '/js/wasteful-game/exit-tile.js';

const wastefulGray = '#cccccc';
const hangryRed = '#ff0000';
const wastefulInfoWidth = 126;

export class Wasteful {
  constructor(canvas) {
    this._canvas = canvas;
    this._context = canvas.getContext('2d');
    this._isRunning = false;
    this._isGameOver = false;
  }

  startGame(displayName) {
    if (this._isRunning) return;

    this._turnNumber = 0;
    this._grid = new Grid(this._canvas, this._context);
    this._info = new Info(this._canvas, this._context, displayName);
    this._exit = new ExitTile(this._grid);
    this._player = new Player(this._grid);
    this._actors = [new Zombie(this._grid)];
    this._items = [new Consumable(this._grid, new ItemEffect(1,5), '/images/ZedChatter/Taco-0.png')];
    this._createObstacles();
    this._background = new Background(this._context, this._canvas.width - wastefulInfoWidth, this._canvas.height);

    this._isRunning = true;
    window.requestAnimationFrame(() => this._updateFrame());
  }

  movePlayer(direction) {
    this._player.move(new Direction(direction));

    this._removeZombies();

    this._actors.forEach(actor => {
      let spawn = actor.takeTurn();
      if (spawn) {
        this._actors.push(spawn);
      }
    });

    if (this._player.health <= 0) {
      this._isGameOver = true;
    }

    this._addZombieSpawners();

    this._turnNumber++;
  }

  _updateFrame() {
    if (this._isGameOver) {
      this._drawGameOver();
      let delay = ms => new Promise(r => setTimeout(r, ms));
      delay(5000).then(() => this._endGame());
    } else {
      this._clearCanvas();
      this._drawBackground();
      this._grid.draw();
      this._info.draw(this._player);
      window.requestAnimationFrame(() => this._updateFrame());
    }
  }

  _clearCanvas() {
    this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
  }

  _endGame() {
    this._isRunning = false;
    this._isGameOver = false;
    this._clearCanvas();
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

  _removeZombies() {
    this._actors = this._actors.filter(zombie => !zombie.isKilled);
  }

  _addZombieSpawners() {
    if (this._turnNumber % 6 === 0 && this._turnNumber > 0) {
      let location = this._grid.getRandomOpenLocation();
      this._actors.push(new ZombieSpawner(this._grid, location.x, location.y));
    }
  }
}
