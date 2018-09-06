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

    var votingCanvas = document.getElementById('votingCanvas');
    var votingContext = votingCanvas.getContext('2d');

    var botHubConn = new signalR.HubConnectionBuilder()
      .withUrl("/BotHub")
      .configureLogging(signalR.LogLevel.Information)
      .build();

    var votingHubConn = new signalR.HubConnectionBuilder()
    .withUrl("/VotingHub") 
    .configureLogging(signalR.LogLevel.Information)
    .build();

    botHubConn.start().catch(err => console.error(err.toString()));
    votingHubConn.start().catch(err => console.error(err.toString()));

    botHubConn.on("Hype", () => {
      doHype();
      window.requestAnimationFrame(render);
    });
    votingHubConn.on("VoteStart", (choices) => {
      voting.voteStart(votingContext, choices);
    });
    votingHubConn.on("VoteReceived", (voteInfo) => {
      voting.voteReceived(votingContext, voteInfo);
    });
    votingHubConn.on("VoteEnd", () => {
      voting.voteEnd(votingContext);
    });
    botHubConn.on("HangmanStart", () => {
      hangman.startGame();
      hangman.drawGallows(hangmanContext);
    });
    botHubConn.on("HangmanWrongAnswer", () => {
      hangman.wrongAnswer(hangmanContext);
    });
    botHubConn.on("HangmanLose", async function () {
      hangman.displayGameOver(hangmanContext);
      await sleep(4000);
      hangman.endGame(hangmanContext);
    });
    botHubConn.on("HangmanWin", async function () {
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

