import { Component } from '/js/wasteful-game/entity/components/component.js';
import { AttackableComponent } from '/js/wasteful-game/entity/components/attackableComponent.js';
import { SpawnableComponent } from '/js/wasteful-game/entity/components/spawnableComponent.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const AttackingComponentMessages = Object.freeze({
  ATTACK: Symbol('entity.attacking.component.attack'),
});

export class AttackingComponent extends Component {
  /**
   * @param {Wasteful} game
   * @param {Entity} entity
   * @param {boolean} diagonal
   * @param {number} range
   * @param {number} damage
   */
  constructor(game, entity, diagonal = false, range = 1, damage = 1) {
    super(game, entity);

    this._diagonal = diagonal;
    this._range = range;
    this._damage = damage;
  }

  /**
   * @public
   * @param {Entity} target
   * @returns {boolean}
   */
  attack(target) {
    if(!this.canAttack(target)) {
      return false;
    }

    const event = Mediator.publish(AttackingComponentMessages.ATTACK, {
      source: this.entity,
      target: target,
      damage: this.damage,
    });
    if(event.process) {
      /** @type {AttackableComponent} */
      const attackableComponent = event.args.target.getComponent(AttackableComponent);
      if(attackableComponent !== null) {
        attackableComponent.takeDamage(event.args.damage);
        return true;
      }
    }

    return false;
  }

  /**
   * @public
   * @param {Entity} target
   * @returns {boolean}
   */
  canAttack(target) {
    /** @type {AttackableComponent} */
    const tAttackableComponent = target.getComponent(AttackableComponent);
    const attackableComponent = this.entity.getComponent(AttackableComponent);
    const spawnableComponent = this.entity.getComponent(SpawnableComponent);

    return (spawnableComponent === null || spawnableComponent.isFinished) &&
      (attackableComponent === null || attackableComponent.isAlive) &&
      tAttackableComponent !== null && tAttackableComponent.canTakeDamage() && this.isInRange(target);
  }

  /**
   * @public
   * @param {Entity} target
   * @returns {boolean}
   */
  isInRange(target) {
    if(this.canAttackDiagonally) {
      return this._chebyshevDistance(target) <= this.range;
    }

    const straightDistance = this._straightDistance(target);
    return straightDistance !== -1 && straightDistance <= this.range;
  }

  /**
   * @public
   * @returns {boolean}
   */
  get canAttackDiagonally() {
    return this._diagonal;
  }

  /**
   * @public
   * @returns {number}
   */
  get range() {
    return this._range;
  }

  /**
   * @public
   * @returns {number}
   */
  get damage() {
    return this._damage;
  }

  /**
   * @private
   * @param {Entity} target
   * @returns {number}
   */
  _straightDistance(target) {
    const targetLocation = target.location;
    const entityLocation = this.entity.location;

    if(targetLocation.x === entityLocation.x) {
      return Math.abs(targetLocation.y - entityLocation.y);
    } else if(targetLocation.y === entityLocation.y) {
      return Math.abs(targetLocation.x - entityLocation.x);
    }

    // Target is not on the same x or y axis
    return -1;
  }

  /**
   * @private
   * @param {Entity} target
   * @returns {number}
   */
  _chebyshevDistance(target) {
    const targetLocation = target.location;
    const entityLocation = this.entity.location;

    return Math.max(Math.abs(targetLocation.x - entityLocation.x), Math.abs(targetLocation.y - entityLocation.y));
  }

  /**
   * @private
   * @param {Entity} target
   * @returns {number}
   */
  _manhattanDistance(target) {
    const targetLocation = target.location;
    const entityLocation = this.entity.location;

    return Math.abs(targetLocation.x - entityLocation.x) + Math.abs(targetLocation.y - entityLocation.y);
  }

  /**
   * @private
   * @param {Entity} target
   * @returns {number}
   */
  _euclideanDistance(target) {
    const targetLocation = target.location;
    const entityLocation = this.entity.location;

    return Math.hypot(targetLocation.x - entityLocation.x, targetLocation.y - entityLocation.y);
  }
}
