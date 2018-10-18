function player(context) {

  let x = 84;
  let y = 84;
  let playerImage = new Image();
  playerImage.src = '/images/ZedChatter/Hat-YellowShirt-Player-Idle-0.png';

  function draw() {
    context.drawImage(playerImage, x, y);
  }

  return { draw };

}
