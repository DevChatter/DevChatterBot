import { MetaData } from '/js/wasteful-game/metadata.js';

export class Background {
  /**
   * @param {CanvasRenderingContext2D} context
   * @param {number} width
   * @param {number} height
   */
  constructor(context, width, height) {
    this._context = context;
    this._backgroundImages = this._getBackgroundImages();
    this._tileGrid = this._createRandomGrid(width, height, this._backgroundImages.length);
  }

  /**
   * @public
   */
  drawBackground() {
    this._tileGrid.forEach(
      (col, colIndex) => col.forEach(
        (imgIndex, rowIndex) => {
          let image = this._backgroundImages[imgIndex];
          this._context.drawImage(image, MetaData.tileSize * colIndex, MetaData.tileSize * rowIndex);
        }));
  }

  /**
   * @private
   */
  _getBackgroundImages() {
    let img1 = new Image();
    img1.src = '/images/ZedChatter/RockyGroundTile-0.png';
    let img2 = new Image();
    img2.src = '/images/ZedChatter/RockyGroundTile-1.png';
    let img3 = new Image();
    img3.src = '/images/ZedChatter/RockyGroundTile-2.png';
    return [img1, img2, img2, img3, img3, img3];
  }

  /**
   * @private
   * @param {number} width
   * @param {number} height
   * @param {number} choiceCount
   */
  _createRandomGrid(width, height, choiceCount) {
    let w = width / MetaData.tileSize;
    let h = height / MetaData.tileSize;
    const result = [];
    for (let i = 0; i < w; i++) {
      result[i] = [];
      for (let j = 0; j < h; j++) {
        result[i][j] = Math.floor(Math.random() * choiceCount);
      }
    }
    return result;
  }
}
