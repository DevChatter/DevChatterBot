export class Component {
  /**
   * @param {Wasteful} game
   * @param {Entity} entity
   */
  constructor(game, entity) {
    this._game = game;
    this._entity = entity;
  }

  /**
   * @returns {Wasteful}
   */
  get game() {
    return this._game;
  }

  /**
   * @returns {Entity}
   */
  get entity() {
    return this._entity;
  }
}
