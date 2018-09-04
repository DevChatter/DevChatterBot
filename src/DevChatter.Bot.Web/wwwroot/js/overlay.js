var overlay = (function () {
  var sprites = [];

  function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  var doHype = async function() {
    var image = new Image();
    image.src = '/images/DevchaHypeEmote.png';
    for (var blastIndex = 0; blastIndex < 20; blastIndex++) {
      for (var i = 0; i < 10; i++) {
        var hypeSprite = new Sprite(image);
        sprites.push(hypeSprite);
      }
      await sleep(250);
    }
  };

  window.onload = function() {
    var animationCanvas = document.getElementById('animationCanvas');
    var animationContext = animationCanvas.getContext('2d');

    var hangmanCanvas = document.getElementById('hangmanCanvas');
    var hangmanContext = hangmanCanvas.getContext('2d');

    var connection = new signalR.HubConnectionBuilder()
      .withUrl("/BotHub")
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection.start().catch(err => console.error(err.toString()));

    connection.on("Hype", () => {
      doHype();
      window.requestAnimationFrame(render);
    });
    connection.on("VoteStart", (choices) => {
      voting.voteStart(hangmanContext, choices);
    });
    connection.on("VoteReceived", (voteInfo) => {
      voting.voteReceived(hangmanContext, voteInfo);
    });
    connection.on("VoteEnd", () => {
      voting.voteEnd(hangmanContext);
    });
    connection.on("HangmanStart", () => {
      hangman.startGame();
      hangman.drawGallows(hangmanContext);
    });
    connection.on("HangmanWrongAnswer", () => {
      hangman.wrongAnswer(hangmanContext);
    });
    connection.on("HangmanLose", async function () {
      hangman.displayGameOver(hangmanContext);
      await sleep(4000);
      hangman.endGame(hangmanContext);
    });
    connection.on("HangmanWin", async function () {
      hangman.displayVictory(hangmanContext);
      await sleep(4000);
      hangman.endGame(hangmanContext);
    });

    function render() {
      animationContext.clearRect(0, 0, animationCanvas.width, animationCanvas.height);
      sprites.forEach(function (sprite, i) {
        sprite.update(animationCanvas);
        sprite.render(animationContext);
        if (sprite.timesRendered > 300) {
          sprites.splice(i, 1);
        }
      });
      if (sprites.length > 0) {
        window.requestAnimationFrame(render);
      } else {
        animationContext.clearRect(0, 0, animationCanvas.width, animationCanvas.height);
      }
    }
  }
  return {
    doHype: doHype
  };
}());

