README FOR EM GAMES

common functions:

functions goRight, goUp, spaceBar etc:
are called directly from the joystick interface.

clickFunc: 
used for clicking on the screen - with or without the voiceOver interface.
the function should call the correct function according to the location of the click.
(recieves the coordinates)

resize:
should always be implemented and controls what happens when the screen resizes or rotated

paint:
redraw everything that was on the screen upon resize. (usually according to the grid)


common variables:

refractory (boolean):
used for disabling user controls for a period of time

gameStart:
used for click to start games: the game will start when something is clicked.
the variable starts as F and turns to T on click.

MAX_LEVEL:
has the number of the highest level - (usually according to the function level_settings)



