/*!
 *	SPONG - sort of a pong clone but created mostly to demo Retro Context
 *
 *	Copyright (c) 2014 Epistemex
 *	www.epistemex.com
*/

/*
	The code herein is merely to show the usage of retro context.
	Ideally the code should be OOP oriented but for the purpose
	of demo we chose to keep it simple and 80's-ish in nature :)


 */

var canvas = document.getElementById('demo'),
	ctx = canvas.getContext('retro'),
	res = ctx.resolution(),
	w = res.width,
	h = res.height,
	cw = (w * 0.5)|0,
	ch = (h * 0.5)| 0
// due to buffering issues in Blink (Chrome/Opera) short samples are only played once.. disabled for now
//	aStart = document.createElement('audio'),
//	aEnd = document.createElement('audio'),
//	aBP = document.createElement('audio'),
//	aBW = document.createElement('audio'),
//	aCnt = 4,
//	audioReady = false
	;

/*aStart.preload =
aEnd.preload =
aBW.preload =
aBP.preload = 'auto';

aStart.addEventListener('canplay', audioLoader, false);
aEnd.addEventListener('canplay', audioLoader, false);
aBW.addEventListener('canplay', audioLoader, false);
aBP.addEventListener('canplay', audioLoader, false);

aStart.src	= aStart.canPlayType('audio/mpeg').replace('no', '').length > 0 ? 'audio/start.mp3' : 'audio/start.ogg';
aEnd.src 	= aStart.canPlayType('audio/mpeg').replace('no', '').length > 0 ? 'audio/end.mp3' : 'audio/end.ogg';
aBW.src 	= aStart.canPlayType('audio/mpeg').replace('no', '').length > 0 ? 'audio/bouncewall.mp3' : 'audio/bouncewall.ogg';
aBP.src		= aStart.canPlayType('audio/mpeg').replace('no', '').length > 0 ? 'audio/bouncepad.mp3' : 'audio/bouncepad.ogg';

function audioLoader() {
	aCnt--;
	audioReady = (aCnt === 0);
}
*/

// load bitmap font first and then go...
ctx	.autoCommit(false)
	.palette('BW')
	.penIndex(1)
	.addFonts([fontC64, fontRetroBig], titleScreen, function() {
		console.log('A font is missng...');
	});

function titleScreen() {

	var highScores = localStorage.getItem('hs');

	if (highScores === null) {
		highScores = ['00000', '00000', '00000'];
		localStorage.setItem('hs', JSON.stringify(highScores));
	}
	else
		highScores = JSON.parse(highScores);

	ctx	.clear()
		.bgIndex(0)
		.fillIndex(0)
		.font('c64')
		.textAlign('center')
		.text('SPONG!', cw, 8)
		//.translate(0, -8)
		.text('for Retro Context', cw, 24)

		.textAlign('left')
		.text('Highscore', 8, 40)
		.text('GEMS', 96, 40)
		.text('1. ' + highScores[0] + '  F - Follow', 16, 56)
		.text('2. ' + highScores[1] + '  H - Helper', 16, 64)
		.text('3. ' + highScores[2] + '  S - Slower', 16, 72)
		.text('P - Points', 96, 80)
		.text('L - Life', 96, 88)
		.text('W - Width', 96, 96)

		.textAlign('left')
		.text('Controls', 8, 96)
		.text('K or A  = UP', 16, 112)
		.text('M or Z  = DOWN', 16, 120)
		.text('SPACE   = PAUSE', 16, 128)
		.text('ESC / Q = QUIT', 16, 136)
		//.text('P       = AUDIO', 16, 144)
		.text('> G       = START GAME <', 0, 144)
		//.translate(0, 8)

		.commit()
	;

	ctx.onkeydown = undefined;
	ctx.onkeyup = function(e) {
		console.log(e);
		if (e.key === 71) { // g
			initGame()
		}
	};

}

