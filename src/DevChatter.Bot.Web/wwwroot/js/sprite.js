//import { debug } from "util";

function Sprite(imageSrc, x, y, direction, mod) {
  this.image = new Image();
  this.image.src = imageSrc;
  this.x = x || Math.floor((Math.random() * 200) + (1920 / 2 - 100));
  this.y = y || Math.floor((Math.random() * 200) + (1080 / 2 - 100));
  this.direction = direction || this.getRandomDirection();
  this.mod = mod || this.getRandomModifier();
  this.timesRendered = 0;
  this.bounced = false;
};

Sprite.prototype.getRandomDirection = function () {
  //var myArray = ['U', 'UR', 'R', 'DR', 'D', 'DL', 'L', 'UL'];
  // var rand = myArray[Math.floor(Math.random() * myArray.length)];
  var rand = Math.random() * 2 * Math.PI; //get random val between 0 and 2 Pi ~ 0 and 360 degrees
  
  return rand;
};

Sprite.prototype.getRandomModifier = function () {
  var rand = Math.random() * 3 + 1; //get random value between 1 and 4 (float)
  return rand;
};

Sprite.prototype.update = function () {
  //assuming right is 0 and left is pi rad, though doesn't really matter
  //mod ~ modifier affects magnitude of displacement

  this.x += this.mod * Math.cos(this.direction); //apply degree to trig function for x displacement
  this.y += this.mod * Math.sin(this.direction); //apply degree to trig function for y displacement

  // screen coords are defined as 0,0 in the top left and max width, height in the bottom right
  // sprites bounce within a box 400 units past the spawn box NOTE! should base off var in init
  var x_right_border = 1460
  var x_left_border = 460

  var y_top_border = 40
  var y_bottom_border = 1040

  if ((this.x < x_left_border || this.x > x_right_border) && !this.bounced) {
    // this.direction += Math.PI / 2;
    this.direction = this.getBounceAngle(this.direction, true);
    this.bounced = true;
  }

  if ((this.y < y_top_border || this.y > y_bottom_border) && !this.bounced) {
    // this.direction += Math.PI / 2;
    this.direction = this.getBounceAngle(this.direction, false);
    this.bounced = true;
  }
  if ((this.y < y_bottom_border && this.y > y_top_border) && (this.x > x_left_border && this.x < x_right_border)) {
    this.bounced = false; //if sprite is within the box reset bounce flag
  }
  
  

  //debug("Testing!!!!!");
  /*
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
  }*/
};

Sprite.prototype.getBounceAngle = function (angle, vertical_wall = true) { //takes angle of colision and a bool to tell which kind of wall it's bouncing off of
  var QUART_CIRCLE = Math.PI/2;
  var FULL_CIRCLE = 2 * Math.PI;
  var wall_mod = vertical_wall ? 1 : -1;
  var bounce_angle = 0;

  if (angle < Math.PI * 0.5)
  {
    bounce_angle = angle + QUART_CIRCLE * wall_mod;
  }
  else
  {
    if (angle < Math.PI)
    {
      bounce_angle = angle - QUART_CIRCLE * wall_mod;
    }
    else
    {
      if (angle < Math.PI * 1.5)
      {
        bounce_angle = angle + QUART_CIRCLE * wall_mod;
      }
      else
      {
        bounce_angle = angle - QUART_CIRCLE * wall_mod; //default
      }
    }
  }
  bounce_angle = bounce_angle > FULL_CIRCLE ? bounce_angle - FULL_CIRCLE: bounce_angle;
  bounce_angle = bounce_angle < 0 ? bounce_angle + FULL_CIRCLE: bounce_angle;
  return bounce_angle;
};

Sprite.prototype.render = function (ctx) {
  this.timesRendered += 1;
  ctx.drawImage(this.image, this.x, this.y);
};
