import { ItemEffectType } from '/js/wasteful-game/entity/items/item.js';
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
   * @param {Wasteful} game
   * @param {Sprite|null} sprite
   * @param {ItemType} type
   * @param {ItemPickupType} pickupType
   * @param {ItemEffectType|Array<ItemEffectType>|null} effectTypes
   * @param {number} uses
   * @param {Array<{ItemEffectType: number}>} effectPointMap
   */
  constructor(game, sprite, type, pickupType, effectTypes, uses, effectPointMap) {
    super('exit',game, sprite, type, pickupType, effectTypes, uses, effectPointMap);

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
