import { Entity } from '/js/wasteful-game/entity/entity.js';
import { AutonomousComponent, AutonomousComponentMessages } from '/js/wasteful-game/entity/components/autonomousComponent.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const ItemType = Object.freeze({
  WEAPON: Symbol('weapon'),
  CONSUMABLE: Symbol('consumable'),
});

/**
 * @readonly
 * @enum {Symbol}
 */
export const ItemPickupType = Object.freeze({
  INSTANT: Symbol('instant'),
  INVENTORY: Symbol('inventory'),
});

/**
 * @readonly
 * @enum {Symbol}
 */
export const ItemEffectType = Object.freeze({
  HEALTH: Symbol('health'),
  POINTS: Symbol('points'),
});

/**
 * @readonly
 * @enum {Symbol}
 */
export const ItemMessages = Object.freeze({
  PICKUP: Symbol('entity.item.pickup'),
  USE: Symbol('entity.item.use'),
});

export class Item extends Entity {
  /**
   * @param {Wasteful} game
   * @param {Sprite|null} sprite
   * @param {ItemType} type
   * @param {ItemPickupType} pickupType
   * @param {ItemEffectType|Array<ItemEffectType>|null} effectTypes
   * @param {number} uses
   */
  constructor(game, sprite, type, pickupType, effectTypes, uses = 1) {
    super(game, sprite, true);

    this._type = type;
    this._pickupType = pickupType;

    if(effectTypes === null) {
      this._effectTypes = [];
    } else {
      this._effectTypes = Array.isArray(effectTypes) ? effectTypes : [effectTypes];
    }

    this._uses = pickupType === ItemPickupType.INSTANT ? 1 : uses;

    this._autonomousComponent = new AutonomousComponent(game, this, 1);
    this.addComponent([
      this._autonomousComponent,
    ]);

    Mediator.subscribe(AutonomousComponentMessages.TURN, this._onTurn.bind(this));
  }

  /**
   * @public
   */
  use() {
    const event = Mediator.publish(ItemMessages.USE, {
      source: this.entity,
      remainingUses: this.remainingUses
    });

    if(event.process) {
      if(event.args.remainingUses <= 0) {
        return;
      }

      if(this.effectTypes.length) {
        this._applyEffect();
      }
      this._uses--;
      if(this.remainingUses <= 0) {
        this.destroy();
        if(this.pickupType === ItemPickupType.INVENTORY) {
          this.game.player.inventory.removeItem(this);
        }
      }
    }
  }

  /**
   * @private
   * @param {Event} event
   */
  _onTurn(event) {
    if(event.args.source !== this) {
      return;
    }

    const playerLocation = this.game.player.location;
    const entityLocation = this.location;

    if(playerLocation.x === entityLocation.x && playerLocation.y === entityLocation.y) {
      const event = Mediator.publish(ItemMessages.PICKUP, {
        source: this.entity,
      });

      if(event.process) {
        this._onPickup();
        this.destroy();
      }
    }
  }

  /**
   * @private
   */
  _onPickup() {
    if(this._pickupType === ItemPickupType.INVENTORY) {
      this.game.player.inventory.addItem(this);
    } else if (this._pickupType === ItemPickupType.INSTANT) {
      this.use();
    }
  }

  /**
   * @protected
   */
  _applyEffect() {

  }

  /**
   * @public
   * @returns {number}
   */
  get remainingUses() {
    return this._uses;
  }

  /**
   * @public
   * @returns {ItemType}
   */
  get type() {
    return this._type;
  }

  /**
   * @public
   * @returns {ItemPickupType}
   */
  get pickupType() {
    return this._pickupType;
  }

  /**
   * @public
   * @returns {Array<ItemEffectType>}
   */
  get effectTypes() {
    return this._effectTypes;
  }
}
