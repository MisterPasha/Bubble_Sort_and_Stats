using System;
using System.IO;
using System.Windows.Forms;

namespace StatisticProgram
{
    public partial class frmStats : Form
    {
        private int[] rawData;

        private void bubbleSort()
        {
            bool sorted = false;
            int temp;

            do
            {
                sorted = true;
                for (int i = 0; i < rawData.Length - 1; i++)
                {
                    if (rawData[i] > rawData[i + 1])
                    {
                        temp = rawData[i];
                        rawData[i] = rawData[i + 1];
                        rawData[i + 1] = temp;
                        sorted = false;
                    }
                }
            }
            while (!sorted);      
        }

        private void output()
        {
            lstRawData.Items.Clear();
            for (int i = 0; i < rawData.Length; i++)
            {
                lstRawData.Items.Add(rawData[i]);
            }
        }

        public frmStats()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Populate(int n)
        {
            rawData = new int[Convert.ToInt32(txtNumberOfItems.Text)];
            Random randomGen = new Random();
            for (int i = 0; i<n; i++)
            {
                rawData[i] = randomGen.Next(1, 1000);
            }
        }

        private void btnGenData_Click(object sender, EventArgs e)
        {
            
            if (txtNumberOfItems.Text == "")
            {
                lstRawData.Items.Clear();
                lstRawData.Items.Add("Please enter a number to generate");
            }
            else
            {
                Populate(Convert.ToInt32(txtNumberOfItems.Text));
            }
        }

        private void btnOutputData_Click(object sender, EventArgs e)
        {
            output();
        }

        private double mean()
        {
            int numberOfItems = 0;
            int total = 0;
            foreach (int i in lstRawData.Items)
            {
                numberOfItems++;
                total += i;
            }

            return total/numberOfItems;
        }

        private void btnMean_Click(object sender, EventArgs e)
        {
            double meanNumber = mean();
            lblResult.Text = $"Mean: {meanNumber}";
        }

        private void bntBubble_Click(object sender, EventArgs e)
        {
            bubbleSort();
            output();
        }

        private int range()
        {
            bubbleSort();
            return rawData[rawData.Length - 1] - rawData[0];
        }

        private void btnRange_Click(object sender, EventArgs e)
        {
            lblResult.Text = $"Range: {range()}";
        }

        private double median()
        {
            double mediumNumber = 0;
            if(rawData.Length % 2 !=0)
            {
                mediumNumber = rawData.Length / 2 + 0.5;
                return rawData[Convert.ToInt32(mediumNumber)];
            }
            else
            {
                mediumNumber = (rawData[rawData.Length / 2] + rawData[rawData.Length / 2 - 1]) / 2;
                return mediumNumber;
            }
            
        }

        private void btnMedian_Click(object sender, EventArgs e)
        {
            bubbleSort();
            lblResult.Text = $"Median: {median()}";
        }

        private void loadFromFile()
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    int numberOfLines = File.ReadAllLines(filePath).Length;
                    rawData = new int[numberOfLines];
                    System.IO.StreamReader file = new System.IO.StreamReader(filePath);


                    for (int i = 0; i < numberOfLines; i++)
                    {
                        rawData[i] = Convert.ToInt32(file.ReadLine());
                    }
                    file.Close();
                }
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            loadFromFile();
            output();
        }

        private double CalculateStandardDeviation()
        {
            double meanNumber = mean();
            int numberOfitems = 0;
            double total = 0;
            foreach (int i in lstRawData.Items)
            {
                numberOfitems++;
                total += Math.Pow(i - meanNumber, 2);
            }
            double variance = total / numberOfitems;

            return Math.Sqrt(variance);
        }

        private void btnDeviation_Click(object sender, EventArgs e)
        {
            lblResult.Text = $"Standard Deviation: {CalculateStandardDeviation():0.00}";
        }

        private double interquartileRange()
        {
            int[] firstHalfArray;
            int[] secondHalfArray;
            int mediumFirstHalf = 0;
            int mediumSecondHalf = 0;

            if (rawData.Length %2==0)
            {
                int mediumRawData = rawData.Length / 2;
                firstHalfArray = new int[mediumRawData];
                secondHalfArray = new int[mediumRawData];
                for (int i =0; i<mediumRawData - 1; i++)
                {
                    firstHalfArray[i] = rawData[i];
                }
                for (int i = mediumRawData; i < rawData.Length; i++)
                {
                    secondHalfArray[i] = rawData[i];
                }

                if(firstHalfArray.Length % 2==0)
                {
                    mediumFirstHalf = (firstHalfArray[firstHalfArray.Length / 2] + firstHalfArray[firstHalfArray.Length / 2 - 1]) / 2 ;
                    mediumSecondHalf = (secondHalfArray[secondHalfArray.Length / 2] + secondHalfArray[secondHalfArray.Length / - 1]) / 2;
                }
                else
                {
                    mediumFirstHalf = firstHalfArray[Convert.ToInt32(firstHalfArray.Length / 2 + 0.5)];
                    mediumSecondHalf = secondHalfArray[Convert.ToInt32(secondHalfArray.Length / 2 + 0.5)];
                }
            }
            else
            {
                int halfArrayLen = Convert.ToInt32(rawData.Length / 2 - 0.5);
                firstHalfArray = new int[halfArrayLen];
                secondHalfArray = new int[halfArrayLen];

                if (firstHalfArray.Length % 2 == 0)
                {
                    mediumFirstHalf = (firstHalfArray[firstHalfArray.Length / 2] + firstHalfArray[firstHalfArray.Length / 2 - 1]) / 2;
                    mediumSecondHalf = (secondHalfArray[secondHalfArray.Length / 2] + secondHalfArray[secondHalfArray.Length / 2 -1]) / 2;
                }
                else
                {
                    mediumFirstHalf = firstHalfArray[Convert.ToInt32(firstHalfArray.Length / 2 + 0.5)];
                    mediumSecondHalf = secondHalfArray[Convert.ToInt32(secondHalfArray.Length / 2 + 0.5)];
                }
            }
            return mediumSecondHalf - mediumFirstHalf;
        }

        private void btnInter_Click(object sender, EventArgs e)
        {
            lblResult.Text = $"Interquartile Range: {interquartileRange()}";
        }
    }
}
