export class Inventory {
  /**
   * @public
   * @param {Array<Item>} startingItems initial items for the inventory
   */
  constructor(startingItems) {
    this.items = startingItems || [];
  }

  /**
   * @public
   * @param {Item} item item to add
   */
  addItem(item) {
    this.items.push(item);
  }

  /**
   * @public
   * @param {Item} item item to remove (if exists)
   */
  removeItem(item) {
    const index = this.items.indexOf(item);
    if (index !== -1) {
      this.items.splice(index, 1);
    }
  }

  /**
   * @public
   * @param {number} key index of item to use
   */
  useItem(key) {
    this.items[key].use();
  }

  /**
   * @public
   * @param {Symbol} itemType type of item
   * @returns {Array<Item>} items of requested type
   */
  getByItemType(itemType) {
    return this.items.filter(item => item.type === itemType);
  }
}
