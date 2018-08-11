var overlay = (function () {
  var sprites = [];

  function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  var doHype = async function() {
    for (var blastIndex = 0; blastIndex < 20; blastIndex++) {
      for (var i = 0; i < 25; i++) {
        var canvas = document.getElementById('myCanvas'); //defining local var canvas, is this necessary? can a wider scoped canvas be used in it's place 
        var hypeSprite = new Sprite('/images/DevchaHypeEmote.png', canvas.width, canvas.height);
        sprites.push(hypeSprite);
      }
      await sleep(250);
    }
  }

  window.onload = function() {
    var mainCanvas = document.getElementById('mainCanvas');
    var mainContext = mainCanvas.getContext('2d');

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
      mainContext.clearRect(0, 0, mainCanvas.width, mainCanvas.height);
      sprites.forEach(function (sprite, i) {
        sprite.update();
        sprite.render(mainContext);
        if (sprite.timesRendered > 300) {
          sprites.splice(i, 1);
        }
      });
      if (sprites.length > 0) {
        window.requestAnimationFrame(render);
      } else {
        mainContext.clearRect(0, 0, mainCanvas.width, mainCanvas.height);
      }
    }
  }
  return {
    doHype: doHype
  };
}());

