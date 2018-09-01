var voting = (function () {
  var isRunning = false;

  var voteEnd = function (ctx) {
    ctx.clearRect(0, 0, 1920, 1080);
    isRunning = false;
  };

  var voteStart = function (ctx, choices) {
    isRunning = true;
    ctx.fillStyle = "#cbcbcb";
    var height = 20 + (40 * choices.length);
    ctx.fillRect(0, 0, 200, height);
    displayChoices(ctx, choices);
  };


  function displayChoices(ctx, choices) {
    ctx.font = "30px Arial";
    ctx.fillStyle = "#000000";
    for (var i = 0; i < choices.length; i++) {
      ctx.fillText(choices[i], 10, 40 + 40*i);
    }
  }

  return {
    voteStart: voteStart,
    voteEnd: voteEnd
  };

}());


