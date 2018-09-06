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

    var botHubConn = createHubConnection("BotHub");

    var votingHubConn = createHubConnection("VotingHub");

    var hangmanHubConn = createHubConnection("HangmanHub");

    function createHubConnection(hubName) {
      var hubConn = new signalR.HubConnectionBuilder()
        .withUrl(`/${hubName}`)
        .configureLogging(signalR.LogLevel.Information)
        .build();
      hubConn.hubName = hubName;
      return hubConn;
    }

    const maxRetryInterval = 30000;

    startHubConn(botHubConn);
    startHubConn(votingHubConn);
    startHubConn(hangmanHubConn);

    function startHubConn(hubToConnect, retryInterval = 2000){
      console.log(`[${new Date()}] Connecting to ${hubToConnect.hubName}`);
      hubToConnect.start().then(()=> {
      }, err => {
        console.error(err.toString());
        var i = Math.min(retryInterval * 1.5, maxRetryInterval);
        console.log(`[${new Date()}] Retry connection to ${hubToConnect.hubName} in ${i} ms.`);
        setTimeout(() => startHubConn(hubToConnect, i), i);
      });
    }

    botHubConn.onclose(() => {
      setTimeout(() => startHubConn(botHubConn), 2000);
    });

    votingHubConn.onclose(() => {
      setTimeout(() => startHubConn(votingHubConn), 2000);
    });

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
    hangmanHubConn.on("HangmanStart", () => {
      hangman.startGame();
      hangman.drawGallows(hangmanContext);
    });
    hangmanHubConn.on("HangmanWrongAnswer", () => {
      hangman.wrongAnswer(hangmanContext);
    });
    hangmanHubConn.on("HangmanLose", async function () {
      hangman.displayGameOver(hangmanContext);
      await sleep(4000);
      hangman.endGame(hangmanContext);
    });
    hangmanHubConn.on("HangmanWin", async function () {
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

