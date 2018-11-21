
	var mid;
	var mid_volume;
	var newLevel = false;
	var DONT_SAY_LEVEL_UP = false;
	
/* must implement:
	MAX_LEVEL
	POINTS_PER_LEVEL
	
	newRound()
	
	switch(level) < in levelSettings()
	*/
	
		var levelUp = SOUND_FORMAT+'/levelUp.'+SOUND_FORMAT;
		var maxLevel = SOUND_FORMAT+'/topLevel.'+SOUND_FORMAT;	
		var zerop = SOUND_FORMAT+'/0p.'+SOUND_FORMAT;
		var onep = SOUND_FORMAT+'/1p.'+SOUND_FORMAT;
		var twop = SOUND_FORMAT+'/2p.'+SOUND_FORMAT;
		var threep = SOUND_FORMAT+'/3p.'+SOUND_FORMAT;
		var fourp = SOUND_FORMAT+'/4p.'+SOUND_FORMAT;
		var fivep = SOUND_FORMAT+'/5p.'+SOUND_FORMAT;
		var sixp = SOUND_FORMAT+'/6p.'+SOUND_FORMAT;
		var sevenp = SOUND_FORMAT+'/7p.'+SOUND_FORMAT;
		var eightp = SOUND_FORMAT+'/8p.'+SOUND_FORMAT;
		var ninep = SOUND_FORMAT+'/9p.'+SOUND_FORMAT;
		var tenp = SOUND_FORMAT+'/10p.'+SOUND_FORMAT;
		
		var nice = 'mp3/nice.mp3';
		var amazing = 'mp3/amazing.mp3';
		var good = 'mp3/prettyGood.mp3';
		var best = 'mp3/best.mp3';
		var bad = 'mp3/bad.mp3';
		
		var youwin = SOUND_FORMAT+'/youwin.'+SOUND_FORMAT;	
		var restart = 'mp3/continue.mp3';
		var endRestart = 'mp3/endRestart.mp3';
		
		var gameOver = false;
		var win = true;
	
		function pointTally() {
			if (!gameOver) {
			
				if (score >= POINTS_PER_LEVEL && win) {
					play_audio(endRestart);
					win = false; 
					gameOver = true;
				} else {
					switch (score) {
						case 0:
							play_audio(zerop);
							break;			
						case 1:
							play_audio(onep);
							break;
						case 2:
							play_audio(twop);
							break;
						case 3:
							play_audio(threep);
							break;
						case 4:
							play_audio(fourp);
							break;		
						case 5:
							play_audio(fivep);
							break;			
						case 6:
							play_audio(sixp);
							break;
						case 7:
							play_audio(sevenp);
							break;
						case 8:
							play_audio(eightp);
							break;
						case 9:
							play_audio(ninep);
							break;	
						case 10:
							play_audio(tenp);
							break;						
						default:
							if (win) { 
								play_audio(youwin);
								setTimeout(function() {
									play_audio(restart);
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
				play_audio(best);
			} else if (score>7) {
				play_audio(amazing);
			} else if (score > 4) {
				play_audio(good);
			} else if (score > 2) {
				play_audio(nice);
			} else {
				play_audio(bad);
			}
		}
		
		function checkLevel() {

			if (score >= POINTS_PER_LEVEL && level < MAX_LEVEL) {
				// level up
				newLevel = true;
				level = level + 1;
				score = 0;
				if (level == MAX_LEVEL) {
					if (!DONT_SAY_LEVEL_UP) { setTimeout(function() {	play_audio(maxLevel); }, 500 ); }
					if (!mobilecheck) { setTimeout(function() { play_audio_vol(mid);     }, 1100 ); }
				} else {
					if (level>0 && !DONT_SAY_LEVEL_UP) { play_audio(levelUp); }
					if (!mobilecheck) { setTimeout(function() { play_audio_vol(mid, mid_volume); }, 1100 ); }
				}
			} else if (score == POINTS_PER_LEVEL)  {
				if (!gameOver) {
					play_audio(youwin); gameOver = true; 
						setTimeout(function() {
							play_audio(restart);
						}, 2500);					
				}
			} else {
				pointTally();
				return false;
			}	
			return true;		
		}
		
		var get = GET();
		// Reading level from address line in browser.
		function getLevel() {
			if ( 'level' in get ) {
				level = parseInt(get['level']);
			}
		}