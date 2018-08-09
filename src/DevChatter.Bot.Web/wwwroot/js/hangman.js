var hangman = (function () {
  this.headImage = new Image();
  this.headImage.src = '/images/BibleThump.png';
  var isRunning = false;
  var wrongAnswerCount = 0;

  var drawHead = function (ctx) {
    ctx.beginPath();
    ctx.drawImage(headImage, 90, 25);
    ctx.stroke();
  };
  var drawTorso = function (ctx) {
    drawLine(ctx, 120, 72, 120, 140);
  };
  var drawRightArm = function (ctx) {
    drawLine(ctx, 120, 92, 180, 100);
  };
  var drawLeftArm = function (ctx) {
    drawLine(ctx, 120, 92, 60, 100);
  };
  var drawRightLeg = function (ctx) {
    drawLine(ctx, 120, 140, 160, 220);
  };
  var drawLeftLeg = function (ctx) {
    drawLine(ctx, 120, 140, 80, 220);
  };

  var hangmanPieces = [drawHead, drawTorso, drawRightArm, drawLeftArm, drawRightLeg, drawLeftLeg];

  var endGame = function (ctx) {
    ctx.clearRect(0, 0, 1920, 1080);
    isRunning = false;
    wrongAnswerCount = 0;
  };

  var wrongAnswer = function (ctx) {
    wrongAnswerCount++;
    if (wrongAnswerCount > hangmanPieces.length) {
      endGame(ctx);
    } else {
      ctx.strokeStyle = "#000000";
      hangmanPieces[wrongAnswerCount- 1](ctx);
    }
  };

  var startGame = function () {
    isRunning = true;
  };


  function drawLine(ctx, x1, y1, x2, y2, width) {
    ctx.lineWidth = width || 1;
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.stroke();
  }


  var drawGallows = function (ctx) {

    if (!isRunning) return;

    ctx.clearRect(0, 0, 1920, 1080);

    // Draw this set when the game is running
    ctx.strokeStyle = "#966F33";
    drawLine(ctx, 0, 300, 300, 300, 25); // Platform
    drawLine(ctx, 10, 0, 10, 300, 10); // Pole
    drawLine(ctx, 0, 5, 140, 5, 5); // Branch
    drawLine(ctx, 120, 5, 120, 30); // Noose
  };

  return {
    startGame: startGame,
    endGame: endGame,
    wrongAnswer: wrongAnswer,
    drawGallows: drawGallows
  };

}());


