let wasteful = function(canvas) {
  const wastefulBrown = '#dfd29e';
  const hangryRed = '#ff0000';

  const context = canvas.getContext('2d');
  const myPlayer = player(canvas, context);
  const myZombie = zombie(canvas, context);

  function startGame() {
    window.requestAnimationFrame(updateFrame);
  }

  function movePlayer(direction) {
    myPlayer.move(direction);
    myZombie.moveToward(myPlayer);
  }

  function updateFrame() {
    clearCanvas();
    drawBackground();
    myPlayer.draw();
    myZombie.draw();
    window.requestAnimationFrame(updateFrame);
  }

  function clearCanvas() {
    context.clearRect(0, 0, canvas.width, canvas.height);
  }

  function drawBackground() {
    context.fillStyle = wastefulBrown;
    context.fillRect(0, 0, canvas.width, canvas.height);
  }

  return { startGame, movePlayer };
};
