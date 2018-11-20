import { Component } from '/js/wasteful-game/entity/components/component.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

/**
 * @readonly
 * @enum {Symbol}
 */
export const MovableComponentMessages = Object.freeze({
  MOVE: Symbol('entity.movable.component.move'),
  BLOCKED: Symbol('entity.movable.component.blocked'),
});

export class MovableComponent extends Component {
  /**
   * @param {Wasteful} game
   * @param {Entity} entity
   * @param {number} steps
   */
  constructor(game, entity, steps = 1) {
    super(game, entity);

    this._steps = steps;
  }

  /**
   * @public
   * @returns {number}
   */
  get steps() {
    return this._steps;
  }

  /**
   * @public
   * @param {Entity} target
   * @param {boolean} advanced
   */
  moveTowardsTarget(target, advanced = false) {
    const targetLocation = target.location;
    this.moveTowardsLocation(targetLocation.x, targetLocation.y, advanced);
  }

  /**
   * @public
   * @param {number} x
   * @param {number} y
   * @param {boolean} advanced
   */
  moveTowardsLocation(x, y, advanced = false) {
    if(advanced) {
      let path = this.game.grid.findPath(this.entity.location, { x, y });
      if (path.length > 0) {
        const firstStep = path[0];
        this._move(firstStep.x, firstStep.y);
      } else {
        // Don't go anywhere. No path to target.
      }
    } else {
      const location = this.entity.location;
      for(let i = 0; i < this.steps; i++) {
        if (x < location.x) {
          this.moveLeft();
        } else if (x > location.x) {
          this.moveRight();
        } else if (y < location.y) {
          this.moveUp();
        } else if (y > location.y) {
          this.moveDown();
        }
      }
    }
  }

  /**
   * @public
   * @param {string} direction
   */
  move(direction) {
    switch (direction.toLowerCase()) {
      case 'left':
        this.moveLeft();
        break;
      case 'up':
        this.moveUp();
        break;
      case 'down':
        this.moveDown();
        break;
      case 'right':
        this.moveRight();
        break;
    }
  }

  /**
   * @public
   */
  moveLeft() {
    const location = this.entity.location;
    return this._move(location.x - 1, location.y);
  }

  /**
   * @public
   */
  moveRight() {
    const location = this.entity.location;
    return this._move(location.x + 1, location.y);
  }

  /**
   * @public
   */
  moveUp() {
    const location = this.entity.location;
    return this._move(location.x, location.y - 1);
  }

  /**
   * @public
   */
  moveDown() {
    const location = this.entity.location;
    return this._move(location.x, location.y + 1);
  }

  /**
   * @private
   * @param {number} x
   * @param {number} y
   */
  _move(x, y) {
    let result = false;
    const toLocation = { x, y };
    if (this.game.grid.canMoveTo(x, y)) {
      const event = Mediator.publish(MovableComponentMessages.MOVE, {
        source: this.entity,
        fromLocation: this.entity.location,
        toLocation: toLocation,
      });
      if(event.process) {
        this.entity.setLocation(event.args.toLocation.x, event.args.toLocation.y);
        result = true;
      }
    } else {
      Mediator.publish(MovableComponentMessages.BLOCKED, {
        source: this.entity,
        fromLocation: this.entity.location,
        toLocation: toLocation,
        blockedBy: this.game.entityManager.getAtLocation(toLocation)
      });
    }
    return result;
  }
}
