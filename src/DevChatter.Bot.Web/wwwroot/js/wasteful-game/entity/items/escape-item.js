import { ItemEffectType, ItemPickupType, ItemType } from '/js/wasteful-game/entity/items/item.js';
import { EffectItem } from '/js/wasteful-game/entity/items/effect-item.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const EscapeItemMessages = Object.freeze({
  ESCAPE: Symbol('entity.item.escape.escape'),
});

export class EscapeItem extends EffectItem {
  /**
   * @param {Wasteful} game wasteful game
   * @param {Sprite|null} sprite sprite object to display
   * @param {ItemEffectType|Array<ItemEffectType>|null} effectTypes effect type(s) of item
   * @param {Array<{ItemEffectType: number}>} effectPointMap array of values for the specified effect(s)
   * @param {string} escapeType type of escape
   */
  constructor(game, sprite, effectTypes, effectPointMap, escapeType) {
    super('escape', game, sprite, ItemType.CONSUMABLE, ItemPickupType.INSTANT, effectTypes, 1, effectPointMap);
    const location = this.game.grid.lowerRightCorner;
    this.setLocation(location.x - 1, Math.ceil(location.y / 2));
    this._escapeType = escapeType;
  }

  /**
   * @private
   */
  _applyEffect() {
    super._applyEffect();

    const event = Mediator.publish(EscapeItemMessages.ESCAPE, {
      source: this.entity,
    });

    if(event.process) {
      this.game.escape(this._escapeType);
    }
  }
}
