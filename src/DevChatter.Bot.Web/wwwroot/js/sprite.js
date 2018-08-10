function Sprite(imageSrc, x, y, direction) {
  this.image = new Image();
  this.image.src = imageSrc;
  this.x = x || Math.floor((Math.random() * 200) + (1920 / 2 - 100));
  this.y = y || Math.floor((Math.random() * 200) + (1080 / 2 - 100));
  this.direction = direction || this.getRandomDirection();
  this.timesRendered = 0;
};

Sprite.prototype.getRandomDirection = function () {
  var rand = Math.random() * 2 * Math.PI //get random val between 0 and 2 Pi ~ 0 and 360 degrees
  
  return rand;
};

Sprite.prototype.update = function () {
  var mod = 2
  //assuming right is 0 and left is pi rad, though doesn't really matter
  //mod ~ modifier affects magnitude of displacement
  this.x += mod * Math.cos(this.direction) //apply degree to trig function for x displacement
  this.y += mod * Math.sin(this.direction) //apply degree to trig function for y displacement
};

Sprite.prototype.render = function (ctx) {
  this.timesRendered += 1;
  ctx.drawImage(this.image, this.x, this.y);
};
