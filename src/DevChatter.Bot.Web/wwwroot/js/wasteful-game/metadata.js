const wastefulInfoWidth = 252;
const tileSize = 42;
const endTypeDied = 'died';
const endTypeEscaped = 'escaped';

export class MetaData {

  /**
   * @public
   * @returns {number}
   */
  static get wastefulInfoWidth() {
    return wastefulInfoWidth;
  }

  /**
   * @public
   * @returns {number}
   */
  static get tileSize() {
    return tileSize;
  }
}

export class EndTypes {

  /**
  * @public
  * @returns {string}
  */
  static get died() {
    return endTypeDied;
  }

  /**
  * @public
  * @returns {string}
  */
  static get escaped() {
    return endTypeEscaped;
  }
}
