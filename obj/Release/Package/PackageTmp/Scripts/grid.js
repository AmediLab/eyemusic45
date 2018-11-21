
		var gridX;
		var gridY;
		var blockX;
		var blockY;
		var colSpace = 15;

		function calcGrid() {
//			gridX = Math.floor(windowWidth/70);
//			gridY = Math.floor(windowHeight/80);
			gridX = 12; // calculated so that each block is 4 pixels in em
			gridY = 7;
		}

		function createGrid(x,y) {
			/** grid is a list of gridX cells. in each cell there is
			a list of gridY cells. each of the inner cells represent a square
			on the screen and holds a number which is interpreted inside the game
			as a type of content **/
			var col;
			var grid = [];
			for (var i=0; i<=x ; i++) {
				col = [];
				for (var j=1; j<=y; j++) {
					col.push(0);
				}
				grid[i] = col;
			}
			
			blockX = Math.floor(windowWidth/gridX);
			blockY = Math.floor(windowHeight/gridY);
			return grid;
		}

		function checkEmpty(x1,x2,y1,y2) {	
			if (x1 == gridX || y1 == gridY || x1 < 1 || y1 < 2) {
				return false;
			}

			for (var i = Math.max(0,x1); i<=Math.min(x2,gridX-1); i++) {
				for (var j = Math.max(0,y1); j<=Math.min(y2,gridY-1); j++) {
					if (grid[i][j] != 0) {
						return false;
					}
				}
			}

			return true;
		}
		
		function clearSquare(x,y,COLOR,BORDER) {
			// BORDER not used
		
			var space = colSpace;
			if (COLOR == BG_COLOR) {
				space = 0;
			}
		
			context.beginPath();
			
			var px = blockX*x;
			var py = blockY*y;			
			context.rect( px+space, py , blockX-2*space , blockY);
			context.fillStyle = COLOR;
			context.fill();
			/*
			context.strokeStyle = BORDER;
			context.lineWidth = 0;
			context.stroke();*/
		}		