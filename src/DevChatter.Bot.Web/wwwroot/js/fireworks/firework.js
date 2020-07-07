function Firework(theColor, theParticleColor, theVelocity, canvas) {
    this.baseColor = theColor;
    this.particleColor = theParticleColor;
    this.velocity = theVelocity;
    this.explodeHeight = (canvas.height * 0.2) + (Math.random() * (canvas.height * 0.4));
    this.particles = [];
    this.x = 1920 / 2;
    this.y = 1060;
    this.exploded = false;

    this.MakeDiamondPattern = function () {
        this.particles.push(new Particle(this.particleColor, {x:2, y:0}, this.x, this.y))
        this.particles.push(new Particle(this.particleColor, {x:1, y:1}, this.x, this.y))
        this.particles.push(new Particle(this.particleColor, {x:0, y:2}, this.x, this.y))
        this.particles.push(new Particle(this.particleColor, {x:-1, y:1}, this.x, this.y))
        this.particles.push(new Particle(this.particleColor, {x:-2, y:0}, this.x, this.y))
        this.particles.push(new Particle(this.particleColor, {x:-1, y:-1}, this.x, this.y))
        this.particles.push(new Particle(this.particleColor, {x:0, y:-2}, this.x, this.y))
        this.particles.push(new Particle(this.particleColor, {x:1, y:-1}, this.x, this.y))
    }
    
    this.update = function () {
        if (this.exploded) return;
        this.x += this.velocity.x;
        this.y += this.velocity.y;
  
        if (this.y < this.explodeHeight) {
            this.exploded = true;
            // TODO: More patterns
            this.MakeDiamondPattern();
        }
    }

    this.render = function (context) {
        let toRemove = [];
        this.particles.forEach(function (particle, i){
            particle.update(); // has own velocity
            particle.render(context);
            if (particle.isDone()) {
                toRemove.push(i);
            }
        });
        toRemove.reverse();
        toRemove.forEach(i => {
            this.particles.splice(i, 1);
        });
    
        if (!this.exploded) {
            context.fillStyle = this.baseColor;
            context.fillRect(this.x, this.y, 12, 12);
        }
    }

    this.isDone = function () {
        return this.exploded && this.particles.length == 0;
    }
}
