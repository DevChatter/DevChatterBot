import { Consumable } from '/js/wasteful-game/consumable.js';
import { ItemEffect } from '/js/wasteful-game/item-effect.js';
import { HeldItem } from '/js/wasteful-game/held-item.js';

export class ItemBuilder {
  constructor(grid) {
    this._grid = grid;

    // TODO: Get the Item Data from passed in argument
    this._itemData = this._createStaticData();
  }

  getItemsForLevel(levelNumber) {
    let items = [];

    if (levelNumber % 2 === 0) {
      let data = this._pickRandom(this._itemData.consumables);
      items.push(new Consumable(this._grid, data.itemEffect, data.imgSrc));
    } else {
      let data = this._pickRandom(this._itemData.weapons);
      let heldItem = new HeldItem(this._grid, data.imgSrc, true);
      items.push(heldItem);
    }

    return items;
  }

  _pickRandom(choices) {
    return choices[Math.floor(Math.random() * choices.length)];
  }

  _createStaticData() {
    return {
      consumables: [
        {
          imgSrc: '/images/ZedChatter/Taco-0.png',
          itemEffect: new ItemEffect(1, 5)
        },
        {
          imgSrc: '/images/ZedChatter/Pizza-0.png',
          itemEffect: new ItemEffect(2, 10)
        },
        {
          imgSrc: '/images/ZedChatter/BlackCan-0.png',
          itemEffect: new ItemEffect(3, 25)
        }
      ],
      weapons: [
        {
          imgSrc: '/images/ZedChatter/BaseballBat-1.png'
        },
        {
          imgSrc: '/images/ZedChatter/Axe-1.png'
        },
        {
          imgSrc: '/images/ZedChatter/Sword-1.png'
        }
      ]
    };
  }
}
