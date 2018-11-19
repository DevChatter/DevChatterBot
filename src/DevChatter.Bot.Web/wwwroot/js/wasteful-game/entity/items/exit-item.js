import { ItemEffectType, ItemPickupType, ItemType } from '/js/wasteful-game/entity/items/item.js';
import { EffectItem } from '/js/wasteful-game/entity/items/effect-item.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const ExitItemMessages = Object.freeze({
  EXIT: Symbol('entity.item.exit.exit'),
});

export class ExitItem extends EffectItem {
  /**
   * @param {Wasteful} game wasteful game
   * @param {Sprite|null} sprite sprite object to display
   * @param {ItemEffectType|Array<ItemEffectType>|null} effectTypes effect type(s) of item
   * @param {Array<{ItemEffectType: number}>} effectPointMap array of values for the specified effect(s)
   */
  constructor(game, sprite, effectTypes, effectPointMap) {
    super('exit', game, sprite, ItemType.CONSUMABLE, ItemPickupType.INSTANT, effectTypes, 1, effectPointMap);

    const location = this.game.grid.lowerRightCorner;
    this.setLocation(location.x - 1, Math.floor(location.y / 2));
  }

  /**
   * @private
   */
  _applyEffect() {
    super._applyEffect();

    const event = Mediator.publish(ExitItemMessages.EXIT, {
      source: this.entity,
    });

    if(event.process) {
      this.game.level.next();
    }
  }
}
