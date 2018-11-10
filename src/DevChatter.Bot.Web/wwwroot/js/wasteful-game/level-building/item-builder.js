import { EffectItem } from '/js/wasteful-game/entity/items/effect-item.js';
import { Item, ItemEffectType, ItemPickupType, ItemType } from '/js/wasteful-game/entity/items/item.js';
import { Sprite } from '/js/wasteful-game/entity/sprite.js';

const itemMetaData = [
  {
    name:'Bat',
    imgSrc: '/images/ZedChatter/BaseballBat-1.png',
    itemType: ItemType.WEAPON,
    pickupType: ItemPickupType.INVENTORY,
    effectType: null,
    uses: 1
  },
  {
    name: 'Axe',
    imgSrc: '/images/ZedChatter/Axe-1.png',
    itemType: ItemType.WEAPON,
    pickupType: ItemPickupType.INVENTORY,
    effectType: null,
    uses: 2
  },
  {
    name: 'Sword',
    imgSrc: '/images/ZedChatter/Sword-1.png',
    itemType: ItemType.WEAPON,
    pickupType: ItemPickupType.INVENTORY,
    effectType: null,
    uses: 3
  },
  {
    name: 'Taco',
    imgSrc: '/images/ZedChatter/Taco-0.png',
    itemType: ItemType.CONSUMABLE,
    pickupType: ItemPickupType.INSTANT,
    effectType: [ItemEffectType.HEALTH, ItemEffectType.POINTS],
    uses: 1,
    effectMap: [
      { [ItemEffectType.HEALTH]: 2 },
      { [ItemEffectType.POINTS]: 5 }
    ]
  },
  {
    name: 'Pizza',
    imgSrc: '/images/ZedChatter/Pizza-0.png',
    itemType: ItemType.CONSUMABLE,
    pickupType: ItemPickupType.INSTANT,
    effectType: ItemEffectType.HEALTH,
    uses: 2,
    effectMap: [
      { [ItemEffectType.HEALTH]: 3 }
    ]
  },
  {
    name: 'BlackCan',
    imgSrc: '/images/ZedChatter/BlackCan-0.png',
    itemType: ItemType.CONSUMABLE,
    pickupType: ItemPickupType.INSTANT,
    effectType: [ItemEffectType.HEALTH, ItemEffectType.POINTS],
    uses: 3,
    effectMap: [
      { [ItemEffectType.HEALTH]: 1 },
      { [ItemEffectType.POINTS]: 15 }
    ]
  }
];

export class ItemBuilder {
  /**
   * @param {Wasteful} game
   */
  constructor(game) {
    this._game = game;
  }

  /**
   * @public
   * @param {number} levelNumber
   */
  getItemsForLevel(levelNumber) {
    let itemType = levelNumber % 2 === 0 ? ItemType.CONSUMABLE : ItemType.WEAPON;
    return this._createItemOfType(itemType);
  }

  /**
   * @public
   * @param {string} name
   * @param {number} uses
   */
  createItemByName(name, uses) {
    let itemData = itemMetaData.filter(item => item.name === name)[0];

    if (uses !== undefined) {
      itemData.uses = uses;
    }

    return this._createItemFromItemData(itemData);
  }

  /**
   * @private
   * @param {Array} choices
   */
  _pickRandom(choices) {
    return choices[Math.floor(Math.random() * choices.length)];
  }

  /**
   * @param {ItemType.CONSUMABLE|ItemType.WEAPON} itemType
   * @private
   */
  _createItemOfType(itemType) {
    let itemData = this._pickRandom(itemMetaData.filter(item => item.itemType === itemType));

    return this._createItemFromItemData(itemData);
  }

  _createItemFromItemData(itemData) {
    let item;
    if (itemData.effectMap !== undefined) {
      item = new EffectItem(itemData.name, this._game, new Sprite(itemData.imgSrc, 1, 1, 1), itemData.itemType, itemData.pickupType, itemData.effectType, itemData.uses, itemData.effectMap);
    } else {
      item = new Item(itemData.name, this._game, new Sprite(itemData.imgSrc, 1, 1, 1), itemData.itemType, itemData.pickupType, itemData.effectType, itemData.uses);
    }
    return item;
  }
}
