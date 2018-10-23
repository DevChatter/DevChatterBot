export class Background {
  constructor(context, width, height) {
    this._context = context;
    this._backgroundImages = Background._getBackgroundImages();
    this._tileSize = 42;
    this._tileGrid = this._createRandomGrid(width, height, this._backgroundImages.length);
  }

  drawBackground() {
    this._tileGrid.forEach(
      (col, colIndex) => col.forEach(
        (imgIndex, rowIndex) => {
          let image = this._backgroundImages[imgIndex];
          this._context.drawImage(image, this._tileSize * colIndex, this._tileSize * rowIndex);
        }));

  }

  static _getBackgroundImages() {
    let img1 = new Image();
    img1.src = '/images/ZedChatter/RockyGroundTile-0.png';
    let img2 = new Image();
    img2.src = '/images/ZedChatter/RockyGroundTile-1.png';
    let img3 = new Image();
    img3.src = '/images/ZedChatter/RockyGroundTile-2.png';
    return [img1, img2, img2, img3, img3, img3];
  }

  _createRandomGrid(width, height, choiceCount) {
    let w = width / this._tileSize;
    let h = height / this._tileSize;
    var result = [];
    for (var i = 0; i < w; i++) {
      result[i] = [];
      for (var j = 0; j < h; j++) {
        result[i][j] = Math.floor(Math.random() * choiceCount);
      }
    }
    return result;
  }

}
