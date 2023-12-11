using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetwork2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NeuralNetworksStand(new Dictionary<string, Func<(int[], int, double), BaseNetwork>>
            {
                {"Accord.Net Perseptron", input => new AccordNet(input.Item1)},
                {"Студентческий персептрон", input => new StudentNetwork(input.Item1, input.Item2, input.Item3)},
            }));
        }
    }
}