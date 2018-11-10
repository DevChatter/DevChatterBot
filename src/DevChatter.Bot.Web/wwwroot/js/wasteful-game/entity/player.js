import { Entity } from '/js/wasteful-game/entity/entity.js';
import { MovableComponent, MovableComponentMessages } from '/js/wasteful-game/entity/components/movableComponent.js';
import { AttackableComponent } from '/js/wasteful-game/entity/components/attackableComponent.js';
import { AttackingComponent } from '/js/wasteful-game/entity/components/attackingComponent.js';
import { Inventory } from '/js/wasteful-game/inventory.js';
import { Sprite } from '/js/wasteful-game/entity/sprite.js';
import { ItemType } from '/js/wasteful-game/entity/items/item.js';
import Mediator from '/js/wasteful-game/helpers/mediator.js';

export class Player extends Entity {
  /**
   * @param {Wasteful} game
   * @param {Array<Item>} startingItems
   */
  constructor(game, startingItems) {
    super(game, new Sprite('/images/ZedChatter/Hat-YellowShirt-Player-Idle-0.png', 1, 1, 100));

    this._movableComponent = new MovableComponent(game, this, 1);
    this._attackableComponent = new AttackableComponent(game, this, 3, 5);
    this._attackingComponent = new AttackingComponent(game, this, false, 1, 1);

    this.addComponent([
      this._movableComponent,
      this._attackableComponent,
      this._attackingComponent,
    ]);

    this.setLocation(2, 2);

    this._points = 0;
    this.inventory = new Inventory(startingItems);

    Mediator.subscribe(MovableComponentMessages.BLOCKED, this._onMove.bind(this));
  }

  /**
   * @public
   * @param {number} amount
   */
  increaseHealth(amount) {
    this._attackableComponent.increaseHealth(amount);
  }

  /**
   * @public
   * @param {number} amount
   */
  increasePoints(amount) {
    this._points += amount;
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
   * @returns {number}
   */
  get points() {
    return this._points;
  }

  /**
   * @private
   * @param {Event} event
   */
  _onMove(event) {
    if(event.args.source !== this) {
      return;
    }

    const targets = this.game.entityManager.getAtLocation(event.args.toLocation).filter(entity => entity.hasComponent(AttackableComponent));
    if(targets.length) {
      const weapons = this.inventory.getByItemType(ItemType.WEAPON);
      let usedWeapon = false;
      if(weapons.length) {
        weapons[0].use();
        usedWeapon = true;
      }
      targets.forEach(target => {
        if(!usedWeapon) {
          const tAttackingComponent = target.getComponent(AttackingComponent);
          if(tAttackingComponent !== null) {
            tAttackingComponent.attack(this);
          }
        }
        this._attackingComponent.attack(target);
      });
    }
  }
}
