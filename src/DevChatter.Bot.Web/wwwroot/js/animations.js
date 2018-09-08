var animations = (function () {
  let sprites = [];
  let canvas;
  let context;

  function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  function init(animationCanvas, animationContext) {
    canvas = animationCanvas;
    context = animationContext;
  }

  function render() {
    context.clearRect(0, 0, canvas.width, canvas.height);
    sprites.forEach(function (sprite, i) {
      sprite.update(canvas);
      sprite.render(context);
      if (sprite.timesRendered > 300) {
        sprites.splice(i, 1);
      }
    });
    if (sprites.length > 0) {
      window.requestAnimationFrame(render);
    } else {
      context.clearRect(0, 0, canvas.width, canvas.height);
    }
  }

  let doHype = async function () {
    let image = new Image();
    image.src = '/images/DevchaHypeEmote.png';
    for (let blastIndex = 0; blastIndex < 20; blastIndex++) {
      for (let i = 0; i < 10; i++) {
        var hypeSprite = new Sprite(image);
        sprites.push(hypeSprite);
      }
      await sleep(250);
    }
  };

  return {
    init: init,
    render : render,
    doHype: doHype
  };

}());
