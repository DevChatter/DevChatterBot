import { Entity } from '/js/wasteful-game/entity/entity.js';
import { MovableComponent, MovableComponentMessages } from '/js/wasteful-game/entity/components/movableComponent.js';
import { AttackableComponent } from '/js/wasteful-game/entity/components/attackableComponent.js';
import { AttackingComponent } from '/js/wasteful-game/entity/components/attackingComponent.js';
import { AutonomousComponent, AutonomousComponentMessages } from '/js/wasteful-game/entity/components/autonomousComponent.js';
import { SpawnableComponent } from '/js/wasteful-game/entity/components/spawnableComponent.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

export class Enemy extends Entity {
  /**
   * @param {Wasteful} game
   * @param {Sprite|null} sprite
   * @param {number} steps
   * @param {number} turns
   * @param {number} health
   * @param {boolean} diagonal
   * @param {number} range
   * @param {number} damage
   */
  constructor(game, sprite, steps, turns, health, diagonal, range, damage) {
    if (new.target === Enemy) {
      throw new TypeError('Cannot construct Enemy instances directly.');
    }

    super(game, sprite);

    this._movableComponent = new MovableComponent(game, this, steps);
    this._autonomousComponent = new AutonomousComponent(game, this, turns);
    this._attackableComponent = new AttackableComponent(game, this, health);
    this._attackingComponent = new AttackingComponent(game, this, diagonal, range, damage);

    this.addComponent([
      this._movableComponent,
      this._autonomousComponent,
      this._attackableComponent,
      this._attackingComponent,
    ]);

    Mediator.subscribe(AutonomousComponentMessages.TURN, this._onTurn.bind(this));
    Mediator.subscribe(MovableComponentMessages.BLOCKED, this._onMoveBlocked.bind(this));
  }

  /**
   * @private
   * @param {Event} event
   */
  _onTurn(event) {
    if(event.args.source !== this) {
      return;
    }

    const spawnableComponent = this.getComponent(SpawnableComponent);
    // If the entity has a spawn animation, wait until it's finished
    if(spawnableComponent !== null && !spawnableComponent.isFinished) {
      this._spawnableComponent.next();
      return;
    }

    let player = this.game.player;
    if(this._attackingComponent.canAttack(player)) {
      this._attackingComponent.attack(player);
    } else {
      this._movableComponent.moveTowardsTarget(player, false);
    }
  }

  /**
   * @private
   * @param {Event} event
   */
  _onMoveBlocked(event) {
    if (event.args.source === this) {
      event.args.blockedBy.filter(target => {
        return !(target instanceof Enemy);
      }).forEach(target => {
        const tAttackingComponent = target.getComponent(AttackingComponent);
          if(tAttackingComponent !== null) {
            tAttackingComponent.attack(this);
          }
      });
    }
  }
}
