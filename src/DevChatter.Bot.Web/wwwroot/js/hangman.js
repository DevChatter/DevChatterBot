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

  var endGame = function () {
    isRunning = false;
    wrongAnswerCount = 0;
  };

  var wrongAnswer = function () {
    wrongAnswerCount++;
    if (wrongAnswerCount > hangmanPieces.length) {
      console.log('Hi there!');
      endGame();
    }
  };

  var startGame = function () {
    isRunning = true;
  };


  function drawLine(ctx, x1, y1, x2, y2, color, width) {
    ctx.lineWidth = width || 1;
    ctx.strokeStyle = color || "#000000";
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.stroke();
  }


  var drawHangman = function (ctx) {

    if (!isRunning) return;

    // Draw this set when the game is running
    drawLine(ctx, 0, 300, 300, 300, '#966F33', 25); // Platform
    drawLine(ctx, 10, 0, 10, 300, '#966F33', 10); // Pole
    drawLine(ctx, 0, 5, 140, 5, '#966F33', 5); // Branch
    drawLine(ctx, 120, 5, 120, 30, '#966F33'); // Noose

    for (var i = 0; i < wrongAnswerCount; i++) {
      hangmanPieces[i](ctx);
    }
  };

  return {
    startGame: startGame,
    endGame: endGame,
    wrongAnswer: wrongAnswer,
    drawHangman: drawHangman
  };

}());


