using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oscilloscope.View
{
    public struct Zoom
    {
        public static readonly Zoom Default = new Zoom(0,1);
        private int _zoomX;
        private int _zoomY;
        private const int MIN_ZOOM_X = 0;
        private const int MIN_ZOOM_Y = 1;
        private const int MAX_ZOOM_X = 3;
        private const int MAX_ZOOM_Y= 3;

        private Zoom(int zoomX, int zoomY)
        {
            _zoomX = zoomX;
            _zoomY = zoomY;

        }

        public void IncreaseX()
        {
            if (_zoomX<MAX_ZOOM_X)
            {
                _zoomX ++;
            }
        }

        public void SetZoomX(int zoom)
        {
            if (zoom <= MAX_ZOOM_X)
            {
                _zoomX=zoom;
            }
            else
            {
                _zoomX = MAX_ZOOM_X;
            }
        }

        public void DecreaseX()
        {
            if (_zoomX > MIN_ZOOM_X)
            {   
                      _zoomX--;
            }
        }
    

        public void IncreaseY()
        {
            if (_zoomY < MAX_ZOOM_Y)
            {
                _zoomY ++;
            }
        }

        public void DecreaseY()
        {
            if (_zoomY > MIN_ZOOM_Y)
            {
                _zoomY --;
            }
        }

        public int ZoomX
        {
            get
            {
                return  (int) Math.Pow(2, _zoomX);
            }
        }

        public int ZoomY
        {
            get { return _zoomY; }
        }

    }
}
