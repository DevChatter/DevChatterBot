import { Wasteful } from '/js/wasteful-game/wasteful.js';

var overlay = (function () {

  let notificationOptions = {
    position: 'top-left',
    maxNotifications: 6,
    icons: { enabled: false }
  };

  function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  window.onload = function () {
    let awesomeNotifier = new AWN(notificationOptions);

    let animationCanvas = document.getElementById('animationCanvas');
    let animationContext = animationCanvas.getContext('2d');
    animations.init(animationCanvas, animationContext);

    const fireworksCanvas = document.getElementById('fireworksCanvas');
    const fireworksCtx = fireworksCanvas.getContext('2d');
    fireworks.init(fireworksCanvas, fireworksCtx);


    let hangmanCanvas = document.getElementById('hangmanCanvas');
    let hangmanContext = hangmanCanvas.getContext('2d');

    let votingCanvas = document.getElementById('votingCanvas');
    let votingContext = votingCanvas.getContext('2d');

    let botHubConn = createHubConnection("BotHub");

    let votingHubConn = createHubConnection("VotingHub");

    let hangmanHubConn = createHubConnection("HangmanHub");

    let wastefulHubConn = createHubConnection("WastefulHub");

    let wastefulCanvas = document.getElementById('wastefulCanvas');
    let myWasteful = new Wasteful(wastefulCanvas, wastefulHubConn);


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
    startHubConn(wastefulHubConn);

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

    wastefulHubConn.onclose(() => {
      setTimeout(() => startHubConn(wastefulHubConn), 2000);
    });

    function doBlast(imagePath) {
      animations.blastImages(imagePath);
      window.requestAnimationFrame(animations.render);
    }

    botHubConn.on("Blast", doBlast);
    botHubConn.on("Fireworks",
      async function () {
        fireworks.addFireworks(fireworksCanvas);
        function step() {
          fireworks.render();
          window.requestAnimationFrame(step);
        }
        window.requestAnimationFrame(step);
        for (let i = 0; i < 9; i++) {
          await sleep(250);
          fireworks.addFireworks(fireworksCanvas);
        }
      });
    votingHubConn.on("VoteStart",
      (choices) => {
        $(votingCanvas).show();
        voting.voteStart(votingContext, choices);
      });
    votingHubConn.on("VoteReceived",
      (voteInfo) => {
        voting.voteReceived(voteInfo, awesomeNotifier);
      });
    votingHubConn.on("VoteEnd",
      () => {
        voting.voteEnd(votingContext);
        $(votingCanvas).hide();
      });
    hangmanHubConn.on("HangmanStart",
      (allLetters, livesRemaining, maskedWord) => {
        hangman.startGame();
        hangman.drawGallows(hangmanContext, allLetters, livesRemaining, maskedWord);
      });
    hangmanHubConn.on("HangmanWrongAnswer",
      () => {
        hangman.wrongAnswer(hangmanContext);
      });
    hangmanHubConn.on("HangmanLose",
      async function () {
        hangman.displayGameOver(hangmanContext);
        await sleep(4000);
        hangman.endGame(hangmanContext);
        doBlast('/images/DevchaFailEmote.png');
      });
    hangmanHubConn.on("HangmanWin",
      async function () {
        hangman.displayVictory(hangmanContext);
        await sleep(4000);
        hangman.endGame(hangmanContext);
        doBlast('/images/DevchaHypeEmote.png');
      });
    hangmanHubConn.on("HangmanShowGuessedLetters",
      async (availableLetters, livesRemaining, maskedWord) => {
        hangman.displayInfo(hangmanContext, availableLetters, livesRemaining, maskedWord);
      });
    wastefulHubConn.on("MovePlayer",
      async (direction, moveNumber) => {
        myWasteful.movePlayer(direction, moveNumber);
      });
    wastefulHubConn.on("StartGame",
      async (displayName, userId, inventoryItems) => {
        myWasteful.startGame({ displayName, userId }, inventoryItems);
      });
    wastefulHubConn.on("DisplaySurvivorRankings",
      async (survivorRankingData) => {
        myWasteful.displaySurvivorRankings(survivorRankingData);
      });
  };
}());

