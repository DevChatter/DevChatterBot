var overlay = (function () {

  function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  window.onload = function() {
    let animationCanvas = document.getElementById('animationCanvas');
    let animationContext = animationCanvas.getContext('2d');
    animations.init(animationCanvas, animationContext);

    let hangmanCanvas = document.getElementById('hangmanCanvas');
    let hangmanContext = hangmanCanvas.getContext('2d');

    let votingCanvas = document.getElementById('votingCanvas');
    let votingContext = votingCanvas.getContext('2d');

    let botHubConn = createHubConnection("BotHub");

    let votingHubConn = createHubConnection("VotingHub");

    let hangmanHubConn = createHubConnection("HangmanHub");

    function createHubConnection(hubName) {
      let hubConn = new signalR.HubConnectionBuilder()
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

    function startHubConn(hubToConnect, retryInterval = 2000) {
      console.log(`[${new Date()}] Connecting to ${hubToConnect.hubName}`);
      hubToConnect.start().then(() => {
        },
        err => {
          console.error(err.toString());
          let i = Math.min(retryInterval * 1.5, maxRetryInterval);
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

    botHubConn.on("Hype",
      () => {
        animations.blastImages('/images/DevchaHypeEmote.png');
        window.requestAnimationFrame(animations.render);
      });
    botHubConn.on("Derp",
      () => {
        animations.blastImages('/images/DevchaDerpEmote.png');
        window.requestAnimationFrame(animations.render);
      });
    votingHubConn.on("VoteStart",
      (choices) => {
        voting.voteStart(votingContext, choices);
      });
    votingHubConn.on("VoteReceived",
      (voteInfo) => {
        voting.voteReceived(votingContext, voteInfo);
      });
    votingHubConn.on("VoteEnd",
      () => {
        voting.voteEnd(votingContext);
      });
    hangmanHubConn.on("HangmanStart",
      () => {
        hangman.startGame();
        hangman.drawGallows(hangmanContext);
      });
    hangmanHubConn.on("HangmanWrongAnswer",
      () => {
        hangman.wrongAnswer(hangmanContext);
      });
    hangmanHubConn.on("HangmanLose",
      async function() {
        hangman.displayGameOver(hangmanContext);
        await sleep(4000);
        hangman.endGame(hangmanContext);
      });
    hangmanHubConn.on("HangmanWin",
      async function() {
        hangman.displayVictory(hangmanContext);
        await sleep(4000);
        hangman.endGame(hangmanContext);
      });

  };
}());

