using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day8
    {
        private const int Width = 25;
        private const int Height = 6;
        private const int PixelsInLayer = Width * Height;

        public static string A(string input)
        {
            var offset = 0;
            var numLayers = input.Length / PixelsInLayer;

            var minZeros = int.MaxValue;
            var result = 0;
            for(var i = 0; i < numLayers; i++)
            {
                var layer = input.Substring(offset, PixelsInLayer);
                var zeros = layer.Count(x => x == '0');
                if(zeros < minZeros)
                {
                    var ones = layer.Count(x => x == '1');
                    var twos = layer.Count(x => x == '2');
                    result = ones * twos;
                    minZeros = zeros;
                }
                offset += PixelsInLayer;
            }
            return result.ToString();
        }

        public static string B(string input)
        {
            var offset = 0;
            var numLayers = input.Length / PixelsInLayer;

            var layers = new List<char[,]>();
            for (var i = 0; i < numLayers; i++)
            {
                var layerString = input.Substring(offset, PixelsInLayer);
                var layer = new char[Width, Height];
                var layerOffset = 0;
                for(var h = 0; h < Height; h++)
                {
                    var row = layerString.Substring(layerOffset, Width);
                    for(var w = 0; w < Width; w++)
                    {
                        layer[w, h] = row[w];
                    }
                    layerOffset += Width;
                }
                layers.Add(layer);
                offset += PixelsInLayer;
            }
            
            for (var h = 0; h < Height; h++)
            { 
                for (var w = 0; w < Width; w++)
                {
                    var visibleChar = layers.Select(x => x[w, h]).First(c => c != '2');
                    Console.Write(visibleChar == '0' ? ' ' : '#');
                }
                Console.WriteLine();
            }

            return string.Empty;
        }
    }
}
