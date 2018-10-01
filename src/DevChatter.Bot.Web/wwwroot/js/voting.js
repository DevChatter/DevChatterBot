var voting = (function () {
  var chart = {};
  var data = {};

  var optionsAnimation = {
    scales: {
      xAxes: [{
        ticks: {
          beginAtZero: true
        }
      }]
    }
  };

  var voteEnd = function (ctx) {
    ctx.clearRect(0, 0, 1920, 1080);
    chart = {};
    data = {};
  };

  let voteReceived = function (ctx, voteInfo) {
    // TODO: Announce the vote.

    var dataSet = data["datasets"][0]["data"];
    voteInfo.voteTotals.forEach(function(voteCount) {
      dataSet.push(voteCount);
      dataSet.shift();
    });
    chart.update();
  };

  var voteStart = function (ctx, choices) {
    data = {
      labels: choices,
      datasets: [
        {
          label: '# of Votes',
          data: Array(choices.length).fill(0),
          borderWidth: 1
        }
      ]
    };
    chart = new Chart(ctx, {
      type: 'horizontalBar',
      data: data,
      options: optionsAnimation
    });
  };

  return {
    voteStart: voteStart,
    voteEnd: voteEnd,
    voteReceived: voteReceived
  };

}());


