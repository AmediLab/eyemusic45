		var COLORS = ["red","yellow","white","blue"];
		var SHAPES = [square, circle, triangle, exe];
		
		function square(x,y,size,color){    
			context.beginPath();    
			context.rect(x - size/2 ,y - size/2 , size, size);		
			context.fillStyle = color;
			context.fill();
		}

		function circle(x,y,size,color){  
			
			context.beginPath();
			
			context.arc(x, y, size/2, 0, Math.PI*2, true);
			context.fillStyle = color;
			
			context.fill();
		}
		
		function exe(x,y,size,color) {

			var line_width = size / 7;
			var margin = line_width / 2;
			
			context.strokeStyle = color;
			context.lineWidth = line_width;
			
			context.beginPath();
			context.moveTo(x - size/2 + margin, y - size/2 + margin);
			context.lineTo(x + size/2 - margin, y + size/2 - margin);
			context.stroke();			

			context.beginPath();
			context.moveTo(x - size/2 + margin, y+size/2 - margin);
			context.lineTo(x + size/2 - margin, y-size/2 + margin);
			context.stroke();					
		}

		function triangle(x,y,size,color){
		
			context.beginPath();
 
			context.moveTo(x-size/2, y+size/2);
			context.lineTo(x + size/2 , y +size/2);
			context.lineTo(x, y - size/2);
			context.lineTo(x-size/2, y+size/2);

			context.fillStyle = color;
			context.fill();

		}