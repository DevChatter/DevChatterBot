import { Info } from '/js/wasteful-game/info.js';
import { Direction } from '/js/wasteful-game/direction.js';
import { Player } from '/js/wasteful-game/player.js';
import { Grid } from '/js/wasteful-game/grid.js';
import { Background } from '/js/wasteful-game/background.js';
import { ExitTile } from '/js/wasteful-game/exit-tile.js';
import { Level } from '/js/wasteful-game/level.js';
import { MetaData } from '/js/wasteful-game/metadata.js';

const wastefulGray = '#cccccc';
const hangryRed = '#ff0000';

export class Wasteful {
  constructor(canvas, hub) {
    this._canvas = canvas;
    this._hub = hub;
    this._context = canvas.getContext('2d');
    this._isRunning = false;
    this._isGameOver = false;
  }

  startGame(displayName) {
    if (this._isRunning) return;

    this._levelNumber = 1;
    this._grid = new Grid(this._canvas, this._context);
    this._info = new Info(this._canvas, this._context, displayName);
    this._exit = new ExitTile(this, this._grid);
    this._player = new Player(this._grid);
    this._background = new Background(this._context, this._canvas.width - MetaData.wastefulInfoWidth, this._canvas.height);
    this._level = new Level(this._grid, this._levelNumber);
    this._isRunning = true;
    window.requestAnimationFrame(() => this._updateFrame());
  }

  movePlayer(direction) {
    this._player.move(new Direction(direction));

    this._level.update();

    if (this._player.health <= 0) {
      this._isGameOver = true;
    }
  }

  exitLevel() {
    this._levelNumber++;
    let exitLocation = this._exit.location;
    this._player.setNewLocation(0, exitLocation.y);

    this._grid.clearSprites();

    this._grid.addSprite(this._player);
    this._grid.addSprite(this._exit);

    this._level = new Level(this._grid, this._levelNumber);
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

    // TODO: Organize data better, so it's not coming from separate objects.
    this._hub.invoke("GameEnd", this._player.points, this._info._playerName, 'died', this._levelNumber).catch(err => console.error(err.toString()));
  }

  _drawGameOver() {
    this._context.fillStyle = hangryRed;
    this._context.font = "128px Arial";
    this._context.fillText('Game Over', 20, this._canvas.height - 10);
  }

  _drawBackground() {
    this._background.drawBackground();

    this._context.fillStyle = wastefulGray;
    this._context.fillRect(this._canvas.width - MetaData.wastefulInfoWidth, 0, MetaData.wastefulInfoWidth, this._canvas.height);
  }
}
