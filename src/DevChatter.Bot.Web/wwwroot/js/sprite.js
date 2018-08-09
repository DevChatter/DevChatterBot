function Sprite(imageSrc, x, y, direction) {
  this.image = new Image();
  this.image.src = imageSrc;
  this.x = x || Math.floor((Math.random() * 200) + (1920 / 2 - 100));
  this.y = y || Math.floor((Math.random() * 200) + (1080 / 2 - 100));
  this.direction = direction || this.getRandomDirection();
  this.timesRendered = 0;
};

Sprite.prototype.getRandomDirection = function () {
  var myArray = ['U', 'UR', 'R', 'DR', 'D', 'DL', 'L', 'UL'];
  var rand = myArray[Math.floor(Math.random() * myArray.length)];

  return rand;
};

Sprite.prototype.update = function () {
  switch (this.direction) {
    case 'U':
      this.y -= 2;
      break;
    case 'UR':
      this.x += 2;
      this.y -= 2;
      break;
    case 'R':
      this.x += 2;
      break;
    case 'DR':
      this.x += 2;
      this.y += 2;
      break;
    case 'D':
      this.y += 2;
      break;
    case 'DL':
      this.x -= 2;
      this.y += 2;
      break;
    case 'L':
      this.x -= 2;
      break;
    case 'UL':
      this.x -= 2;
      this.y -= 2;
      break;
  }
};

Sprite.prototype.render = function (ctx) {
  this.timesRendered += 1;
  ctx.drawImage(this.image, this.x, this.y);
};
