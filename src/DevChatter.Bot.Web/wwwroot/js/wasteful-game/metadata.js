const wastefulInfoWidth = 252;
const tileSize = 42;
const endTypeDied = 'died';
const endTypeEscaped = 'escaped';

export class MetaData {

  /**
   * @public
   * @returns {number} wastefulInfoWidth
   */
  static get wastefulInfoWidth() {
    return wastefulInfoWidth;
  }

  /**
   * @public
   * @returns {number} tileSize
   */
  static get tileSize() {
    return tileSize;
  }
}

export class EndTypes {

  /**
  * @public
  * @returns {string} died
  */
  static get died() {
    return endTypeDied;
  }

  /**
  * @public
  * @returns {string} escaped
  */
  static get escaped() {
    return endTypeEscaped;
  }
}
