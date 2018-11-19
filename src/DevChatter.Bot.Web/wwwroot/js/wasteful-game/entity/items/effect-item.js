import { Item, ItemEffectType } from '/js/wasteful-game/entity/items/item.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const EffectItemMessages = Object.freeze({
  APPLY: Symbol('entity.effect_item.apply'),
});

export class EffectItem extends Item {
  /**
   * @param {string} name item name
   * @param {Wasteful} game wasteful game
   * @param {Sprite|null} sprite sprite for displaying entity
   * @param {ItemType} type type of the item
   * @param {ItemPickupType} pickupType type defining how it's picked up
   * @param {ItemEffectType|Array<ItemEffectType>|null} effectTypes effect(s) of the item
   * @param {number} uses number of uses it has
   * @param {Array<{ItemEffectType: number}>} effectPointMap array mapping of the values of the effect(s)
   */
  constructor(name, game, sprite, type, pickupType, effectTypes, uses, effectPointMap) {
    super(name, game, sprite, type, pickupType, effectTypes, uses);

    this._effectPointMap = effectPointMap;
  }

  /**
   * @protected
   */
  _applyEffect() {
    super._applyEffect();
    const player = this.game.player;
    this.effectTypes.forEach((effectType) => {
      const amount = this._getAmountForEffect(effectType);
      if(amount !== null) {
        const event = Mediator.publish(EffectItemMessages.APPLY, {
          source: this.entity,
          effectType: effectType,
          amount: amount
        });
        if(event.process) {
          switch (event.args.effectType) {
            case ItemEffectType.HEALTH:
              player.increaseHealth(event.args.amount);
              break;
            case ItemEffectType.POINTS:
              player.increasePoints(event.args.amount);
              break;
            case ItemEffectType.MONEY:
              player.increaseMoney(event.args.amount);
              break;
          }
        }
      }
    });
  }

  /**
   * @protected
   * @param {ItemEffectType} effectType effect type to filter on
   * @returns {number|null} value of the effect
   */
  _getAmountForEffect(effectType) {
    for(let i = 0, l = this._effectPointMap.length; i < l; i++) {
      if(this._effectPointMap[i].hasOwnProperty(effectType)) {
        return this._effectPointMap[i][effectType];
      }
    }

    return null;
  }
}
