

	function putTarget(context,centerX,centerY,radius) {
		var lineWidth = Math.floor(radius / 6);
	
		  context.beginPath();
		  context.arc(centerX, centerY, radius, 0, 2 * Math.PI, false);
		  context.lineWidth = lineWidth;
		  context.strokeStyle = 'red';
		  context.stroke();
		  
		  context.beginPath();
		  context.arc(centerX, centerY, radius/1.8, 0, 2 * Math.PI, false);
		  context.lineWidth = lineWidth;
		  context.strokeStyle = 'red';
		  context.stroke();		  
		  
		  context.beginPath();
		  context.arc(centerX, centerY, radius/10, 0, 2 * Math.PI, false);
		  context.fillStyle = 'red';
		  context.fill();
		  context.stroke();				  
 
		}
		
	function putTargetSimple(context,centerX,centerY,radius) {

		  context.beginPath();
		  context.arc(centerX, centerY, radius, 0, 2 * Math.PI, false);
		  context.fillStyle = 'red';
		  context.fill();		  
 
		}		
		
	  
    function writeMessage(canvas, message) {

        context.clearRect(-windowWidth / 2, 0 , windowWidth  / 4, -windowHeight / 4);
        context.font = '16pt arial';
        context.fillStyle = 'white';
        context.fillText(message,-windowWidth / 2, -20);
      }	  
	  
	function putSmiley(x,y,r,t) {
	
			context.beginPath();
			context.arc(x, y, r, 0, 2 * Math.PI, false);
			context.fillStyle = 'yellow';
			context.fill();
			
			var faceMiddle = x;
			faceMiddle = faceMiddle + (t-1)*r/2;
			
		   context.strokeStyle = 'black';
		   context.lineWidth = 2;
		   context.beginPath();
		   context.arc(faceMiddle - r/14,y,r/2,0.2*Math.PI,0.8*Math.PI,false);
		   context.stroke();	

		   	context.beginPath();
			context.arc(faceMiddle - r*1/7, y-r/5, 2, 0, 2 * Math.PI, false);
			context.fillStyle = 'black';
			context.fill();
			
		   	context.beginPath();
			context.arc(faceMiddle + r*1/7, y-r/5, 2, 0, 2 * Math.PI, false);
			context.fillStyle = 'black';
			context.fill();		
	}
	  
  