import { Component } from '/js/wasteful-game/entity/components/component.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const AttackableComponentMessages = Object.freeze({
  INCREASE_HEALTH: Symbol('entity.attackable.component.increase_health'),
  TAKE_DAMAGE: Symbol('entity.attackable.component.take_damage'),
});

export class AttackableComponent extends Component {
  /**
   * @param {Wasteful} game
   * @param {Entity} entity
   * @param {number} health
   * @param {number} [maximumHealth]
   */
  constructor(game, entity, health, maximumHealth) {
    super(game, entity);

    this._maximumHealth = typeof maximumHealth === 'undefined' ? health : maximumHealth;
    this.health = health;
    this.invulnerable = false;
  }

  /**
   * @public
   * @param {number} amount
   * @returns {boolean}
   */
  takeDamage(amount) {
    if(!this.canTakeDamage) {
      return false;
    }

    const event = Mediator.publish(AttackableComponentMessages.TAKE_DAMAGE, {
      source: this.entity,
      amount: amount,
    });

    if(event.process) {
      this._health = Math.max(0, this._health - event.args.amount);
    }

    return event.process;
  }

  /**
   * @public
   * @param {number} amount
   * @returns {boolean}
   */
  increaseHealth(amount) {
    const event = Mediator.publish(AttackableComponentMessages.INCREASE_HEALTH, {
      source: this.entity,
      amount: amount,
    });

    if(event.process) {
      this.health = this.health + event.args.amount;
    }

    return event.process;
  }

  /**
   * @public
   * @returns {boolean}
   */
  canTakeDamage() {
    return !this.invulnerable && !this.isDead;
  }

  /**
   * @public
   * @returns {boolean}
   */
  get invulnerable() {
    return this._invulnerable;
  }

  /**
   * @public
   * @param {boolean} invulnerable
   */
  set invulnerable(invulnerable) {
    this._invulnerable = invulnerable;
  }

  /**
   * @public
   * @returns {number}
   */
  get health() {
    return this._health;
  }

  /**
   * @public
   * @param {number} health
   */
  set health(health) {
    this._health = Math.min(this.maximumHealth, health);
  }

  /**
   * @public
   * @returns {number}
   */
  get maximumHealth() {
    return this._maximumHealth;
  }

  /**
   * @public
   * @param {number} maximumHealth
   */
  set maximumHealth(maximumHealth) {
    this._maximumHealth = maximumHealth;
    this.health = this.health;
  }

  /**
   * @public
   * @returns {boolean}
   */
  get isDead() {
    return !this.isAlive;
  }

  /**
   * @public
   * @returns {boolean}
   */
  get isAlive() {
    return this._health > 0;
  }
}
