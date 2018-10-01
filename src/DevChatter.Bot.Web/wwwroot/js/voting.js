var voting = (function () {
  var options = [];
  var votes = [];

  var voteEnd = function (ctx) {
    ctx.clearRect(0, 0, 1920, 1080);
    options = [];
    votes = [];
  };

  var voteReceived = function (ctx, voteInfo) {
    votes = voteInfo.voteTotals;
    // TODO: Announce the vote.
    displayVoteOverlay(ctx);
  };

  var voteStart = function (ctx, choices) {
    options = choices;
    votes = Array(choices.length).fill(0);
    displayChart(ctx);
  };

  function displayChart(ctx) {
    var myChart = new Chart(ctx, {
      type: 'horizontalBar',
      data: {
        labels: options,
        datasets: [{
          label: '# of Votes',
          data: votes,
          borderWidth: 1
        }]
      },
      options: {
        scales: {
          xAxes: [{
            ticks: {
              beginAtZero: true
            }
          }]
        }
      }
    });
  }

  function displayVoteOverlay(ctx) {
    var optionDisplays = options.map((x, i) => (i + 1) + " ) " + x + " - " + (votes[i] || '0'));
    ctx.clearRect(0, 0, 1920, 1080);
    ctx.fillStyle = "#cbcbcb";
    var height = 20 + (40 * optionDisplays.length);
    ctx.font = "30px Arial";
    var textWidth = Math.max(...optionDisplays.map(x => ctx.measureText(x).width));
    ctx.fillRect(0, 0, textWidth + 20, height);
    displayChoices(ctx, optionDisplays);
  }

  function displayChoices(ctx, optionDisplays) {
    ctx.fillStyle = "#000000";
    for (var i = 0; i < optionDisplays.length; i++) {
      ctx.fillText(optionDisplays[i], 10, 40 + 40*i);
    }
  }

  return {
    voteStart: voteStart,
    voteEnd: voteEnd,
    voteReceived: voteReceived
  };

}());


