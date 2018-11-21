	
		// setup target
		var targetDefaultRadius;
		var targetRadius;
		var targetHeight;
		var centerlize;
		var distractor = true;
		
		// setup hit mark
		var hitSpotY;
		var hitSpotSize;
		var hitSpotColor = "green";
		var hitX;
		
		// setup cannon
		var cannonX;
		var cannonHalfWidth;
		var cannonHeight;		
		var CANNON_COLOR = 'white';
		var angleBoundry = 75;
		var CANNON_ACCURACY;
		var NEEDED_ACCURACY = 10;	
		
		// setup sounds
		var laser = 'cannonGame/laser.'+SOUND_FORMAT;
		var hit = 'cannonGame/hit.'+SOUND_FORMAT;
		var firehit = 'mp3/firehit.'+SOUND_FORMAT;
		var stuck = 'cannonGame/stuck.'+SOUND_FORMAT;
		var scream = 'cannonGame/scream.'+SOUND_FORMAT;
		var startGame = 'cannonGame/startGame.mp3';		
		
		var move = {value: false};
		
		// level setup
		var AFTER_LEVEL_PLAY_LEVEL = -1;  // determines if the next level is a game level. -1 = continue toturial
		var ARROW_IS_NEXT_ROUND = false;
		
		function calcSizes() {

			targetDefaultRadius = Math.min(windowWidth/10,windowHeight/10);
			cannonX = windowWidth / 2;
			targetHeight = windowHeight / 5			
			centerlize = windowWidth/7;
			cannonHalfWidth = windowWidth/45;
			cannonHeight = windowHeight/2;					
			targetRadius = Math.floor(targetRadius);
			hitSpotSize = targetRadius/2;
			hitX = windowWidth;		
			CANNON_ACCURACY = 30;
			targetRadius = targetDefaultRadius;			
			CANNON_COLOR = "white";
			
		}
		
		function putDistractor(x,y,r) {
			putSmiley(x,y,r,Math.floor(Math.random()*3));
		}
		
		function restartCannon() {
			
			context.beginPath();
			context.fillStyle = CANNON_COLOR;
			context.fillRect(-cannonHalfWidth , -cannonHeight , cannonHalfWidth * 2 , cannonHeight*2);
			
			context.beginPath();
			context.arc(0, 10, cannonHalfWidth * 2, 0, 2 * Math.PI, false);
			context.fillStyle = CANNON_COLOR;
			context.fill();
			
		}
		
		function cannonShift(newAngel) {

			/*
			if (!gameStart) { gameStart = true;
								StartNewGame();
								return; }*/

			if (!move.value || !gameStart) { return;	}
								
			if (ARROW_IS_NEXT_ROUND) {
				level = level+1;
				newRound();
			}								
								
			if (Math.abs(newAngel) > angleBoundry) {
				// play out of bounds sound
				play_audio(stuck);
			} else {
			
				context.clearRect(-cannonHeight*2 , 0 , cannonHeight*4, -cannonHeight - 10);
				cannonAngle = newAngel;

				context.rotate(cannonAngle * Math.PI / 180);
				restartCannon();
				context.restore();
				
				context.rotate(-cannonAngle * Math.PI / 180);
			}		

			hideShame();
		}

		function hideShame() {
			context.beginPath();
			context.fillStyle = BG_COLOR;
			context.fillRect(-cannonHeight*2 , 0 , cannonHeight*4, cannonHeight);	
		}


		function hitmark(cannonAngle) { 
			var hitSpotY = targetHeight;

			// delete previous
			context.beginPath();
			context.arc(hitX, -windowHeight + hitSpotY, hitSpotSize*1.5, 0, 2 * Math.PI, false);
			context.fillStyle = "black";
			context.fill();	
			
			// add new
			hitX = Math.tan(-cannonAngle * Math.PI / 180) * (-windowHeight + hitSpotY);
		   context.strokeStyle = hitSpotColor;
		   context.lineWidth = hitSpotSize / 4;
			
		  context.beginPath();
		  context.moveTo(hitX - hitSpotSize, -windowHeight + hitSpotY - hitSpotSize);
		  context.lineTo(hitX + hitSpotSize, -windowHeight + hitSpotY + hitSpotSize);
		  context.stroke();			
		  
		  context.beginPath();
		  context.moveTo(hitX - hitSpotSize, -windowHeight + hitSpotY + hitSpotSize);
		  context.lineTo(hitX + hitSpotSize, -windowHeight + hitSpotY - hitSpotSize);
		  context.stroke();				  
			
			
		}

		window.addEventListener('keydown', function(e) { 
		
			if (!move.value) { return;	}
		
			if (!gameStart) { spaceBar();
								return; }				
		
			switch (e.keyCode) {
				case 39:
					cannonShift(cannonAngle+CANNON_ACCURACY);
					break;
				case 37:
					cannonShift(cannonAngle-CANNON_ACCURACY);
					break;
				case 38:
					fire();
					break;
				case 32:
					spaceBar();	
					break;
			}

		});		

		function goLeft() {
			if (!move.value) { return;	}
			cannonShift(cannonAngle-CANNON_ACCURACY);
		}
		
		function goRight() {
			if (!move.value) { return;	}
			cannonShift(cannonAngle+CANNON_ACCURACY);
		}		
		
		function goDown() {
			if (!move.value) { return;	}
			fire();
		}
		
		function goUp() {
			if (!move.value) { return;	}
			fire();
		}		

		