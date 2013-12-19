using Meta.Numerics.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdLabMRZ
{
    class Network
    {
        private RectangularMatrix W;

        public Network(int width, int height) 
        {
            W = new RectangularMatrix(width * height, width * height);
            FillMatrixWithZeros(W);
        }

        public bool Learn(Image[] images) {
            foreach (Image image in images) {
                LearnImage(image);
            }
            return CheckLearn(images);
        }

        public List<Image> Recognize(Image image, int maxIterationsNumber) 
        {
            List<Image> images = new List<Image>();

            Image recognizedImage;

            for (int i = 0; i < maxIterationsNumber; i++) {
                Image currentImage = images.Count > 0 ? images[images.Count - 1] : image;
                Image previousImage = images.Count > 2 ? images[images.Count - 2] : null;

                recognizedImage = RecognizeImage(currentImage);

                if (currentImage.Equals(recognizedImage) || (previousImage != null && previousImage.Equals(recognizedImage))) {
                    break;
                }
                else {
                    images.Add(recognizedImage);
                }
            }

            return images;
        }

        private void FillMatrixWithZeros(RectangularMatrix matrix) 
        {
            for (int i = 0; i < matrix.RowCount; i++) 
            {
                for (int j = 0; j < matrix.ColumnCount; j++) 
                {
                    matrix[i, j] = 0.0;
                }
            }
        }

        private Image RecognizeImage(Image image) 
        {
            int[] vector = image.ToVector();
            RowVector X = new RowVector(vector.Length);

            for (int i = 0; i < X.ColumnCount; i++) 
            {
                X[i] = vector[i];
            }

            ColumnVector sMatrix = W * X.Transpose();

            int[] Y = new int[sMatrix.RowCount];
            for (int i = 0; i < Y.Length; i++) {
                Y[i] = sMatrix[i] > 0 ? 1 : -1;
            }

            return Image.GetImage(Y, image.Width, image.Height);
        }

        private void LearnImage(Image image) 
        {
            int[] vector = image.ToVector();
            ColumnVector X = new ColumnVector(vector.Length);

            for (int i = 0; i < X.RowCount; i++)
            {
                X[i] = vector[i];
            }

            ColumnVector b = W * X;
            ColumnVector a = b - X;

            RectangularMatrix first = a * a.Transpose();
            double second = (X.Transpose() * X) - (X.Transpose() * W * X);
            W = W + Division(first, second);

            for (int i = 0; i < W.ColumnCount; i++) {
                W[i, i] = 0;
            }
        }

        private RectangularMatrix Division(RectangularMatrix first, double second) 
        {
            RectangularMatrix divMatrix = new RectangularMatrix(first.RowCount, first.ColumnCount);

            for (int i = 0; i < divMatrix.RowCount; i++) 
            {
                for (int j = 0; j < divMatrix.ColumnCount; j++) 
                {
                    divMatrix[i, j] = first[i, j] / second;
                }
            }

            return divMatrix;
        }

        private bool CheckLearn(Image[] images) 
        {
            foreach (Image image in images) {
                Image recognizedImage = RecognizeImage(image);
                if (!image.Equals(recognizedImage)) {
                    return false;
                }
            }
            return true;
        }
    }
}
