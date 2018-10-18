let wasteful = function(canvas) {
  const wastefulBrown = '#dfd29e';
  const hangryRed = '#ff0000';

  const context = canvas.getContext('2d');
  const myPlayer = player(context);

  function startGame() {
    window.requestAnimationFrame(updateFrame);
  }

  function updateFrame() {
    clearCanvas();
    drawBackground();
    myPlayer.draw();
    window.requestAnimationFrame(updateFrame);
  }

  function clearCanvas() {
    context.clearRect(0, 0, canvas.width, canvas.height);
  }

  function drawBackground() {
    context.fillStyle = wastefulBrown;
    context.fillRect(0, 0, canvas.width, canvas.height);
  }

  return { startGame };
};
