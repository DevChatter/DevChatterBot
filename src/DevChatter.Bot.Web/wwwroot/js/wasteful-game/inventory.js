export class Inventory {
  /**
   * @public
   * @param {Array<Item>} startingItems
   */
  constructor(startingItems) {
    this.items = startingItems || [];
  }

  /**
   * @public
   * @param {Item} item
   */
  addItem(item) {
    this.items.push(item);
  }

  /**
   * @public
   * @param {Item} item
   */
  removeItem(item) {
    const index = this.items.indexOf(item);
    if (index !== -1) {
      this.items.splice(index, 1);
    }
  }

  /**
   * @public
   * @param {number} key
   */
  useItem(key) {
    this.items[key].use();
  }

  /**
   * @public
   * @param {Symbol} itemType
   * @returns {Array<Item>}
   */
  getByItemType(itemType) {
    return this.items.filter(item => item.type === itemType);
  }
}
