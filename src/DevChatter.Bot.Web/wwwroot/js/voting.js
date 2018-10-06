var voting = (function () {
  var chart = {};
  var data = {};
  var choiceNames = [];

  function getRandomColor() {
    let colorOptions = [
      '#3366CC', '#DC3912', '#FF9900', '#109618', '#990099', '#3B3EAC', '#0099C6', '#DD4477', '#66AA00', '#B82E2E',
      '#316395', '#994499', '#22AA99', '#AAAA11', '#6633CC', '#E67300', '#8B0707', '#329262', '#5574A6', '#3B3EAC'
    ];
    return colorOptions[Math.floor(Math.random() * colorOptions.length)];
  }

  var optionsAnimation = {
    scales: {
      xAxes: [{
        ticks: {
          beginAtZero: true,
          display: false
        }
      }],
      yAxes: [{
        ticks: {
          fontSize: 40
        }
      }]
    }
  };

  var voteEnd = function (ctx) {
    ctx.clearRect(0, 0, 1920, 1080);
    chart = {};
    data = {};
  };

  let voteReceived = function (voteInfo, notifier) {
    notifier.info(`${voteInfo.voterName} voted for ${voteInfo.voterChoiceName}`);

    var dataSet = data["datasets"][0]["data"];
    voteInfo.voteTotals.forEach(function(voteCount) {
      dataSet.push(voteCount);
      dataSet.shift();
    });
    chart.update();
  };

  var voteStart = function (ctx, choices) {
    var barColors = choices.map(x => getRandomColor());
    choiceNames = choices; // set so we can access for notifier
    data = {
      labels: choices,
      datasets: [
        {
          label: '# of Votes',
          data: Array(choices.length).fill(0),
          backgroundColor: barColors,
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


