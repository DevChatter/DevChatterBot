import { MetaData } from '/js/wasteful-game/metadata.js';
import { AttackableComponent } from '/js/wasteful-game/entity/components/attackableComponent.js';

export class Info {
  /**
   * @param {HTMLCanvasElement} canvas
   * @param {CanvasRenderingContext2D} context
   * @param {string} displayName
   */
  constructor(canvas, context, displayName) {
    this._canvas = canvas;
    this._context = context;
    this._leftX = canvas.width - MetaData.wastefulInfoWidth;
    this._topY = 0;
    this._playerName = displayName;
    this._fontSize = 16;
  }

  /**
   * @public
   * @param {Player} player
   */
  draw(player) {
    this._lineNumber = 1;
    this._context.fillStyle = '#000000';
    this._context.font = `${this._fontSize}px Arial`;

    this._writeLine(this._playerName);
    this._writeLine('\u{1F499}'.repeat(player.getComponent(AttackableComponent).health));
    this._writeLine(`Points: ${player.points}`);
    this._writeLine(`Money: 0`);

    this._showItems(player.inventory.items);
  }

  /**
   * @private
   * @param {string} text
   */
  _writeLine(text) {
    this._context.fillText(text, this._leftX + 5, this._topY + 24 * this._lineNumber);
    this._lineNumber++;
  }

  /**
   * @private
   * @param {Array<Item>} items
   */
  _showItems(items) {
    let y = this._canvas.height - MetaData.tileSize;
    items.forEach((item, index) => {
      let x = this._leftX + (MetaData.tileSize * index + 5);

      this._context.rect(x, y - this._fontSize, MetaData.tileSize, MetaData.tileSize + this._fontSize);
      this._context.stroke();

      this._context.drawImage(item.sprite.image, x, y);
      this._context.fillText(item.remainingUses.toString(), x + 10, y);
    });
  }
}
