

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
		
		
	function buildColorString(colorComponent){
		var colorString;
		colorString = 'rgb(' + colorComponent + ',' + colorComponent + ',' + colorComponent + ')';
		return colorString;
	}

		
	//function putTargetAndArrow(context, targetX, targetY, targetRadius, distX, targetY, distRadius){
	function putShapesOnScreen(context, gameLevel, targetX, targetY, targetRadius, distX, targetY, distRadius){

		var arrowDirection = arrowDefaultDirection;
		arrowDirection = Math.floor(Math.random() * 4) + 1; // We add 1 (... + 1) to move result's range from 0-3 to 1-4.
		
		var tempColorString = buildColorString(255 - (48 * gameLevel));
		
		//Target pitchfork points to the direction of the arrow.
		putDirectionArrow(context, arrowDefaultCoordinate_X, arrowDefaultCoordinate_Y, arrowDefaultWidth, arrowDefaultLength, arrowDirection);
		putTargetPitchfork(context, targetX, targetY, targetRadius, arrowDirection, tempColorString);
		
		tempColorString = buildColorString(255 - (48 * (gameLevel - 1))); //Distractor's color will always be 1 level brighter 
		
		//We make sure that distractor pitchfork is always pointing in direction different from the arrow/target.
		var distDirection = Math.floor(Math.random() * 4) + 1; // We add 1 (... + 1) to move result's range from 0-3 to 1-4.
		while (distDirection == arrowDirection){
			var distDirection = Math.floor(Math.random() * 4) + 1; // We add 1 (... + 1) to move result's range from 0-3 to 1-4.
		}
		
		putDistractorPitchfork(context, distX, targetY, distRadius, distDirection, tempColorString);
	}
	
	
	function putTargetPitchfork(context,centerX,centerY,radius, target_direction, shapeColor) {

		//var localXZero = - (windowWidth / 2);
		//var localYZero = - windowHeight;
		
		var localX = (centerX-radius);
		var localY = (centerY-radius);
		
		context.beginPath();
		//Drawing the whole target (the rectangle).
		context.rect((centerX-radius), (centerY-radius), radius*2, radius*2);
		//context.fillStyle = 'red';		
		//context.fillStyle = 'rgb(15,15,15)';
		context.fillStyle = shapeColor;
		context.fill();

		var pitchWidth = ((radius * 2) / 5);
		//Drawing the pitches of the fork on a target's rectangle - according to the direction (of an arrow).
		switch (target_direction) {
			case 1:
				context.clearRect(localX + pitchWidth, localY, pitchWidth, ((radius*2) - pitchWidth));
				context.clearRect(localX + pitchWidth * 3, localY, pitchWidth, ((radius*2) - pitchWidth));
				break;
			case 2:
				context.clearRect(localX + pitchWidth, localY + pitchWidth, (radius*2) - pitchWidth, pitchWidth);
				context.clearRect(localX + pitchWidth, localY + pitchWidth*3, (radius*2) - pitchWidth, pitchWidth);		
				break;
			case 3:
				context.clearRect(localX + pitchWidth, localY + pitchWidth, pitchWidth, ((radius*2) - pitchWidth));
				context.clearRect(localX + pitchWidth * 3, localY + pitchWidth, pitchWidth, ((radius*2) - pitchWidth));
				break;
			case 4:
				context.clearRect(localX, localY + pitchWidth, (radius*2) - pitchWidth, pitchWidth);
				context.clearRect(localX, localY + pitchWidth*3, (radius*2) - pitchWidth, pitchWidth);					
				break;
			default:
				break;					
		}				
	}
		
		
	function putDirectionArrow(context,centerX,centerY,width, length, target_direction) {
	//function putDirectionArrow(context,centerX,centerY,target_direction) {

		var localXZero =  -(windowWidth / 2);
		var localYZero =  -windowHeight;
		//var width  =  arrowDefaultWidth;
		//var length =  arrowDefaultLength;
		//arrowDefaultCoordinate_X		
		
		context.beginPath();
		
		//var message = 'X = ' + (-localXZero) + ' Y = ' + (-localYZero) + ' UPcornerX = ' + (-(localXZero - (centerX -(width / 2)))) + ' UPcornerY = ' + (-(localYZero - (centerY-(length / 2))));
		//writeMessage(canvas, message);

		switch (target_direction) {
			case 1:
				//Drawing arrow body (the rod).
				context.rect((centerX -(width / 2)) + localXZero, (centerY-(length / 2)) + localYZero, width, length);
				//context.rect((centerX -(width / 2)), (centerY-(length / 2)), width, length);
				//Drawing arrow triangle (the spike).
				context.moveTo((centerX - width) + localXZero, (centerY - (length / 2)) + localYZero);
				context.lineTo((centerX + width) + localXZero, (centerY - (length / 2)) + localYZero);
				context.lineTo((centerX) + localXZero, (centerY - (length / 2) - (width*1.5)) + localYZero);
				context.lineTo((centerX - width) + localXZero, (centerY-(length / 2)) + localYZero);
				break;
			case 2:
				//Drawing arrow body (the rod).
				context.rect((centerX -(length / 2)) + localXZero, (centerY-(width / 2)) + localYZero, length, width);
				//Drawing arrow triangle (the spike).
				context.moveTo((centerX + (length / 2)) + localXZero, (centerY - width) + localYZero);
				context.lineTo((centerX + (length / 2)) + localXZero, (centerY + width) + localYZero);
				context.lineTo((centerX + (length / 2) + (width*1.5)) + localXZero, (centerY) + localYZero);
				context.lineTo((centerX + (length / 2)) + localXZero, (centerY - width) + localYZero);
				break;				
			case 3:
				//Drawing arrow body (the rod).
				context.rect((centerX -(width / 2)) + localXZero, (centerY-(length / 2)) + localYZero, width, length);
				//Drawing arrow triangle (the spike).
				context.moveTo((centerX - width) + localXZero, (centerY + (length / 2)) + localYZero);
				context.lineTo((centerX + width) + localXZero, (centerY + (length / 2)) + localYZero);
				context.lineTo((centerX) + localXZero, (centerY + (length / 2) + (width*1.5)) + localYZero);
				context.lineTo((centerX - width) + localXZero, (centerY + (length / 2)) + localYZero);
				break;			
			case 4:
				//Drawing arrow body (the rod).
				context.rect((centerX -(length / 2)) + localXZero, (centerY-(width / 2)) + localYZero, length, width);
				//Drawing arrow triangle (the spike).
				context.moveTo((centerX - (length / 2)) + localXZero, (centerY - width) + localYZero);
				context.lineTo((centerX - (length / 2)) + localXZero, (centerY + width) + localYZero);
				context.lineTo((centerX - (length / 2) - (width*1.5)) + localXZero, (centerY) + localYZero);
				context.lineTo((centerX - (length / 2)) + localXZero, (centerY - width) + localYZero);
				break;							
			default:
				break;					
		}		
			
		context.fillStyle = 'rgb(255,255,255)';
		context.fill();

	}


	function putDistractor(context,centerX,centerY,radius) {
		//putSmiley(x,y,r,Math.floor(Math.random()*3));
		var target_direction = arrowDefaultDirection;
		putDistractorPitchfork(context, centerX, centerY, radius, target_direction);
	}
	  

	function putDistractorPitchfork(context,centerX,centerY,radius, target_direction, shapeColor) {

		var localX = (centerX-radius);
		var localY = (centerY-radius);
		context.beginPath();
		//Drawing the whole distractor (the rectangle).
		context.rect((centerX-radius), (centerY-radius), radius*2, radius*2);
		//context.fillStyle = 'yellow';
		//context.fillStyle = 'rgb(255,255,0)';
		context.fillStyle = shapeColor;
		context.fill();

		var pitchWidth = ((radius * 2) / 5);
		//Drawing the pitches of the fork on a distractor's rectangle.
		switch (target_direction) {
			case 1:
				context.clearRect(localX + pitchWidth, localY, pitchWidth, ((radius*2) - pitchWidth));
				context.clearRect(localX + pitchWidth * 3, localY, pitchWidth, ((radius*2) - pitchWidth));
				break;
			case 2:
				context.clearRect(localX + pitchWidth, localY + pitchWidth, (radius*2) - pitchWidth, pitchWidth);
				context.clearRect(localX + pitchWidth, localY + pitchWidth*3, (radius*2) - pitchWidth, pitchWidth);		
				break;
			case 3:
				context.clearRect(localX + pitchWidth, localY + pitchWidth, pitchWidth, ((radius*2) - pitchWidth));
				context.clearRect(localX + pitchWidth * 3, localY + pitchWidth, pitchWidth, ((radius*2) - pitchWidth));
				break;
			case 4:
				context.clearRect(localX, localY + pitchWidth, (radius*2) - pitchWidth, pitchWidth);
				context.clearRect(localX, localY + pitchWidth*3, (radius*2) - pitchWidth, pitchWidth);					
				break;
			default:
				break;	
 
		}
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

	
    function writeMessage(canvas, message) {

        context.clearRect(-windowWidth / 2, 0 , windowWidth  / 4, -windowHeight / 4);
        context.font = '16pt arial';
        context.fillStyle = 'white';
        context.fillText(message,-windowWidth / 2, -20);
      }	  