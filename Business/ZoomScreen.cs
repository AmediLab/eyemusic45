using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/**
 * intercation: this form is shown when a zoom training is selected or when 'Zoom' is pressed
 * 'UIoriginalImageZoomPictureBox_MouseDown' crops the image around the desired fixation point using 'eyeMusic.cs' functions. 
 *  (This is enabled by using '_myEyeMusic' component.) 
 * 'eyeMusic.cs' sets the image in the current scan picture box when setting it's own current scan
 *  this form is hidden when 'training done' selected or when 'Stop' pressed again
 * */
namespace eyeMusic45
{
    public class ZoomScreen
    {
        // variabels      
        private int _numRows = 4;
        private int _numCols = 4;
        private Size _windowSize;
        private string _currImageName; // used for insert purpose + display purpose
        private Point _position;

        private eyeMusic2 _myEyeMusic;
        private managementGUI _myManager;
        private wavCreator _myWavCreator;

        // functions
        public int NumRows { get { return _numRows; } set { _numRows = value; } }
        public int NumCols { get { return _numCols; } set { _numCols = value; } }
        public Point Position { get { return _position; } set { _position = value; } }
        public Bitmap thisImage;

        //constructor
        public ZoomScreen(eyeMusic2 myEyeMusic, managementGUI myManager, wavCreator myWavCreator)
        {
            _myEyeMusic = myEyeMusic;
            _myManager = myManager;
            _myWavCreator = myWavCreator;
            _position = new Point();
            //this.FormClosing += form_Closing;
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(_myEyeMusic.eyeMusic_KeyDown);
        }

        /// <summary>
        /// sets the current scan image according to the given image clustered or not
        /// </summary>
        /// <param name="image">the image to display</param>
        public void setScanImage(Bitmap image)
        {
            thisImage = managementGUI.resizeImage(image, _myWavCreator.ScanWidth * 5, _myWavCreator.ScanHeight * 5,
                System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
        }

    }
}
