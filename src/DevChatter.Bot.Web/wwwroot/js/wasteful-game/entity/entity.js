export class Entity {
  /**
   * @param {Wasteful} game
   * @param {Sprite|null} sprite
   * @param {boolean} walkable
   */
  constructor(game, sprite = null, walkable = false) {
    this._components = [];
    this._game = game;
    this.sprite = sprite;
    this.walkable = walkable;
    this.setLocation(0, 0);
  }

  /**
   * @public
   * @param {function|string} type
   * @returns {Component|null}
   */
  getComponent(type) {
    const isFunction = typeof type === 'function';
    for(let i = 0, l = this._components.length; i < l; i++) {
      if((isFunction && this._components[i] instanceof type) ||
        (!isFunction && this._components[i].constructor.name === type.constructor.name)) {
        return this._components[i];
      }
    }
    return null;
  }

  /**
   * @public
   * @param {function|string} type
   * @returns {boolean}
   */
  hasComponent(type) {
    return this.getComponent(type) !== null;
  }

  /**
   * @public
   * @param {Component|Array<Component>} component
   */
  addComponent(component) {
    if(Array.isArray(component)) {
      component.forEach((c) => {
        this.addComponent(c)
      });
    } else {
      if(this.hasComponent(component)) {
        throw new TypeError('Entity already contains a component of this type.');
      }
      this._components.push(component);
    }
  }

  /**
   * @public
   */
  spawn() {
    this.game.entityManager.add(this);
  }

  /**
   * @public
   */
  destroy() {
    this.game.entityManager.remove(this);
  }

  /**
   * @public
   * @param {number|{x: number, y: number}} x
   * @param {number} [y]
   */
  setLocation(x, y) {
    this.location = typeof y === 'undefined' ? x : {x, y};
  }

  /**
   * @public
   * @param {{x: number, y: number}} location
   */
  set location(location) {
    this._x = location.x;
    this._y = location.y;
  }

  /**
   * @public
   * @returns {{x: number, y: number}}
   */
  get location() {
    return { x: this._x , y : this._y };
  }

  /**
   * @public
   * @returns {boolean}
   */
  get walkable() {
    return this._walkable;
  }

  /**
   * @public
   * @param {boolean} walkable
   */
  set walkable(walkable) {
    this._walkable = walkable;
  }

  /**
   * @public
   * @returns {Sprite|null}
   */
  get sprite() {
    return this._sprite;
  }

  /**
   * @public
   * @param {Sprite|null} sprite
   */
  set sprite(sprite) {
    this._sprite = sprite;
  }

  /**
   * @public
   * @returns {Wasteful}
   */
  get game() {
    return this._game;
  }

  /**
   * @public
   * @returns {number}
   */
  get zIndex() {
    return this.sprite !== null ? this.sprite.zIndex : Number.MAX_SAFE_INTEGER;
  }
}
