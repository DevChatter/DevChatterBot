function Sprite(imageSrc, width, height, x, y, direction, mod) {
  this.image = new Image();
  this.image.src = imageSrc;
  this.x = x || Math.floor((Math.random() * 200) + (1920 / 2 - 100));
  this.y = y || Math.floor((Math.random() * 200) + (1080 / 2 - 100));
  this.direction = direction || this.getRandomDirection();
  this.mod = mod || this.getRandomModifier(); //sets rate of displacement (how fast the sprite is moving)
  this.timesRendered = 0;
  this.bounced = false;
  this.x_right_border = width - this.image.width; //width of canvas - width of image, insures the right border of the image bounces off the right border of the canvas
  this.y_bottom_border = height - this.image.height; //same as above but for bottom of image and canvas
  this.x_left_border = 0;
  this.y_top_border = 0;
  // console.log("width: " + this.x_right_border + " height: " + this.y_bottom_border);
};

Sprite.prototype.getRandomDirection = function () {
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


  //check if x position has moved beyond the left or right border
  if ((this.x < this.x_left_border || this.x > this.x_right_border) && !this.bounced) {
    // this.direction += Math.PI / 2;
    this.direction = this.getBounceAngle(this.direction, true);
    this.bounced = true;
  }

  //check if y position has moved beyond the upper or lower border
  if ((this.y < this.y_top_border || this.y > this.y_bottom_border) && !this.bounced) {
    // this.direction += Math.PI / 2;
    this.direction = this.getBounceAngle(this.direction, false);
    this.bounced = true;
  }

  //not sure if this could be removed and replaced with a canvas.intersects(this.x, this.y) or something simmilar. Could also be moved to a intersects() function
  if ((this.y < this.y_bottom_border && this.y > this.y_top_border) && (this.x > this.x_left_border && this.x < this.x_right_border)) {
    this.bounced = false; //if sprite is within the box reset bounce flag
  }

};

// takes angle of colision and a bool to tell which kind of wall it's bouncing off of
// outputs the incident angle that the sprite will be going in
Sprite.prototype.getBounceAngle = function (angle, vertical_wall = true) { 
  var QUART_CIRCLE = Math.PI / 2;
  var HALF_CIRCLE = Math.PI;
  var FULL_CIRCLE = 2 * Math.PI;
  var bounce_angle = 0;


  //half baked math incoming
  //this seems to work for calculating incident angle, may need to be revised or simplified
  if (vertical_wall) { //handling bounces off of vertical walls
    if (angle < Math.PI * 0.5) //90 degrees
    {
      bounce_angle = (QUART_CIRCLE - angle) * 2 + angle;
    }
    else {
      if (angle < Math.PI) //180 degrees
      {
        bounce_angle = (angle - QUART_CIRCLE) * 2 - angle;
      }
      else {
        if (angle < Math.PI * 1.5) // 270 degrees
        {
          bounce_angle = (3 * QUART_CIRCLE - angle) * 2 + angle;
        }
        else {
          bounce_angle = (angle - 3 * QUART_CIRCLE) * 2 - angle; //default 360/0 degrees
        }
      }
    }
  }
  else {//handling bounces off of horizontal walls
    if (angle < Math.PI * 0.5) //90 degrees
    {
      bounce_angle = angle - 2 * angle;
    }
    else {
      if (angle < Math.PI) //180 degrees
      {
        bounce_angle = angle + (HALF_CIRCLE - angle) * 2;
      }
      else {
        if (angle < Math.PI * 1.5) // 270 degrees
        {
          bounce_angle = angle - (angle - HALF_CIRCLE)* 2;
        }
        else {
          bounce_angle = angle + (FULL_CIRCLE - angle) * 2; //default 360/0 degrees
        }
      }
    }
  }
  //if angle has gone passed 360 or below 0, correct this. The trig functions shouldn't care though the logic above is broken if the angle isn't within 0 and 360 (0 and 2pi)
  bounce_angle = bounce_angle > FULL_CIRCLE ? bounce_angle - FULL_CIRCLE: bounce_angle; 
  bounce_angle = bounce_angle < 0 ? bounce_angle + FULL_CIRCLE: bounce_angle;
  return bounce_angle;
};

Sprite.prototype.render = function (ctx) {
  this.timesRendered += 1;
  ctx.drawImage(this.image, this.x, this.y);
};
