	
	function detectmob() { 
	 if( navigator.userAgent.match(/Android/i)
	 || navigator.userAgent.match(/webOS/i)
	 || navigator.userAgent.match(/iPhone/i)
	 || navigator.userAgent.match(/iPad/i)
	 || navigator.userAgent.match(/iPod/i)
	 || navigator.userAgent.match(/BlackBerry/i)
	 || navigator.userAgent.match(/Windows Phone/i)
	 ){
		return true;
	  }
	 else {
	     return true;
		//return false;
	  }
	} mobilecheck = detectmob();

	$(document).ready(function() {

		appversion = check_get_param("appVersion");
	
		if (mobilecheck)
		{
		    dont_check_duration = true;

			$('body').prepend("<div id=\"clickToStart\"></div>");
			$('#clickToStart').prepend(startMessage);
			
			$('#clickToStart').click(function() {
				$(this).remove();
				clickToStart();
			});
		} 
		else 
		{
			newGame();
			gameStart = true;
		}
	});
	
	function clickToStart() {
	    $('#clickToStart').remove();

	    $.ajax({
	        url: '~/../../../../Home/sendStart',
	        contentType: 'application/json; charset=utf-8',
	        type: 'post',
	        data: JSON.stringify({
	            num_level: "1",
	            if_hide: "false",
	            if_HTML: "true"
	        })
	    });

		if (use_EM) { start_EM(); }
		gameStart = true;
		newGame();
	}


	var focusOff = false;

	this.addEventListener('focus', function() {
		if (!mobilecheck) {
			if (gameStart && focusOff) { 

				if (sound_ended)
					{ startEM(); } 
				else 
					{ sound_playing.play(); }
					
				focusOff = false;
			} 
		}
	}, true);
	this.addEventListener("blur", function() { 
		if (!mobilecheck) {
			if (!focusOff) { stopEM(); sound_playing.pause(); focusOff = true; } 
		}
	}, false);
	