var fireworks = (function () {
  let canvas;
  let context;
  let fireworks = [];

  function randomColor() {
    return `#${Math.floor(Math.random() * 16777216).toString(16).padStart(6, '0')}`;
  }

  function addFireworks(theCanvas) {
    fireworks.push(new Firework(randomColor(), randomColor(), { x: (Math.random() * 4 - 2), y: -4 }, theCanvas));
    fireworks.push(new Firework(randomColor(), randomColor(), { x: (Math.random() * 4 - 2), y: -4 }, theCanvas));
    fireworks.push(new Firework(randomColor(), randomColor(), { x: (Math.random() * 4 - 2), y: -4 }, theCanvas));
    fireworks.push(new Firework(randomColor(), randomColor(), { x: (Math.random() * 4 - 2), y: -4 }, theCanvas));
    fireworks.push(new Firework(randomColor(), randomColor(), { x: (Math.random() * 4 - 2), y: -4 }, theCanvas));
    fireworks.push(new Firework(randomColor(), randomColor(), { x: (Math.random() * 4 - 2), y: -4 }, theCanvas));
  }

  function init(theCanvas, theContext) {
    canvas = theCanvas;
    context = theContext;
  }

  function render() {
    context.clearRect(0, 0, canvas.width, canvas.height);
    fireworks.forEach(function (firework, i) {
      firework.update(); // has own velocity
      firework.render(context);
      if (firework.isDone()) {
        fireworks.splice(i, 1);
      }
    });
    const keepGoing = fireworks.length > 0;
    if (!keepGoing) {
      context.clearRect(0, 0, canvas.width, canvas.height);
    }
    return keepGoing;
  }

  return {
    init,
    render,
    addFireworks
  };
}());
