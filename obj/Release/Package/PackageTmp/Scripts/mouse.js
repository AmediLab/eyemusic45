		
	if (!voiceOver) {
	
		var x;
		var y;
			
		window.addEventListener('mousedown', function(evt) {
			var mpos = getMousePos(canvas,evt);
			x = mpos.x;
			y = mpos.y;
			alert(x);
			clickFunc(x,y);
		} , false);
		
		function getMousePos(canvas, evt) {
			var rect = canvas.getBoundingClientRect();
			return {
			  x: evt.clientX - rect.left,
			  y: evt.clientY - rect.top
			};
		  }	
		  
	 }