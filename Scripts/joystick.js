
		// for table design
		// must implement windowWidth, windowHeight
		// goRight, goUp etc.
		
		function joystick() {
			horCells = 7;
			verCells = 5;

			$('body').prepend("<div id=\"joystick\">	</div>");
			$("#joystick").append("<div id=\"table\"></div>");
			var tbl = $("#joystick #table");
			var row;
			for (var tr = 1 ; tr <= verCells ; tr++) {
				tbl.append("<div class=\"tr\" id="+tr+"></div>");
				row = $("#joystick #table .tr").last();
				for (var td = 1 ; td <= horCells ; td++) {
					var text;
					switch (tr) {
						case 5:
							switch (td) {
								case 1:
									text = "down";
									break;
								case 2:
									text = "down";
									break;
								case 6:
									text = "left";
									break;		
								case 7:
									text = "right";
									break;
								default:
									text = "space bar";
									break;
							}
							break;
						case 4:
							switch (td) {
								case 1:
									text = "up";
									break;
								case 2:
									text = "up";
									break;
								case 6:
									text = "left";
									break;		
								case 7:
									text = "right";
									break;
								default:
									text = "screen";
									break;
							}
							break;
						case 1:
							if (td == 1) {
								text = "back";
							} else {
								text = "screen";
							}
							break;
						default:
							text = "screen";
							break;
					}
					row.append("<div class='"+td+"'><a href='#'>"+text+"</a></div>");
				}
			}

			tableCellSize1(horCells,verCells);
		}
		
		function newJoystick() {
			tableCellSize1(horCells,verCells);
		}
		
		function tableCellSize1(horCells,verCells) {
			
			var cellHeight = windowHeight/verCells;
			var cellWidth = windowWidth/horCells;
			
			for (var row=1 ; row<=verCells; row++) {
				for (var col=1 ; col<=horCells; col++) {
					$("#joystick .tr#"+row+" div."+col).css("top",cellHeight*(row-1));
					$("#joystick .tr#"+row+" div."+col).css("left",cellWidth*(col-1));
				}
			}
			$("#joystick .tr div").css("height",cellHeight);
			$("#joystick .tr div").css("width",cellWidth);
		}		
				
	$(document).ready(function() {	
		$('#joystick #4 .1').click(function() {
			goUp();
		});
		
		$('#joystick #4 .2').click(function() {
			goUp();
		});
		
		$('#joystick #5 .1').click(function() {
			goDown();
		});
		$('#joystick #5 .2').click(function() {
			goDown();
		});		
		
		$('#joystick #4 .7').click(function() {
			goRight();
		});
		$('#joystick #5 .7').click(function() {
			goRight();
		});
		
		$('#joystick #4 .6').click(function() {
			goLeft();
		});
		$('#joystick #5 .6').click(function() {
			goLeft();
		});		

		$('#joystick #5 .5').click(function() {
			spaceBar();
		});
		$('#joystick #5 .4').click(function() {
			spaceBar();
		});		
		$('#joystick #5 .3').click(function() {
			spaceBar();
		});			
		
		$('#joystick #1 .1').click(function() {
			window.history.go(-1)
		});				
	});
	
		  
	 
		
	
	
	