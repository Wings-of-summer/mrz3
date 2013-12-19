using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdLabMRZ
{
    public class Image
    {
        public int Width;

        public int Height;

        private int[,] values;

        public Image(int[,] values, int width, int height) 
        {
            this.Width = width;
            this.Height = height;
            this.values = values;
        }

        public int[] ToVector() 
        {
            int[] vector = new int[Width * Height];

            int n = 0;

            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) 
                {
                    vector[n] = values[i, j];
                    n++;
                }
            }
            return vector;
        }

        public static Image GetImage(int[] values, int width, int height) 
        {
            int[,] valuesArray = new int[height, width];

            int n = 0;

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) 
                {
                    valuesArray[i, j] = values[n];
                    n++;
                }
            }

            return new Image(valuesArray, width, height);
        }

        public int GetValue(int row, int column) 
        {
            return values[row, column];
        }

        public bool Equals(Object o) 
        {
            if (this == o) {
                return true;
            }

            Image image = (Image) o;

            if (Height != image.Height) {
                return false;
            }
            if (Width != image.Width) {
                return false;
            }

            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    if (values[i, j] != image.GetValue(i, j)) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
