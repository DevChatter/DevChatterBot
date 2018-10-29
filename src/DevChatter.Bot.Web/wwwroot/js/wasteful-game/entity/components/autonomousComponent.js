import { Component } from '/js/wasteful-game/entity/components/component.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const AutonomousComponentMessages = Object.freeze({
  TURN: Symbol('entity.autonomous.component.turn'),
});

export class AutonomousComponent extends Component {
  /**
   * @param {Wasteful} game
   * @param {Entity} entity
   * @param {number} turns
   */
  constructor(game, entity, turns) {
    super(game, entity);

    this._turns = turns;
    this._currentTurn = 0;
  }

  /**
   * @public
   *  @returns {number}
   */
  get turns() {
    return this._turns;
  }

  /**
   * @public
   *  @returns {number}
   */
  get currentTurn() {
    return this._currentTurn;
  }

  /**
   * @public
   */
  resetCurrentTurn() {
    this._currentTurn = 0;
  }

  /**
   * @public
   * @returns {boolean}
   */
  takeTurn() {
    if (this._currentTurn < this._turns - 1) {
      this._currentTurn++;
      return false;
    }
    this.resetCurrentTurn();

    const event = Mediator.publish(AutonomousComponentMessages.TURN, {
      source: this.entity,
    });

    return event.process;
  }
}
