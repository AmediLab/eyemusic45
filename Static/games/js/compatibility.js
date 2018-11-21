	$('body').prepend("<div id=\"error\"></div>");
	$('#error').prepend('We are sorry, your screen orientation is not compatible with this game.');
	$('#error').append('<br><br>Try using landscape mode or play with a different device');
	
	var adjustedScreen = false;
	screen_compatibility_check();
	function screen_compatibility_check() {
	
		switch (screen_compatibility) { 
			case 1:
				// if the screen is too narrow shrink it vertically
				if (windowHeight*1.5 > windowWidth) {
					var windowNewHeight = windowWidth/1.5;
					context.translate(0, (windowHeight-windowNewHeight)/2);
					windowHeight = windowNewHeight;
					adjustedScreen = true;
				} else {
					adjustedScreen = false;
				}
				break;
			case 2:
				break;					
			case 3:
				// if the screen is too narrow print an error message 
				if (windowHeight*1.5 > windowWidth) {
					screenSizeError()
					adjustedScreen = true;
				} else {
					screenSizeErrorRemove();
					adjustedScreen = false;
				}
				break;				
		}
	}
				
	function screenSizeError() {
		$('#error').show();
	}

	function screenSizeErrorRemove() {
		$('#error').hide();
	}
