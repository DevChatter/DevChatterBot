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

  function moveToward(player) {
    let location = player.getLocation();
    if (location.x < x) {
      moveLeft();
    } else if (location.x > x) {
      moveRight();
    } else if (location.y < y) {
      moveUp();
    } else if (location.y > y) {
      moveDown();
    } else {
      // nom nom nom
      // How you get on the player?!?!?!
    }
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

  return { draw, moveToward };
}
