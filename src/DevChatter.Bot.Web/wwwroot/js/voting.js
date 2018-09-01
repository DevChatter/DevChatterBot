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
    ctx.font = "30px Arial";
    // TODO: find the longest measureText result from the choices
    var textLength = ctx.measureText("TheLongestChoiceInHere");
    ctx.fillRect(0, 0, textLength.width + 20, height);
    displayChoices(ctx, choices);
  };


  function displayChoices(ctx, choices) {
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


