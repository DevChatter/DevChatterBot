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

    var connection = new signalR.HubConnectionBuilder()
      .withUrl("/BotHub")
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection.start().catch(err => console.error(err.toString()));

    connection.on("Hype", () => {
      doHype();
    });

    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');

    function render() {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      sprites.forEach(function (sprite, i) {
        sprite.update();
        sprite.render(ctx);
        if (sprite.timesRendered > 300) {
          sprites.splice(i, 1);
        }
      });
      window.requestAnimationFrame(render);
    }

    window.requestAnimationFrame(render);

  }
  return {
    doHype: doHype
  };
}());

