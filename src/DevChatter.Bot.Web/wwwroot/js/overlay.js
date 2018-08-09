var overlay = (function () {
  var sprites = [];

  function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  var doHype = async function() {
    for (var blastIndex = 0; blastIndex < 20; blastIndex++) {
      for (var i = 0; i < 25; i++) {
        var hypeSprite = new Sprite('/images/DevchaHypeEmote.png');
        sprites.push(hypeSprite);
      }
      await sleep(250);
    }
  }

  window.onload = function() {

    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');

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
      hangman.drawGallows(ctx);
    });
    connection.on("HangmanWrongAnswer", () => {
      hangman.wrongAnswer(ctx);
    });
    connection.on("HangmanEnd", () => {
      hangman.endGame(ctx);
    });

    function render() {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      sprites.forEach(function (sprite, i) {
        sprite.update();
        sprite.render(ctx);
        if (sprite.timesRendered > 300) {
          sprites.splice(i, 1);
        }
      });
      if (sprites.length > 0) {
        window.requestAnimationFrame(render);
      } else {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
      }
    }
  }
  return {
    doHype: doHype
  };
}());

