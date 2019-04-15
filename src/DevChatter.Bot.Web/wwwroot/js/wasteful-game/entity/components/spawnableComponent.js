import { Component } from '/js/wasteful-game/entity/components/component.js';
import { AttackableComponent } from '/js/wasteful-game/entity/components/attackableComponent.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const SpawnableComponentMessages = Object.freeze({
  UPDATE: Symbol('entity.spawnable.component.update'),
});

export class SpawnableComponent extends Component {
  /**
   * @param {Wasteful} game
   * @param {Entity} entity
   * @param {Array<{sprite: Sprite|null, [walkable]: boolean, [attackable]: boolean}>} steps
   */
  constructor(game, entity, steps) {
    super(game, entity);

    this._steps = steps.concat(this._getDefaultData());
    this._step = 1;
    this._update(this.currentStepData);
  }

  /**
   * @public
   * @returns {boolean}
   */
  get isFinished() {
    return this.remainingSteps <= 0;
  }

  /**
   * @public
   * @returns {number}
   */
  get currentStep() {
    return this._step;
  }

  /**
   * @public
   * @returns {{sprite: Sprite|null, [walkable]: boolean, [attackable]: boolean}}
   */
  get currentStepData() {
    return this._steps[this._step - 1];
  }

  /**
   * @public
   * @returns {number}
   */
  get remainingSteps() {
    return this.totalSteps - this._step;
  }

  /**
   * @public
   * @returns {number}
   */
  get totalSteps() {
    return this._steps.length;
  }

  /**
   * @public
   */
  next() {
    this._step++;
    this._step = Math.min(this.totalSteps, this._step);
    this._executeUpdate();
  }

  /**
   * @public
   */
  previous() {
    this._step--;
    this._step = Math.max(1, this._step);
    this._executeUpdate();
  }

  /**
   * @private
   */
  _executeUpdate() {
    const event = Mediator.publish(SpawnableComponentMessages.UPDATE, {
      source: this.entity,
      currentStep: this.currentStep,
      remainingSteps: this.remainingSteps,
      stepData: this.currentStepData,
    });

    if(event.process) {
      this._update(event.args.stepData);
    }
  }

  /**
   * @private
   * @returns {{sprite: Sprite|null, [walkable]: boolean, [attackable]: boolean}}
   */
  _getDefaultData() {
    const data = {
      sprite: this.entity.sprite,
      walkable: this.entity.walkable,
    };

    /** @type {AttackableComponent} */
    const attackableComponent = this.entity.getComponent(AttackableComponent);
    if (attackableComponent !== null) {
      data.attackable = attackableComponent.invulnerable;
    }

    return data;
  }

  /**
   * @private
   * @param {{sprite: Sprite|null, [walkable]: boolean, [attackable]: boolean}} stepData
   */
  _update(stepData) {
    this.entity.sprite = stepData.sprite;

    if(typeof stepData.walkable !== 'undefined') {
      this.entity.walkable = stepData.walkable;
    }

    if(typeof stepData.attackable !== 'undefined') {
      /** @type {AttackableComponent} */
      const attackableComponent = this.entity.getComponent(AttackableComponent);
      if (attackableComponent !== null) {
        attackableComponent.invulnerable = stepData.attackable;
      }
    }
  }
}