function initGame() {

	var	x = cw,
		y = (((h - 20) * Math.random())|0) + 10,
		dx = 3,
		dy = 2,
		maxDY = 6,
		trapCnt = 0,
		isDirty = false,
		isPaused = false,
		toggle = true,
		useSounds = true,
		pause,
		quit = false,

		score = 0,
		scoreInc = 10,
		lives = 5,

		gemWait = 10,
		gemLife = 10,
		gemType = 0,

		//Force, Helper, Life, Points, Slow-down, Widener (weighted)
		gemTypes = 'FFFFFFFFFFFFFFFHHHHHHHHHHHHHHHHHHHLLLLLPPPPPPPPPSSSSSSSSSWWWWWWWWW',
		hasGem = false,
		gemHit = false,
		nextGem = 0,

		isForce = false,
		forceExpire = 0,
		forceToggle = 0,

		isHelper = false,
		helperExpire = 0,

		//slowExpire = 0,
		//slowSpeedX = 0,
		//slowSpeedY = 0,
		gemX,
		gemY,
		bDelta = 0,
		bh = 25,
		by = ((h - bh) * 0.5)|0,
		bSpeed = 4;

	ctx	.clear()
		.fillIndex(1)
		.font('c64')

		// draw new board
		.rect(8, by, 3, bh)
	;

	updateScore();

	ctx.onkeydown = handleKeysDown;
	ctx.onkeyup = handleKeysUp;

	function gameLoop(t) {

		if (isPaused) {
			showPause();
			return;
		}
		else if(quit) {
			titleScreen();
			return;
		}

		handleGems(t);

		// clear old pad
		ctx.clearRect(8, by, 3, bh);

		// clear old ball
		ctx.clearRect(x, y, 3, 3);

		x += dx;
		y += dy;

		// move pad
		if (isForce) {
			if (t > forceExpire - 2000) {
				forceToggle++;
			}
			if (t > forceExpire) {
				isForce = false;
				forceToggle = 0;
			}
			else
				by = (y - bh * 0.5)|0;
		}
		else if (bDelta !== 0) {
			by += bDelta;
		}

		// check helper
		if (isHelper && t > helperExpire) {
			isHelper = false;
		}

		if (by < 0) by = 0;
		if (by > h - bh) by = h - bh;

		// ball at pad?
		if (y >= by && y <= by + bh && x >= 0 && x <= 11) {

			/*if (useSounds) {
				aBP.currentTime = 0;
				aBP.play();
			}*/

			score += scoreInc;

			adjustY();
			adjustSpeeds();
			updateScore();

			dx = -dx;
			x = 11;

			if (isHelper)
				drawTrajectory();
		}

		// whoops!
		if (x < 0) {
			endLive();
			return;
		}

		// update scores if ball erased it (a bit primitive but it works)
		if (y < 10 && (x < 62 || x > w - 62)) {
			isDirty = true;
		}
		else if (y > 9 && isDirty) {
			isDirty = false;
			updateScore();
		}

		if (x > w - 3) {
			dx = -dx;
			if (dx > 0) dx = -dx;
			x = w - 3;
			/*if (useSounds) {
				aBW.currentTime = 0;
				aBW.play();
			}*/
		}

		if (y < 0) {
			dy = -dy;
			if (dy < 0) dy = -dy;
			y = 0;
			/*if (useSounds) {
				aBW.currentTime = 0;
				aBW.play();
			}*/
		}
		else if (y > h - 3) {
			dy = -dy;
			if (dy > 0) dy = -dy;
			y = h -3;
			/*if (useSounds) {
				aBW.currentTime = 0;
				aBW.play();
			}*/
		}

		// draw new ball
		ctx.rect(x, y, 3, 3);

		// draw new pad
		if (forceToggle % 4 === 0)
			ctx.rect(8, by, 3, bh);

		ctx.commit();

		requestAnimationFrame(gameLoop);
	}

	countDown(gameLoop);

	function adjustY() {

		var py = y - by,
			f;

		if (py < 0) py = 0;
		if (py > bh) py = bh;

		//TODO this need some improvement
		f = (0.5 - py / bh) * maxDY + Math.random();
		f = (f + 0.5)|0;

		// prevent ball getting stuck with no delta y
		if (f === 0) {
			trapCnt++;
			f = (Math.random() * 4 - 2)|0;
		}
		else
			trapCnt = 0;

		if (trapCnt > 1) {
			f = 3;
			trapCnt = 0;
		}

		dy = dy < 0 ? -f : f;
	}

	function adjustSpeeds() {

		// shrink board?
		if (score % (scoreInc*10) === 0) {
			bh--;
			if (bh < 10) bh = 10;
		}

		// increase ball x speed?
		if (score % (scoreInc*32) === 0) {
			dx = dx < 0 ? dx - 1 : dx + 1;
			gemLife--;
			if (gemLife < 4) gemLife = 4;
		}

		// increase ball y speed and board speed?
		// give an extra life as well...
		if (score % (scoreInc*50) === 0 && bDelta < 10) {
			bDelta++;
			if (lives < 5) lives++;
		}
	}

	function handleGems(t) {
		if (nextGem === 0) getGemTime(t, gemWait, gemWait * 2);

		if (!hasGem && t > nextGem) {

			getGem();
			getGemTime(t, gemLife, gemLife);

			ctx.fillIndex(0);
			ctx.textAlign('left');
			ctx.text(gemType, gemX, gemY);
			ctx.fillIndex(1);

			hasGem = true;

		} else if (hasGem) {

			if (x >= gemX - 4 && x < gemX + 8 && y >= gemY - 4 && y < gemY + 8) {
				gemHit = true;
				ctx.rect(gemX, gemY, 8, 8).commit();
			}

			if (gemHit || t > nextGem) {

				if (gemHit) {
					// Reward
					switch(gemType) {
						case 'W':
							bh += 5;
							if (bh > 100) bh = 100;
							if (by > h - bh) by = h - bh;
							break;
						case 'P':
							score += (((10 * Math.random())|0) + 5) * scoreInc;
							break;
						case 'S':
							dx = dx < 0 ? -2 : 2;
							dy = dy < 0 ? -2 : 2;
							if (dx < 0) dx = -dx;  // bounce if away
							break;
						case 'L':
							if (lives < 5) lives++;
							break;
						case 'F':
							isForce = true;
							forceExpire = t + 15000;
							break;
						case 'H':
							isHelper = true;
							helperExpire = t + 20000;
							drawTrajectory();
							break;
					}
					updateScore();
				}
				ctx.fillIndex(0);
				ctx.clearRect(gemX, gemY, 8, 8);
				ctx.fillIndex(1);

				hasGem = false;
				gemHit = false;
				getGemTime(t, gemWait, gemWait * 2);
			}
		}
	}

	function getGemTime(t, min, max) {
		nextGem = t + (min + ((max - min + 1) * Math.random())) * 1000;
	}

	function getGem() {
		gemX = (((w - 40) * Math.random()) + 20)|0;
		gemY = (((h - 40) * Math.random()) + 20)|0;
		gemType = gemTypes.charAt((Math.random() * gemTypes.length)|0);
	}

	function resetGem() {
		hasGem = false;
		nextGem = 0;
		forceExpire = 0;
		helperExpire = 0;
	}

	function drawTrajectory() {

		var tx = x,
			ty = y,
			tdx = dx,
			tdy = dy;

		// primitive but functional as we need to plot as well
		for(; tx > 9;) {

			tx += tdx;
			ty += tdy;

			ctx.setPixel(tx + 1, ty + 1);

			if (tx > w - 3) {
				tdx = -tdx;
				if (tdx > 0) tdx = -tdx;
				tx = w - 3;
			}

			if (ty < 0) {
				tdy = -tdy;
				if (tdy < 0) tdy = -tdy;
				ty = 0;
			}
			else if (ty > h - 3) {
				tdy = -tdy;
				if (tdy > 0) tdy = -tdy;
				ty = h -3;
			}
		}
	}

	function updateScore() {

		var l = '', i = 0;
		for(; i < lives; i++) l += '+';

		ctx .fillIndex(0)
			.textAlign('left')
			//.font('c64')
			.text(l + '      ', 20, 2)
			.textAlign('right')
			.text(''+score, w - 20, 2)
			.fillIndex(1);
	}

	function updateHQscore(score) {

		var highScores = localStorage.getItem('hs'),
			sc = '0000' + score;

		sc = sc.substr(sc.length - 5, 5);

		if (highScores === null) {
			highScores = ['00000', '00000', '00000'];
		}
		else
			highScores = JSON.parse(highScores);

		highScores.push(sc);
		highScores.sort(function(a, b) {return a < b});
		highScores.pop();

		localStorage.setItem('hs', JSON.stringify(highScores));
	}

	function endLive() {
		lives--;
		if (lives === 0) {

			updateScore();

			ctx .fillIndex(0)
				.textAlign('center')
				//.font('retrobig')
				.text('GAME OVER', cw, ch)
				.textAlign('left')
				.commit();

			updateHQscore(score);

			setTimeout(titleScreen, 5000);
		}
		else {
			// clear old board
			ctx.clear();

			resetGem();

			x = cw;
			y = (((h - 20) * Math.random())|0) + 10;
			dx = -dx;

			updateScore();

			countDown(gameLoop);
		}
	}

	function countDown(next) {

		var count = 3;

		/*if (useSounds) {
			aStart.currentTime = 0;
			aStart.play();
		}*/

		function counter() {
			ctx .fillIndex(0)
				.textAlign('center')
				//.font('retrobig')
				.text(''+count, cw, ch - 10)
				.commit();

			count--;
			if (count < 0) {
				ctx .clearRect(cw - 4, ch - 10, 8, 8)
					.fillIndex(1);
				requestAnimationFrame(next);
			}
			else
				setTimeout(counter, 1000);
		}
		counter();
	}

	function showPause() {

		if (toggle) {
			ctx .fillIndex(0)
				.textAlign('center')
				.text('PAUSED', cw, ch - 8)
				.fillIndex(1)
				.commit();
		}
		else {
			ctx .fillIndex(0)
				.clearRect(cw - 30, ch - 8, 60, 8)
				.fillIndex(1)
				.commit();
		}

		toggle = !toggle;

		if (isPaused)
			pause = setTimeout(showPause, 500);

		else {
			ctx .fillIndex(0)
				.clearRect(cw - 30, ch - 8, 60, 8)
				.fillIndex(1)
				.commit();

			// reset gems
			nextGem = 0;
			hasGem = false;
			gemHit = false;

			requestAnimationFrame(gameLoop);
		}
	}

	function handleKeysDown(e) {

		if (e.key === 75 || e.key === 65) { // k or a
			bDelta = -bSpeed;
		}
		else if (e.key === 77 || e.key === 90) { // m or z
			bDelta = bSpeed;
		}
		else if (e.key === 32) {  // SPC
			isPaused = !isPaused;
			toggle = true;
		}
		else if (e.key === 27 || e.key === 81) {  // ESC or Q
			quit = true;
		}
		/*else if (e.key === 80) {  // p
			useSounds = !useSounds;
		}*/
	}

	function handleKeysUp(e) {

		if (e.key === 75 || e.key === 65) { // k or a
			if (bDelta < 0) bDelta = 0;
		}
		else if (e.key === 77 || e.key === 90) { // m or z
			if (bDelta > 0) bDelta = 0;
		}
	}

}
