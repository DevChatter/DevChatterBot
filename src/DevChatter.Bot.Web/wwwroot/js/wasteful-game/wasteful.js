import { Info } from '/js/wasteful-game/info.js';
import { Grid } from '/js/wasteful-game/grid.js';
import { Background } from '/js/wasteful-game/background.js';
import { Level } from '/js/wasteful-game/level.js';
import { ItemBuilder } from '/js/wasteful-game/level-building/item-builder.js';
import { MetaData, EndTypes } from '/js/wasteful-game/metadata.js';
import { Player } from '/js/wasteful-game/entity/player.js';
import { MovableComponent } from '/js/wasteful-game/entity/components/movableComponent.js';
import { AttackableComponent } from '/js/wasteful-game/entity/components/attackableComponent.js';
import { EntityManager } from '/js/wasteful-game/entityManager.js';

const wastefulGray = '#cccccc';
const hangryRed = '#ff0000';

export class Wasteful {
  /**
   * @param {HTMLCanvasElement} canvas
   * @param {object} hub
   */
  constructor(canvas, hub) {
    this._mouseDownHandle = this._onMouseDown.bind(this);
    this._keyDownHandle = this._onKeyDown.bind(this);
    this._canvas = canvas;
    this._hub = hub;
    this._context = canvas.getContext('2d');
    this._isRunning = false;
    this._isGameOver = false;
    this._endType = '';
    this._lastMouseTarget = null;

    const url = new URL(window.location.href);
    if(url.searchParams.has('autostart')) {
      this._lastMouseTarget = canvas;
      this.startGame({ displayName: url.searchParams.get('name'), userId: url.searchParams.get('userid')});
    }
  }

  /**
   * @public
   * @param {{displayName: string, userId: string}} userInfo
   * @param {Array<{string, number}>} startingItems
   */
  startGame(userInfo, startingItems) {
    if (this._isRunning) {
      return;
    }

    this._isRunning = true;
    this._userInfo = userInfo;
    this._entityManager = new EntityManager();
    this._grid = new Grid(this._entityManager, this._canvas);
    this._info = new Info(this._canvas, this._context, this._userInfo.displayName);

    let itemBuilder = new ItemBuilder(this);
    let items = [];
    if (startingItems !== undefined && startingItems.length > 0) {
      items = startingItems.map(item => itemBuilder.createItemByName(item.name, item.uses));
      items.forEach(item => this._entityManager.add(item));
    }

    this._player = new Player(this, items);

    this._level = new Level(this, this._player, itemBuilder);
    this._background = new Background(this._context, this._canvas.width - MetaData.wastefulInfoWidth, this._canvas.height);

    this._level.next();

    this._animationHandle = window.requestAnimationFrame(() => this._updateFrame());
    this._mouseDownHandle = this._onMouseDown.bind(this);

    document.addEventListener('mousedown', this._mouseDownHandle);
    document.addEventListener('keydown', this._keyDownHandle);
  }

  /**
   * @public
   * @returns {Grid}
   */
  get grid() {
    return this._grid;
  }

  /**
   * @public
   * @returns {EntityManager}
   */
  get entityManager() {
   return this._entityManager;
  }

  /**
   * @public
   * @returns {Player}
   */
  get player() {
    return this._player;
  }

  /**
   * @public
   * @returns {Level}
   */
  get level() {
    return this._level;
  }

  /**
   * @public
   * @param {string} direction direction to move player
   * @param {number} moveNumber number of spaces to move
   */
  movePlayer(direction, moveNumber = 1) {
    if (moveNumber > 0) {
      this._player.getComponent(MovableComponent).move(direction);

      this._level.update();

      if (this._player.getComponent(AttackableComponent).isDead) {
        this._isGameOver = true;
        this._endType = EndTypes.died;
        return;
      }

      //TODO: Stop recursion if going to new level.
      const millisecondsToWait = 200;
      setTimeout(() => this.movePlayer(direction, moveNumber - 1), millisecondsToWait);
    }
  }

  escape() {
    this._isGameOver = true;
    this._endType = EndTypes.escaped;
  }

  /**
   * @private
   */
  _updateFrame() {
    if (this._isGameOver) {
      this._drawGameOver();
      let delay = ms => new Promise(r => setTimeout(r, ms));
      delay(5000).then(() => this._endGame());
    } else {
      this._clearCanvas();
      this._drawBackground();

      this._entityManager.update();
      this._entityManager.all.forEach(entity => {
        const location = entity.location;
        if(entity.sprite !== null) {
          this._context.drawImage(entity.sprite.image, location.x * MetaData.tileSize, location.y * MetaData.tileSize);
        }
      });

      this._info.draw(this._player);
      this._animationHandle = window.requestAnimationFrame(() => this._updateFrame());
    }
  }

  /**
   * @private
   */
  _clearCanvas() {
    this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
  }

  /**
   * @private
   */
  _endGame() {
    document.removeEventListener('mousedown', this._mouseDownHandle);
    document.removeEventListener('keydown', this._keyDownHandle);
    window.cancelAnimationFrame(this._animationHandle);

    // TODO: Organize data better, so it's not coming from separate objects.
    let heldItems = this.player.inventory.items.map(item => ({
        name: item.name,
        uses: item.remainingUses
    }));

    this._hub.invoke('GameEnd', this.player.points, this._userInfo.displayName, this._userInfo.userId, this._endType, this.level.levelNumber, heldItems)
      .catch(err => console.error(err.toString()));

    this._isRunning = false;
    this._isGameOver = false;
    this._clearCanvas();
    this._endType = '';
  }

  /**
   * @private
   */
  _drawGameOver() {
    this._context.fillStyle = hangryRed;
    this._context.font = '128px Arial';
    this._context.fillText('Game Over', 20, this._canvas.height - 10);
  }

  /**
   * @private
   */
  _drawBackground() {
    this._background.drawBackground();

    this._context.fillStyle = wastefulGray;
    this._context.fillRect(this._canvas.width - MetaData.wastefulInfoWidth, 0, MetaData.wastefulInfoWidth, this._canvas.height);
  }

  /**
   * @private
   * @param {object} event
   */
  _onMouseDown(event) {
    this._lastMouseTarget = event.target;
  }

  /**
   * @private
   * @param {object} event
   */
  _onKeyDown(event) {
    if(this._lastMouseTarget !== this._canvas) {
      return;
    }
    switch (event.keyCode) {
      case 38: // up
      case 87: // w
        this.movePlayer('up');
        break;
      case 39: // right
      case 68: // d
        this.movePlayer('right');
        break;
      case 40: // down
      case 83: // s
        this.movePlayer('down');
        break;
      case 37: // left
      case 65: // a
        this.movePlayer('left');
        break;
      case 90: // z
        this._level.next();
      break;
      case 82: // r
        const timestamp = new Date().getTime();
        const url = new URL(window.location.href);
        if(url.searchParams.has('t')) {
          url.searchParams.set('t', timestamp.toString());
        } else {
          url.searchParams.append('t', timestamp.toString());
        }
        if(!url.searchParams.has('autostart')) {
          url.searchParams.append('autostart', 'true');
        }
        if(!url.searchParams.has('name')) {
          url.searchParams.append('name', this._userInfo.displayName);
        }
        if(!url.searchParams.has('userid')) {
          url.searchParams.append('userid', this._userInfo.userId);
        }
        window.location.href = url.toString();
        break;
    }
  }
}
