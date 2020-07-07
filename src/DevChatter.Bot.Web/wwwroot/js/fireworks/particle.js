function Particle(theColor, theVelocity, startX, startY) {
    this.color = theColor;
    this.velocity = theVelocity;
    this.x = startX;
    this.y = startY;
    this.renderCount = 0;

    this.update = function () {
        this.x += this.velocity.x;
        this.y += this.velocity.y;
    }

    this.render = function (context) {
        this.renderCount++;
        context.fillStyle = this.color;
        context.fillRect(this.x, this.y, 4, 4)
    }

    this.isDone = function () {
        return this.renderCount > 120;
    }
}
