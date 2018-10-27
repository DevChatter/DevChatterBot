export class Inventory {
  constructor() {
    this.items = [];
  }

  addItem(item) {
    this.items.push(item);
  }

  removeItem(item) {
    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
  }

  useItem(key) {
    this.items[key].use();
  }

  getWeapons() {
    return this.items.filter(item => item.isWeapon);
  }
}
