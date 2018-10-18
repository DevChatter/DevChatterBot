function zombie(canvas, context) {
  const size = 42;
  let x = 840;
  let y = 126;
  let playerImage = new Image();
  playerImage.src = '/images/ZedChatter/Zombie-0.png';

  let movers = {
    left: moveLeft,
    right: moveRight,
    up: moveUp,
    down: moveDown
  };

  function draw() {
    context.drawImage(playerImage, x, y);
  }

  function move(direction) {
    let moveIt = movers[direction];
    if (moveIt) moveIt();
  }

  function moveLeft() {
    x = Math.max(0, x - size);
  }

  function moveRight() {
    x = Math.min(canvas.width - size, x + size);
  }

  function moveUp() {
    y = Math.max(0, y - size);
  }

  function moveDown() {
    y = Math.min(canvas.height - size, y + size);
  }

  return { draw, move };
}
