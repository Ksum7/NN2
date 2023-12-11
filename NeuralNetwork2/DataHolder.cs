using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace NeuralNetwork2
{
    public class DataHolder
    {
        private Random rand = new Random();

        public SamplesSet trainData = new SamplesSet();
        public SamplesSet testData = new SamplesSet();

        public void loadData(string trainImagesPath, string testImagesPath, string trainMarkup, string testMarkup)
        {
            Dictionary<String, int> trainImageLabels = new Dictionary<String, int>();
            using (StreamReader reader = new StreamReader(trainMarkup))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string trimed = line.Trim();
                    if (trimed != "")
                    {
                        string[] parts = trimed.Split(':');
                        trainImageLabels[parts[0]] = int.Parse(parts[1]);
                    }
                }
            }
            string[] fileEntries = Directory.GetFiles(trainImagesPath);
            foreach (string filePath in fileEntries)
            {
                Bitmap image = new Bitmap(filePath);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
                string fileName = filePath.Split('\\').Last().Split('.').First();
                double[] input = new double[48 * 48];
                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0;
                    int heightInPixels = bmpData.Height;
                    int widthInBytes = bmpData.Stride;
                    for (int y = 0; y < heightInPixels; y++)
                    {
                        for (int x = 0; x < 48; x = x + x + 1)
                        {
                            double grayValue = ptr[(y * bmpData.Stride) + x * (widthInBytes / 48)]/255.0;
                            input[(y * 48) + x] = grayValue;

                        }
                    }
                }
                Sample sample = new Sample(input, 10, (SymbolType)trainImageLabels[fileName]);
                trainData.AddSample(sample);
                image.Dispose();
            }


            Dictionary<String, int> testImageLabels = new Dictionary<String, int>();
            using (StreamReader reader = new StreamReader(testMarkup))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string trimed = line.Trim();
                    if (trimed != "")
                    {
                        string[] parts = trimed.Split(':');
                        testImageLabels[parts[0]] = int.Parse(parts[1]);
                    }
                }
            }
            fileEntries = Directory.GetFiles(testImagesPath);
            foreach (string filePath in fileEntries)
            {
                Bitmap image = new Bitmap(filePath);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
                string fileName = filePath.Split('\\').Last().Split('.').First();
                double[] input = new double[48 * 48];
                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0;
                    int heightInPixels = bmpData.Height;
                    int widthInBytes = bmpData.Stride;
                    for (int y = 0; y < heightInPixels; y++)
                    {
                        for (int x = 0; x < 48; x = x + 1)
                        {
                            double grayValue = ptr[(y * bmpData.Stride) + x * (widthInBytes / 48)] /255.0;
                            input[(y * 48) + x] = grayValue;

                        }
                    }
                }
                Sample sample = new Sample(input, 10, (SymbolType)trainImageLabels[fileName]);
                testData.AddSample(sample);
                image.Dispose();
            }
        }

        public Sample getRandomTestImage()
        {
            return testData[rand.Next(testData.Count)];
        }
    }
}
