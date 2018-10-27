export class Inventory {
  constructor() {
    this.items = [];
  }

  addItem(item) {
    this.items.push(item);
  }

  useItem(key) {
    this.items[key].use();
  }
}
