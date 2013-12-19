using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThirdLabMRZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string parentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Images\";
        private Network network;
        private List<string> badImageSymbols;
        private List<Image> recognizedImages;
        int curentImageNumber = -1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.InitialDirectory = parentPath;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;

                using (StreamReader badImageStreamReader = new StreamReader(filename))
                {
                    String badImage = badImageStreamReader.ReadToEnd();
                    badImageTextBox.Text = badImage;
                    badImageSymbols = badImage.Split("\r\n".ToCharArray()).Where(s => !s.Equals(String.Empty)).ToList();
                }

                network = new Network(badImageSymbols[0].Length, badImageSymbols.Count);
            }
        }

        private void learnButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.Multiselect = true;
            dialog.InitialDirectory = parentPath;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string[] fileNames = dialog.FileNames;

                if (fileNames.Length > 0)
                {
                    Image[] images = new Image[fileNames.Length];

                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        images[i] = Parser.FromFile(fileNames[i]);
                    }
                    network.Learn(images);
                }
            }
        }

        private void recognizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (network != null)
            {
                Image image = Parser.FromLines(badImageSymbols);
                recognizedImages = network.Recognize(image, 1000);
                recognizedImages.Insert(0, image);
                curentImageNumber = recognizedImages.Count - 1;
                recognizedImageTextBox.Text = CompleteImageText(Parser.ToSymbolsLines(recognizedImages[curentImageNumber]));
                updateNextPreviousButtonState();
            }
        }

        private String CompleteImageText(List<String> lines) 
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (String line in lines) 
            {
                stringBuilder.Append(line + "\r\n");
            }

            return stringBuilder.ToString();
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            curentImageNumber--;
            recognizedImageTextBox.Text = CompleteImageText(Parser.ToSymbolsLines(recognizedImages[curentImageNumber]));
            updateNextPreviousButtonState();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            curentImageNumber++;
            recognizedImageTextBox.Text = CompleteImageText(Parser.ToSymbolsLines(recognizedImages[curentImageNumber]));
            updateNextPreviousButtonState();
        }

        private void updateNextPreviousButtonState()
        {
            if (curentImageNumber <= 0)
            {
                nextButton.IsEnabled = true;
            }
            else
            {
                nextButton.IsEnabled = false;
            }

            if (curentImageNumber >= recognizedImages.Count - 1)
            {
                previousButton.IsEnabled = true;
            }
            else
            {
                previousButton.IsEnabled = false;
            }
        }
    }
}
