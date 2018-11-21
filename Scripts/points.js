
	var mid = new Audio();
	var newLevel = false;
	
/* must implement:
	MAX_LEVEL
	POINTS_PER_LEVEL
	
	newRound()
	
	switch(level) < in levelSettings()
	*/
	
	var levelUp = new Audio('~/../../cannonGame/levelUp.' + SOUND_FORMAT);
	var maxLevel = new Audio('~/../../cannonGame/topLevel.' + SOUND_FORMAT);
	var zerop = new Audio('~/../../cannonGame/0p.' + SOUND_FORMAT);
	var onep = new Audio('~/../../cannonGame/1p.' + SOUND_FORMAT);
	var twop = new Audio('~/../../cannonGame/2p.' + SOUND_FORMAT);
	var threep = new Audio('~/../../cannonGame/3p.' + SOUND_FORMAT);
	var fourp = new Audio('~/../../cannonGame/4p.' + SOUND_FORMAT);
	var fivep = new Audio('~/../../cannonGame/5p.' + SOUND_FORMAT);
	var sixp = new Audio('~/../../cannonGame/6p.' + SOUND_FORMAT);
	var sevenp = new Audio('~/../../cannonGame/7p.' + SOUND_FORMAT);
	var eightp = new Audio('~/../../cannonGame/8p.' + SOUND_FORMAT);
	var ninep = new Audio('~/../../cannonGame/9p.' + SOUND_FORMAT);
	var tenp = new Audio('~/../../cannonGame/10p.' + SOUND_FORMAT);
		
	var nice = new Audio('~/../../cannonGame/nice.mp3');
	var amazing = new Audio('~/../../cannonGame/amazing.mp3');
	var good = new Audio('~/../../cannonGame/prettyGood.mp3');
	var best = new Audio('~/../../cannonGame/best.mp3');
	var bad = new Audio('~/../../cannonGame/bad.mp3');
		
	    var youwin = new Audio('~/../../cannonGame/youwin.'+SOUND_FORMAT);	
		var restart = new Audio('~/../../cannonGame/continue.mp3');
		
		var gameOver = false;
		var win = true;
	
		function pointTally() {
			if (!gameOver) {
			
				if (score >= POINTS_PER_LEVEL && win) {
					youwin.play();
					setTimeout(function() {
						restart.play();
					}, 2500);
					win = false; 
					gameOver = true;
				} else {
					switch (score) {
						case 0:
							zerop.play();
							break;			
						case 1:
							onep.play();
							break;
						case 2:
							twop.play();
							break;
						case 3:
							threep.play();
							break;
						case 4:
							fourp.play();
							break;		
						case 5:
							fivep.play();
							break;			
						case 6:
							sixp.play();
							break;
						case 7:
							sevenp.play();
							break;
						case 8:
							eightp.play();
							break;
						case 9:
							ninep.play();
							break;	
						case 10:
							tenp.play();
							break;						
						default:
							if (win) { 
								youwin.play();
								setTimeout(function() {
									restart.play();
								}, 2500);
								win = false; 
							}
							break;					
					}
				}
			}
		}
		
		function pointTalk() {
			// talk about points 1-10
			if (score == 10) {
				best.play();
			} else if (score>7) {
				amazing.play();
			} else if (score > 4) {
				good.play();
			} else if (score > 2) {
				nice.play();
			} else {
				bad.play();
			}
		}
		
		function checkLevel() {

			if (score >= POINTS_PER_LEVEL && level < MAX_LEVEL) {
				// level up
				newLevel = true;
				level = level + 1;
				score = 0;
				if (level == MAX_LEVEL) {
					setTimeout(function() {	maxLevel.play(); }, 500 );
					if (!mobilecheck) { setTimeout(function() { mid.play();     }, 1100 ); }
				} else {
					if (level>0) { setTimeout(function() {	levelUp.play(); }, 500 ); }
					if (!mobilecheck) { setTimeout(function() { mid.play(); }, 1100 ); }
				}
			} else if (score == POINTS_PER_LEVEL)  {
				if (!gameOver) {
					youwin.play(); gameOver = true; 
						setTimeout(function() {
							restart.play();
						}, 2500);					
				}
			} else {
				pointTally();
				return false;
			}	
			return true;		
		}
		
		
		function GET() {
			var parts = window.location.search.substr(1).split("&");
			var $_GET = {};
			for (var i = 0; i < parts.length; i++) {
				var temp = parts[i].split("=");
				$_GET[decodeURIComponent(temp[0])] = decodeURIComponent(temp[1]);
			}
			
			return $_GET;
		}
		
		var get = GET();
		// Reading level from address line in browser.
		function getLevel() {
			if ( 'level' in get ) {
				level = parseInt(get['level']);
			}
		}