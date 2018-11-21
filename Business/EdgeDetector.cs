using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace eyeMusic45
{    
    class EdgeDetector
    {
        // Constants
        private const int DEF_MAX_THRESHOLD = 40;
        private const int DEF_MIN_THRESHOLD = 10;
        private const int DEF_STRENGTH = 1;
        private const bool DEF_CROP_IMAGE = true;        

        private const int SIGMA = 1;
        private const int KERNEL_SIZE = 5;
        private const int LIMIT = 2;
        private const int EDGE_VALUE = 255;

        // Members
        private Bitmap _origImage;
        private Bitmap _edges;
        private int _highThreshold, _lowThreshold;
        private int _edgeStrength;
        private bool _cropImage;
        private bool _isDirty;
        

        // Static members
        private static int[,] _gaussianKernel;
        private static int _kernelWeigth;
        private static bool _isGaussianInitialized = false;

        // Properties
        public Bitmap OrigImage { get { return _origImage; } set { _origImage = value; _isDirty = true; } }       
        public int HighThreshold { get { return _highThreshold; } set { _highThreshold = value; _isDirty = true; } }
        public int LowThreshold { get { return _lowThreshold; } set { _lowThreshold = value; _isDirty = true; } }
        public int EdgeStrength { get { return _edgeStrength; } set { _edgeStrength = value; _isDirty = true; } }
        public bool CropImage { get { return _cropImage; } set { _cropImage = value; _isDirty = true; } }        
        private int Width { get { return _origImage.Width; } }
        private int Height { get { return _origImage.Height; } }

        // Public Methods

        // Ctors
        public EdgeDetector(Bitmap image) : this(image, DEF_MAX_THRESHOLD,DEF_MIN_THRESHOLD,DEF_STRENGTH, DEF_CROP_IMAGE)
        {
        }

        public EdgeDetector(Bitmap image, int maxThreshold, int minThreshold, int strength, bool cropImage)
        {
            OrigImage = image;
            HighThreshold = maxThreshold;
            LowThreshold = minThreshold;
            EdgeStrength = strength;
            CropImage = cropImage;

            InitGaussianFilter();
        }

        public Bitmap GetImageEdges()
        {    
            if (_isDirty)
            {
                DetectEdges();
            }
            
            return _edges;
        }

        /// <summary>
        /// Detect edges in the original image
        /// </summary>
        private void DetectEdges()
        {
            int[,] edges;
            
            // Turn image to gray scale before starting edge detection
            edges = TurnToGrayscale(OrigImage);

            // Smooth the gray image using a gaussian filter
            edges = GaussianFilter(edges);
            
            // Get gradients and remove the ones which are not local maxima
            edges = NonMaxSuppression(edges);

            // Apply hysteresis thresholding to the image
            edges = ApplyThresholding(edges);

            edges = EnhanceEdges(edges);

            if (CropImage)
                edges = GetCroppedImage(edges);

            _isDirty = false;

            _edges = DisplayImage(edges);           
        }

        /// <summary>
        /// Return a grayscale image of the image parameter
        /// </summary>
        /// <param name="image"></param>
        /// <returns>2 dimensional array of the grayscale image</returns>
        private int[,] TurnToGrayscale(Bitmap image)
        {
            int i, j;
            int[,] grayImage = new int[image.Width, image.Height];              
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            //unsafe
            {

                int size = imageData.Stride * imageData.Height;

                /*Allocate buffer for image*/
                byte[] data = new byte[size];

                /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
                System.Runtime.InteropServices.Marshal.Copy(imageData.Scan0, data, 0, size);

                //byte* imagePointer = (byte*)imageData.Scan0;

                long index = 0;
                for (i = 0; i < imageData.Height; i++)
                {
                    for (j = 0; j < imageData.Width; j++)
                    {
                        grayImage[j, i] = (int)((data[index] + data[index + 1] + data[index + 2]) / 3.0);
                        
                        //4 bytes per pixel
                        index += 4;
                    }

                    //4 bytes per pixel
                    index += imageData.Stride - (imageData.Width * 4);
                }
            }

            // Release memory
            image.UnlockBits(imageData);
            
            return grayImage;
        }

        /// <summary>
        /// Smooth and blur grayscale image using the gaussian filter
        /// </summary>
        private int[,] GaussianFilter(int[,] image)
        {
            int[,] filteredImage = new int[Width, Height];
            float Sum = 0;

            for (int i = LIMIT; i <= ((Width - 1) - LIMIT); i++)
            {
                for (int j = LIMIT; j <= ((Height - 1) - LIMIT); j++)
                {
                    // Add all the pixels around the point [i,j]
                    Sum = 0;
                    for (int k = -LIMIT; k <= LIMIT; k++)
                    {
                        for (int l = -LIMIT; l <= LIMIT; l++)
                        {
                            Sum = Sum + ((float)image[i + k, j + l] * _gaussianKernel[LIMIT + k, LIMIT + l]);

                        }
                    }

                    // Set the value of the pixel by the sum and weight of the kernel
                    filteredImage[i, j] = (int)(Math.Round(Sum / (float)_kernelWeigth));
                }

            }

            return filteredImage;
        }

        /// <summary>
        /// Find the gradient in each pixel, and leave only local maxima, after using gaussian filter
        /// </summary>
        /// <param name="image"></param>
        /// <returns>An image with only local maxima gradients as pixel values</returns>
        private int[,] NonMaxSuppression(int [,] image)
        {
            float[,] DerivativeX, DerivativeY;
            float [,] gradient = new float[Width,Height];            
            int [,] output = new int[Width,Height];
            float tangent;

            // Get image deriviatives using sobel masks 
            int [,] dXMask = {{1,0,-1},
                              {1,0,-1},
                              {1,0,-1}};

            int[,] dYMask = {{1,1,1},
                             {0,0,0},
                             {-1,-1,-1}};

            DerivativeX = ApplyFilter(image, dXMask);
            DerivativeY = ApplyFilter(image, dYMask);

            // Get Gradient size by deriviatives
            for (int i = 0; i <= (Width - 1); i++)
            {
                for (int j = 0; j <= (Height - 1); j++)
                {                    
                    gradient[i, j] = (float)Math.Sqrt((DerivativeX[i, j] * DerivativeX[i, j]) + (DerivativeY[i, j] * DerivativeY[i, j]));
                    output[i, j] = (int)gradient[i, j];
                }
            }

            // Suppress non-maximum gradients by size and direction
            for (int i = LIMIT; i <= (Width - LIMIT) - 1; i++)
            {
                for (int j = LIMIT; j <= (Height - LIMIT) - 1; j++)
                {
                    // get the direction of the gradient
                    if (DerivativeX[i, j] == 0)
                        tangent = 90F;
                    else
                        tangent = (float)(Math.Atan(DerivativeY[i, j] / DerivativeX[i, j]) * 180 / Math.PI); //rad to degree


                    //Horizontal Edge
                    if (((-22.5 < tangent) && (tangent <= 22.5)) || ((157.5 < tangent) && (tangent <= -157.5)))
                    {
                        if ((gradient[i, j] < gradient[i, j + 1]) || (gradient[i, j] < gradient[i, j - 1]))
                            output[i, j] = 0;
                    }


                    //Vertical Edge
                    else if (((-112.5 < tangent) && (tangent <= -67.5)) || ((67.5 < tangent) && (tangent <= 112.5)))
                    {
                        if ((gradient[i, j] < gradient[i + 1, j]) || (gradient[i, j] < gradient[i - 1, j]))
                            output[i, j] = 0;
                    }

                    //+45 Degree Edge
                    else if (((-67.5 < tangent) && (tangent <= -22.5)) || ((112.5 < tangent) && (tangent <= 157.5)))
                    {
                        if ((gradient[i, j] < gradient[i + 1, j - 1]) || (gradient[i, j] < gradient[i - 1, j + 1]))
                            output[i, j] = 0;
                    }

                    //-45 Degree Edge
                    else if (((-157.5 < tangent) && (tangent <= -112.5)) || ((67.5 < tangent) && (tangent <= 22.5)))
                    {
                        if ((gradient[i, j] < gradient[i + 1, j + 1]) || (gradient[i, j] < gradient[i - 1, j - 1]))
                            output[i, j] = 0;
                    }

                }
            }

            return output;
        }

        /// <summary>
        /// Set Edges by low and high thresholds
        /// </summary>
        /// <param name="image">Image with edge strength in each pixel</param>
        /// <returns>An image with edges acoording to the thresholds</returns>
        private int[,] ApplyThresholding(int[,] image)
        {
            int[,] tempImage = new int[Width,Height];
            int[,] output = new int[Width, Height];

            for (int i = LIMIT; i <= (Width - LIMIT) -1; i++)
            {
                for (int j = LIMIT; j <= (Height - LIMIT) - 1; j++)
                {
                    if (image[i, j] >= HighThreshold)
                        tempImage[i, j] = 1;
                    else if (image[i, j] >= LowThreshold)
                        tempImage[i, j] = 2;
                }
            }

            for (int i = LIMIT; i <= (Width - LIMIT) - 1; i++)
            {
                for (int j = LIMIT; j <= (Height - LIMIT) - 1; j++)
                {
                    if (tempImage[i, j] == 1)
                    {
                        output[i, j] = EDGE_VALUE;
                
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Enhances the edges of the image by given strength
        /// </summary>
        private int[,] EnhanceEdges(int[,] image)
        {                        
            // Return original image if there's no  need to strengthen the edges
            if (EdgeStrength <= 0)
            {
                return image; 
            }
            
            int[,] output = new int[Width, Height];
            Array.Copy(image, output, image.Length);
            for (int i = EdgeStrength; i <= (Width - 1) - EdgeStrength; i++)
            {
                for (int j = EdgeStrength; j <= (Height - 1) - EdgeStrength; j++)
                {

                    // Check if there is an edge at this point
                    if (image[i, j] == EDGE_VALUE)
                    {
                        // Strengthen edge around the point
                        for (int k = -EdgeStrength; k <= EdgeStrength; k++)
                        {
                            for (int l = -EdgeStrength; l <= EdgeStrength; l++)
                            {
                                output[i + k, j + l] = EDGE_VALUE;
                            }
                        }
                    }
                }
            }

            return output;
                     
        }

        private float[,] ApplyFilter(int[,] image, int[,] Filter)
        {
            int filterHeight, filterWidth;

            filterWidth = Filter.GetLength(0);
            filterHeight = Filter.GetLength(1);

            float sum = 0;
            float[,] output = new float[Width, Height];

            for (int i = filterWidth / 2; i <= (Width - filterWidth / 2) - 1; i++)
            {
                for (int j = filterHeight / 2; j <= (Height  - filterHeight / 2) - 1; j++)
                {

                    // Set pixel value according to filter
                   sum=0;
                   for(int k=-filterWidth/2; k<=filterWidth/2; k++)
                   {
                       for(int l=-filterHeight/2; l<=filterHeight/2; l++)
                       {
                          sum=sum + image[i+k,j+l]*Filter[filterWidth/2+k,filterHeight/2+l];
                       }
                   }
                   
                    output[i,j]=sum;
                }
            }
            return output;
        }

        /// <summary>
        /// Initialize gaussian filter and weight by sigma = 1
        /// </summary>
        private static void InitGaussianFilter()
        {
            if (_isGaussianInitialized)
                return;

            _gaussianKernel = new int[KERNEL_SIZE, KERNEL_SIZE] {{2,4,5,4,2}, 
                                                                {4,9,12,9,4},
                                                                {5,12,15,12,5},
                                                                {4,9,12,9,4},
                                                                {2,4,5,4,2}};
            _kernelWeigth = 159;

        }

        public Bitmap DisplayImage(int[,] image)
        {
            int i, j;
            int width, height;
            width = image.GetLength(0);
            height = image.GetLength(1);
            Bitmap bmpImage = new Bitmap(width, height);
            BitmapData bitmapData1 = bmpImage.LockBits(new Rectangle(0, 0, width, height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            //unsafe
            {
                int size = bitmapData1.Stride * bitmapData1.Height;

                /*Allocate buffer for image*/
                byte[] data = new byte[size];

                /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
                System.Runtime.InteropServices.Marshal.Copy(bitmapData1.Scan0, data, 0, size);

                long index = 0;


                //byte* imagePointer1 = (byte*)bitmapData1.Scan0;

                for (i = 0; i < bitmapData1.Height; i++)
                {
                    for (j = 0; j < bitmapData1.Width; j++)
                    {
                        // write the logic implementation here
                        data[index] = (byte)image[j, i];
                        data[index + 1]  = (byte)image[j, i];
                        data[index + 2] = (byte)image[j, i];
                        data[index + 3] = (byte)255;
                        
                        //4 bytes per pixel
                        index += 4;
                    }   
                    //4 bytes per pixel
                    index += (bitmapData1.Stride - (bitmapData1.Width * 4));
                }
            }//end unsafe

            bmpImage.UnlockBits(bitmapData1);
            return bmpImage;
        }

        private int[,] GetCroppedImage(int[,] edgesImage)
        {

            int[,] croppedImage;            
            int right=-1, left=-1, upper=-1, lower=-1;

            // Get left index            
            for (int i = 0; i < Width && left == -1; i++)
            {
                for (int j = 0; j < Height && left == -1; j++)
                {
                    if (edgesImage[i, j] != 0)
                        left = i;
                }
            }                     
            if (left == -1)
                return edgesImage;
            left = (left >= 2) ? (left - 2) : (left == 1) ? 0 : left;

            // Get right index
            for (int i = Width-1; i >= 0 && right == -1; i--)
            {
                for (int j = 0; j < Height && right == -1; j++)
                {
                    if (edgesImage[i, j] != 0)
                        right = i;
                }
            }
            right = (right <= Width - 3) ? (right + 2) : (right == Width - 2) ? Width - 1 : right;

            // Get upper index
            for (int i = 0; i < Height && upper == -1; i++)
            {
                for (int j = 0; j < Width && upper == -1; j++)
                {
                    if (edgesImage[j, i] != 0)
                        upper = i;
                }
            }
            upper = (upper >= 2) ? (upper - 2) : (upper == 1) ? 0 : upper;

            // Get lower index
            for (int i = Height-1; i >= 0 && lower == -1; i--)
            {
                for (int j = 0; j < Width && lower == -1; j++)
                {
                    if (edgesImage[j, i] != 0)
                        lower = i;
                }
            }
            lower = (lower <= Height - 3) ? (lower + 2) : (lower == Height - 2) ? Height - 1 : lower;

            // Create the new cropped image
            croppedImage = new int[(right-left)+1, (lower-upper)+1];
            for (int i = 0; i < (right - left) + 1; i++)
            {
                for (int j = 0; j < (lower - upper) + 1; j++)
                {
                    croppedImage[i, j] = edgesImage[i + left, j + upper];
                }
            }

            return croppedImage;

        }

    }
}
