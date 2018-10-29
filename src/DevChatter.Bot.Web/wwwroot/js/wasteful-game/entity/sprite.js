export class Sprite {
  /**
   * @param {HTMLImageElement|HTMLCanvasElement|HTMLVideoElement|ImageBitmap|string} image
   * @param {number} width
   * @param {number} height
   * @param {number} zIndex
   */
  constructor(image, width = 1, height = 1, zIndex = 1) {
    this.image = image;
    this.width = width;
    this.height = height;
    this.zIndex = zIndex;

    // TODO: Pathfinding, attacking etc. should take width & height into calculations
  }

  /**
   * @public
   * @returns {HTMLImageElement|HTMLCanvasElement|HTMLVideoElement|ImageBitmap}
   */
  get image() {
    return this._image;
  }

  /**
   * @public
   * @param {HTMLImageElement|HTMLCanvasElement|HTMLVideoElement|ImageBitmap|string} image
   */
  set image(image) {
    if(typeof image === 'string') {
     const imageObject = new Image();
     imageObject.src = image;
      this._image = imageObject;
    } else {
      this._image = image;
    }
  }

  /**
   * @public
   * @returns {number}
   */
  get width() {
    return this._width;
  }

  /**
   * @public
   * @param {number} width
   */
  set width(width) {
    this._width = width;
  }

  /**
   * @public
   * @returns {number}
   */
  get height() {
    return this._height;
  }

  /**
   * @public
   * @param {number} height
   */
  set height(height) {
    this._height = height;
  }

  /**
   * @public
   * @returns {number}
   */
  get zIndex() {
    return this._zIndex;
  }

  /**
   * @public
   * @param {number} zIndex
   */
  set zIndex(zIndex) {
    this._zIndex = zIndex;
  }
}
