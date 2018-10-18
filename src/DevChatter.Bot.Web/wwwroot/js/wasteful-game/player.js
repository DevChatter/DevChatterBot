function player(context) {
  const size = 42;
  let x = 84;
  let y = 84;
  let playerImage = new Image();
  playerImage.src = '/images/ZedChatter/Hat-YellowShirt-Player-Idle-0.png';

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
    x -= size;
  }

  function moveRight() {
    x += size;
  }

  function moveUp() {
    y -= size;
  }

  function moveDown() {
    y += size;
  }

  return { draw, move };

}
