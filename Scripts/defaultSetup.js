
	/* general */
	
	var SOUND_FORMAT = "mp3";
	var voiceOver = true;
	var joystick_control = true;
	var x;
	var y;
	var screen_compatibility = 0;
	var mobilecheck = false;
	
	/* SCREEN SIZE */
		
	var windowWidth;
	var windowHeight;
	updateScreenSize();
	
	function updateScreenSize() {

		// android
		if (navigator.appVersion.indexOf("Linux")!=-1) {
			windowWidth = $(window).width();
			windowHeight = $(window).height();
		} else {
			// other
			windowWidth = window.innerWidth;
			windowHeight = window.innerHeight;
		}

	}

		
	function resize_canvas(canvas){
	
		updateScreenSize();		
		tableCellSize(horCells,verCells);
		
		canvas.width  = windowWidth;
		canvas.height = windowHeight;
		
		if (joystick_control) {
			newJoystick();
		}
		
		if (screen_compatibility > 0) {
			screen_compatibility_check();
		}	
		

	}		
	
	
	/* canvas settings */
	
		var canvas = document.getElementById("game");
		canvas.style.background = 'black';
		canvas.width = windowWidth ;
		canvas.height = windowHeight;
		var context = canvas.getContext('2d');
		
		window.addEventListener('resize', resize , false);
		window.addEventListener('orientationchange', resize, false);				
		
		function clear() {
			context.beginPath();
			context.fillStyle = 'black';
			context.fillRect(-windowWidth , -windowHeight , 2*windowWidth , 2*windowHeight);
		}

		function changeTextColor(c) {
			var stylesheet = document.styleSheets[0],
				selector = ".clicked", rule = "{color: "+c+"}";

			if (stylesheet.insertRule) {
				stylesheet.insertRule(selector + rule, stylesheet.cssRules.length);
			} else if (stylesheet.addRule) {
				stylesheet.addRule(selector, rule, -1);
			}
		}
		