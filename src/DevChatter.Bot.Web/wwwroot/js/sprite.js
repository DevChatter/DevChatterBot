function Sprite(imageSrc, x, y, direction, mod) {
  this.image = new Image();
  this.image.src = imageSrc;
  this.x = x || Math.floor((Math.random() * 200) + (1920 / 2 - 100));
  this.y = y || Math.floor((Math.random() * 200) + (1080 / 2 - 100));
  this.direction = direction || this.getRandomDirection();
  this.mod = mod || this.getRandomModifier(); //sets rate of displacement (how fast the sprite is moving)
  this.timesRendered = 0;
};

Sprite.prototype.getRandomDirection = function () {
  var rand = Math.random() * 2 * Math.PI; //get random val between 0 and 2 Pi ~ 0 and 360 degrees
  
  return rand;
};

Sprite.prototype.getRandomModifier = function () {
  var rand = Math.random() * 3 + 1; //get random value between 1 and 4 (float)
  return rand;
};

Sprite.prototype.update = function (canvas) {
  //assuming right is 0 and left is pi rad, though doesn't really matter
  //mod ~ modifier affects magnitude of displacement

  var xRightBorder = canvas.width - this.image.width; //width of canvas - width of image, insures the right border of the image bounces off the right border of the canvas
  var yBottomBorder = canvas.height - this.image.height; //same as above but for bottom of image and canvas
  var xLeftBorder = 0;
  var yTopBorder = 0;

  this.x += this.mod * Math.cos(this.direction); //apply degree to trig function for x displacement
  this.y += this.mod * Math.sin(this.direction); //apply degree to trig function for y displacement

  // screen coords are defined as 0,0 in the top left and max width, height in the bottom right


  //check if x position has moved beyond the left or right border
  if ((this.x < xLeftBorder || this.x > xRightBorder)) {
    // this.direction += Math.PI / 2;
    this.direction = this.getBounceAngle(this.direction, true);
  }

  //check if y position has moved beyond the upper or lower border
  if ((this.y < yTopBorder || this.y > yBottomBorder)) {
    // this.direction += Math.PI / 2;
    this.direction = this.getBounceAngle(this.direction, false);
  }
};

// takes angle of colision and a bool to tell which kind of wall it's bouncing off of
// outputs the incident angle that the sprite will be going in
Sprite.prototype.getBounceAngle = function (angle, hitVerticalWall = true) { 
  var FULL_CIRCLE = 2 * Math.PI;
  var bounceAngle = 0;

  
  //simplified angle calc code by benrick
  if (hitVerticalWall) {
    bounceAngle = Math.PI - angle;
  } else {
    bounceAngle = Math.PI * 2 - angle;
  }

  //if angle has gone passed 360 or below 0, correct this. The trig functions shouldn't care though the logic above is broken if the angle isn't within 0 and 360 (0 and 2pi)
  bounceAngle = bounceAngle > FULL_CIRCLE ? bounceAngle - FULL_CIRCLE: bounceAngle; 
  bounceAngle = bounceAngle < 0 ? bounceAngle + FULL_CIRCLE: bounceAngle;
  return bounceAngle;
};

Sprite.prototype.render = function (ctx) {
  this.timesRendered += 1;
  ctx.drawImage(this.image, this.x, this.y);
};
