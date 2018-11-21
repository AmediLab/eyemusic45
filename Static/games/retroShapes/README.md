Retro Context
=============

Retro context is a pseudo context for HTML5 Canvas providing retro looking graphics:

- No anti-aliasing
- Pixelated graphics and images
- Low resolution
- No alpha channel
- Dithered images
- Indexed palette (comes bundles with 20+ retro palettes)
- Old-school look and feel

Draw:
-----

All draw methods are custom implemented draw operation that allows drawing
with aliased shapes and fills (exception for polygon fill (emulated) and
drawing of images).

The methods covers:

- Lines
- Arcs
- Circles (outlined and filled)
- Ellipses (outlined and filled)
- Rectangles (outlined and filled)
- Polygons (outlined and filled)
- Curves (cardinal, quadratic, Bezier)
- Text using bitmap fonts (fonts included)
- Bucket fill
- Images (with built-in palette reducer and dither: Floyd-Steinberg and Sierra)
- Use indexed palettes (more than 20+ retro palettes included)

<a href="http://epistemex.github.io/retro-context-canvas/docs/index.html">Full API documentation included. Click here to read.</a>

Build
-----

- Classic pixel based paint applications (Deluxe Paint in JS anyone?)
- Sprite editors
- Image converters
- Simple games and applications
- Retro looking user interfaces or UX components

and more.

Usage:
------

Define a standard canvas element in your HTML document:

    <canvas id="myCanvas" width=960 height=600 data-width=320 data-height=200></canvas>

Then obtain a reference to it and get the retro context like this:

    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('retro');

The optional <code>data-width</code> and <code>data-height</code>
represents the "native" retro resolution.

This retro screen is then scaled to the regular size of the canvas
(here 960 x 600, or three times the native resolution - for best
results always scale the native resolution by 2x, 3x or 4x etc.).

You can use the <code>ctx.resolution(w, h)</code> instead of the
data-* attributes to set (or get) the native resolution at any point.

**Full extensive documentation is included as HTML in the docs/ folder.**
Also see the included demos which demonstrates most of the methods.


Status
------

This is an **ALPHA** version.

The API is not fully frozen at this time but it is pretty much set.
Some testing and optimization remains as well as to see if there is
interest from the audience.

We recommend a Blink/V8 based browser for performance reasons (latest
Opera seem to perform best but Chrome do well too).


Requirements
------------

- HTML5 browser with **native** canvas support
- A somewhat capable/newer hardware (making things go slow can take a
surprisingly huge performance hit...)


Snapshots from included demo
============================

The following snapshots are from the first demo included - it differs
a tad from the new one but the graphics is the same. The demo is simple
in nature and is written mostly to test the methods as they where built.

Lines
-----

We are currently using our implementation based on the EFLA Line
algorithm by Po-Han Lin (www.edepot.com/algorithm.html) which is faster
than Bresenham. But the licensing is a bit unclear for libraries such as
this so we are having that in mind.

    ctx.line(x1, y1, x2, y2);

![Lines](http://i.imgur.com/LWsS61R.png)

Rectangles (outlined and filled)
--------------------------------

    ctx.rect(x, y, width, height);

	// Outlined
	ctx .fillColor(null)
	    .penIndex(4)         // index (if palette is set)
		.rect(x, y, width, height);

![Rectangles](http://i.imgur.com/fHmyxhS.png)

Circles (Bresenham, outlined and filled)
----------------------------------------

    ctx.circle(x, y, radius);

![Circles](http://i.imgur.com/Jzk5kMx.png)

Ellipses (Bresenham, outlined and filled)
-----------------------------------------

    ctx.ellipse(x, y, hRadius, vRadius);

![Ellipses](http://i.imgur.com/WYDBYzr.png)

Polygons (optimized lines, outlined and filled)
-----------------------------------------------

    var shape1 = [x1,y1, x2,y2, x3,y3, ...];
	ctx.polygon(shape1);

![Polygons](http://i.imgur.com/9nGBjNx.png)

Bucket-fill
-----------

    ctx .fillColor(255, 128, 0)
	    .bucketFill(x, y);

![Bucket fill](http://i.imgur.com/31KOQqR.png)

Bitmap fonts
------------

    ctx .addFont(fontObject, callback);
    ...

    ctx	.font('c64')
		.text("Hello world", x, y);

![Fonts](http://i.imgur.com/gI4BJAI.png)

Built-in image processing to give that right retro look
-------------------------------------------------------

    ctx.drawImage(image, x, y, width, height, '8bit-sierra');

![Pseudo 8-bit palette Sierra dithered](http://i.imgur.com/4z2vXxg.png)

    ctx .palette('C64')
        .drawImage(image, x, y, width, height, 'palette-fs');

![16 color palette Floyd-Steinberg dithered](http://i.imgur.com/px8Fvx3.png)

    ctx.drawImage(image, x, y, width, height, 'mono-fs');

![Mono-chromatic image Floyd-Steinberg dithered](http://i.imgur.com/Etx4jyC.png)

![Bundled retro palettes](http://i.imgur.com/16iWksE.png)


License
-------

Released under [GPL-3.0 license](http://choosealicense.com/licenses/gpl-v3/).


*&copy; 2013-2014 Epistemex*

![Epistemex](http://i.imgur.com/YxO8CtB.png)