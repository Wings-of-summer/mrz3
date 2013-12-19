using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdLabMRZ
{
    public class Parser
    {
        private static Dictionary<char, int> SYMBOLS_TO_VALUES = new Dictionary<char, int>() {{'0', 1}, {'-', -1}};

        private static Dictionary<int, char> VALUES_TO_SYMBOLS = new Dictionary<int, char>() {{1, '0'}, {-1, '-'}};

        public static Image FromFile(string file) {
            List<String> lines = ReadFile(file);
            return FromLines(lines);
        }

        public static Image Image(int[,] values, int width, int height) 
        {
            return new Image(values, width, height);
        }

        public static Image FromSymbols(char[,] symbols, int width, int height) 
        {
            int[,] values = new int[height, width];

            for(int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    values[i, j] = SYMBOLS_TO_VALUES[symbols[i, j]];
                }
            }

            return Image(values, width, height);
        }

        public static Image FromLines(List<String> lines) 
        {
            char[,] symbols = new char[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                char[] charSymbols = lines[i].ToCharArray();
                for (int j = 0; j < charSymbols.Length; j++) 
                {
                    symbols[i, j] = charSymbols[j];
                }
            }

            return FromSymbols(symbols, lines[0].Length, lines.Count);
        }

        public static List<String> ToSymbolsLines(Image image) 
        {
            List<String> symbolsList = new List<string>();

            for(int i = 0; i < image.Height; i++) {
                StringBuilder lines = new StringBuilder();
                for (int j = 0; j < image.Width; j++) {
                    lines.Append(ToSymbol(image.GetValue(i, j)));
                }
                symbolsList.Add(lines.ToString());
            }

            return symbolsList;
        }

        private static List<String> ReadFile(string file) 
        {
            List<String> lines = new List<string>();

            string line;

            using (StreamReader streamReader = new StreamReader(file))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        private static char ToSymbol(int value)
        {
            return VALUES_TO_SYMBOLS[value];
        }
    }
}
