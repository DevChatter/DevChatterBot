import { AttackableComponent } from '/js/wasteful-game/entity/components/attackableComponent.js';

export class EntityManager {
  constructor() {
    this._entities = [];
  }

  /**
   * @public
   * @param {Entity} entity entity to add
   */
  add(entity) {
    this._entities.push(entity);
    this._entities.sort((a, b) => a.zIndex < b.zIndex ? -1 : 1);
  }

  /**
   * @public
   * @param {Entity} entity entity to remove (if exists)
   */
  remove(entity) {
    const index = this._entities.indexOf(entity);
    if(index !== -1) {
      this._entities.splice(index, 1);
    }
  }

  /**
   * @public
   */
  clear() {
    this._entities = [];
  }

  /**
   * @public
   * @param {{x: number, y: number}} location location to check
   * @returns {Array<Entity>} all entities at the location
   */
  getAtLocation(location) {
    return this._entities.filter(entity => entity.location.x === location.x && entity.location.y === location.y);
  }

  /**
   * @public
   * @param {function} className class to look for
   * @returns {Entity|undefined} first Entity of the requested type (undefined if none)
   */
  getFirstByClass(className) {
    return this._entities.find(entity => entity instanceof className);
  }

  /**
   * @public
   * @param {function} className class to look for
   * @returns {Array<Entity>} array of Entities of the requested type
   */
  getByClass(className) {
    return this._entities.filter(entity => entity instanceof className);
  }

  /**
   * @public
   */
  update() {
    this._entities = this._entities.filter(entity => {
      if(entity.hasComponent(AttackableComponent)) {
        return entity.getComponent(AttackableComponent).isAlive;
      }
      return true;
    });
  }

  /**
   * @public
   * @returns {number} number of Entities managed by EntityManager
   */
  get count() {
    return this._entities.length;
  }

  /**
   * @public
   * @returns {Array<Entity>} all the Entities managed by EntityManager
   */
  get all() {
    return this._entities;
  }
}
