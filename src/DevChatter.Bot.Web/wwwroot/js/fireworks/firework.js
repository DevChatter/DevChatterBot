// import { Particle } from "/js/fireworks/particle.js";

function Firework(theColor, theParticleColor, theVelocity, canvas) {
    const that = this;
    this.baseColor = theColor;
    this.particleColor = theParticleColor;
    this.velocity = theVelocity;
    this.explodeHeight = (canvas.height * 0.2) + (Math.random() * (canvas.height * 0.4));
    this.particles = [];
    this.x = 1920 / 2;
    this.y = 1060;
    this.exploded = false;

    this.MakeDiamondPattern = function () {
        that.particles.push(new Particle(that.particleColor, {x:2, y:0}, that.x, that.y))
        that.particles.push(new Particle(that.particleColor, {x:1, y:1}, that.x, that.y))
        that.particles.push(new Particle(that.particleColor, {x:0, y:2}, that.x, that.y))
        that.particles.push(new Particle(that.particleColor, {x:-1, y:1}, that.x, that.y))
        that.particles.push(new Particle(that.particleColor, {x:-2, y:0}, that.x, that.y))
        that.particles.push(new Particle(that.particleColor, {x:-1, y:-1}, that.x, that.y))
        that.particles.push(new Particle(that.particleColor, {x:0, y:-2}, that.x, that.y))
        that.particles.push(new Particle(that.particleColor, {x:1, y:-1}, that.x, that.y))
    }

    that.MakeSingleRingPattern = function () {
        let particleCount = 2 * (Math.floor(Math.random() * 3) + 8);
        let angle = Math.PI * 2 * Math.random();
        for (let index = 0; index < particleCount; index++) {
            angle = (angle + Math.PI * 2 / particleCount) % (Math.PI * 2);
            const newVelocity = { x:Math.cos(angle), y:Math.sin(angle) };
            that.particles.push(new Particle(that.particleColor, newVelocity, that.x, that.y));
        }
    }

    that.MakeRingPattern = function (ringCount) {
        let particleCount = 2 * (Math.floor(Math.random() * 3) + (8));
        let angle = Math.PI * 2 * Math.random();
        for (let ring = 0; ring < ringCount; ring++) {
            particleCount /= 2;
            for (let index = 0; index < particleCount; index++) {
                const speedMod = Math.pow(2, ring);
                angle = (angle + Math.PI * 2 / particleCount) % (Math.PI * 2);
                const newVelocity = { x:Math.cos(angle)/speedMod, y:Math.sin(angle)/speedMod };
                that.particles.push(new Particle(that.particleColor, newVelocity, that.x, that.y));
            }
        }
    }
    
    this.update = function () {
        if (this.exploded) return;
        this.x += this.velocity.x;
        this.y += this.velocity.y;
  
        if (this.y < this.explodeHeight) {
            this.exploded = true;
            const patterns = [this.MakeDiamondPattern, this.MakeSingleRingPattern, () => this.MakeRingPattern(2), () => this.MakeRingPattern(3)];
            patterns[Math.floor(Math.random() * patterns.length)]();
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
