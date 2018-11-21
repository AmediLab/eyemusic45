
		// for table design
		// must implement windowWidth, windowHeight
		function createTable(horCells,verCells) {
			$('body').prepend("<div id=\"touch\">	</div>");
			$("#touch").append("<div id=\"table\"></div>");
			var tbl = $("#touch #table");
			var row;
			for (var tr = 1 ; tr <= verCells ; tr++) {
				tbl.append("<div class=\"tr\" id="+tr+"></div>");
				row = $("#touch #table .tr").last();
				for (var td = 1 ; td <= horCells ; td++) {
					row.append("<div class='"+td+"'><a href='#'>r "+tr+", c "+td+"</a></div>");
				}
			}

			tableCellSize(horCells,verCells);
		}
		
		function tableCellSize(horCells,verCells) {
			
			var cellHeight = windowHeight/verCells;
			var cellWidth = windowWidth/horCells;
			
			for (var row=1 ; row<=verCells; row++) {
				for (var col=1 ; col<=horCells; col++) {
					$("#touch .tr#"+row+" div."+col).css("top",cellHeight*(row-1));
					$("#touch .tr#"+row+" div."+col).css("left",cellWidth*(col-1));
				}
			}
			$("#touch .tr div").css("height",cellHeight);
			$("#touch .tr div").css("width",cellWidth);
		}
		
		function fillTableWithText(text,horCells) {
			var index = 0;
			var tr = 1;
			var td = 1;
			var len = text.length;
			while (index < len) {
				$("#touch #table .tr#"+tr+" div."+td+" a").text(text[index]);
				td++;
				index++;
				if (td > horCells) {
					td = 1;
					tr ++;
				}	
			}
		}

				
	$(document).ready(function() {	
		$('#touch .tr div').click(function() {
		
			var td = $(this);
			var lbl;
			
			if (td.children("a").hasClass("clicked")) {
				td.children("a").removeClass("clicked");
				lbl = 0;
			} else {
				td.children("a").addClass("clicked");
				lbl = 1;
			}
			// horCells, verCells should be defined in the game
			
			var width = windowWidth;
			var tdid = td.attr("class");

			var x = Math.floor(width/horCells * tdid) - width/(2*horCells);
			
			var height = windowHeight;
			var trid = td.parent(".tr").attr("id");
			var y = Math.floor(height/verCells * trid) - height/(2*verCells);		

			// sends the location of the clicked cell, lbl alternating 1/0 (1 first)
			clickFunc(x,y,lbl);	
		});
	});
	
	/* no voiceOver */

		window.addEventListener('mousedown', function(evt) {
			var mpos = getMousePos(canvas,evt);
			x = mpos.x;
			y = mpos.y;
			if (!voiceOver) {
				clickFunc(x,y);
			}
		} , false);
		
		function getMousePos(canvas, evt) {
			var rect = canvas.getBoundingClientRect();
			return {
			  x: evt.clientX - rect.left,
			  y: evt.clientY - rect.top
			};
		  }	
		  