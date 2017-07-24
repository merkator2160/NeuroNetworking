// AForge Framework
// Approximation using Mutli-Layer Neural Network
//
// Copyright © Andrew Kirillov, 2006
// andrew.kirillov@gmail.com
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AForge.Controls;
using AForge.Core;
using AForge.Neuro.Activation_Functions;
using AForge.Neuro.Learning;
using AForge.Neuro.Networks;

namespace Approximation
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : Form
    {
        private GroupBox _groupBox1;
        private ListView _dataList;
        private Button _loadDataButton;
        private ColumnHeader _xColumnHeader;
        private ColumnHeader _yColumnHeader;
        private OpenFileDialog _openFileDialog;
        private GroupBox _groupBox2;
        private AForge.Controls.Chart _chart;
        private GroupBox _groupBox3;
        private TextBox _momentumBox;
        private Label _label6;
        private TextBox _alphaBox;
        private Label _label2;
        private TextBox _learningRateBox;
        private Label _label1;
        private Label _label8;
        private TextBox _iterationsBox;
        private Label _label10;
        private Label _label9;
        private GroupBox _groupBox4;
        private TextBox _currentErrorBox;
        private Label _label3;
        private TextBox _currentIterationBox;
        private Label _label5;
        private Button _stopButton;
        private Button _startButton;
        private Label _label4;
        private TextBox _neuronsBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container _components = null;

        private double[,] _data;

        private double _learningRate = 0.1;
        private double _momentum;
        private double _sigmoidAlphaValue = 2.0;
        private int _neuronsInFirstLayer = 20;
        private int _iterations = 1000;

        private Thread _workerThread;
        private bool _needToStop;

        // Constructor
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // init chart control
            _chart.AddDataSeries("data", Color.Red, Chart.SeriesType.Dots, 5);
            _chart.AddDataSeries("solution", Color.Blue, Chart.SeriesType.Line, 1);

            // init controls
            UpdateSettings();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _groupBox1 = new System.Windows.Forms.GroupBox();
            _dataList = new System.Windows.Forms.ListView();
            _loadDataButton = new System.Windows.Forms.Button();
            _xColumnHeader = new System.Windows.Forms.ColumnHeader();
            _yColumnHeader = new System.Windows.Forms.ColumnHeader();
            _openFileDialog = new System.Windows.Forms.OpenFileDialog();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _chart = new AForge.Controls.Chart();
            _groupBox3 = new System.Windows.Forms.GroupBox();
            _momentumBox = new System.Windows.Forms.TextBox();
            _label6 = new System.Windows.Forms.Label();
            _alphaBox = new System.Windows.Forms.TextBox();
            _label2 = new System.Windows.Forms.Label();
            _learningRateBox = new System.Windows.Forms.TextBox();
            _label1 = new System.Windows.Forms.Label();
            _label8 = new System.Windows.Forms.Label();
            _iterationsBox = new System.Windows.Forms.TextBox();
            _label10 = new System.Windows.Forms.Label();
            _label9 = new System.Windows.Forms.Label();
            _groupBox4 = new System.Windows.Forms.GroupBox();
            _currentErrorBox = new System.Windows.Forms.TextBox();
            _label3 = new System.Windows.Forms.Label();
            _currentIterationBox = new System.Windows.Forms.TextBox();
            _label5 = new System.Windows.Forms.Label();
            _stopButton = new System.Windows.Forms.Button();
            _startButton = new System.Windows.Forms.Button();
            _label4 = new System.Windows.Forms.Label();
            _neuronsBox = new System.Windows.Forms.TextBox();
            _groupBox1.SuspendLayout();
            _groupBox2.SuspendLayout();
            _groupBox3.SuspendLayout();
            _groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            _groupBox1.Controls.Add(_dataList);
            _groupBox1.Controls.Add(_loadDataButton);
            _groupBox1.Location = new System.Drawing.Point(10, 10);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(180, 320);
            _groupBox1.TabIndex = 1;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Data";
            // 
            // dataList
            // 
            _dataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                       _xColumnHeader,
                                                                                       _yColumnHeader});
            _dataList.FullRowSelect = true;
            _dataList.GridLines = true;
            _dataList.Location = new System.Drawing.Point(10, 20);
            _dataList.Name = "_dataList";
            _dataList.Size = new System.Drawing.Size(160, 255);
            _dataList.TabIndex = 0;
            _dataList.View = System.Windows.Forms.View.Details;
            // 
            // loadDataButton
            // 
            _loadDataButton.Location = new System.Drawing.Point(10, 285);
            _loadDataButton.Name = "_loadDataButton";
            _loadDataButton.TabIndex = 1;
            _loadDataButton.Text = "&Load";
            _loadDataButton.Click += new System.EventHandler(loadDataButton_Click);
            // 
            // xColumnHeader
            // 
            _xColumnHeader.Text = "X";
            // 
            // yColumnHeader
            // 
            _yColumnHeader.Text = "Y";
            // 
            // openFileDialog
            // 
            _openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            _openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            _groupBox2.Controls.Add(_chart);
            _groupBox2.Location = new System.Drawing.Point(200, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(300, 320);
            _groupBox2.TabIndex = 2;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Function";
            // 
            // chart
            // 
            _chart.Location = new System.Drawing.Point(10, 20);
            _chart.Name = "_chart";
            _chart.Size = new System.Drawing.Size(280, 290);
            _chart.TabIndex = 0;
            // 
            // groupBox3
            // 
            _groupBox3.Controls.Add(_neuronsBox);
            _groupBox3.Controls.Add(_label4);
            _groupBox3.Controls.Add(_momentumBox);
            _groupBox3.Controls.Add(_label6);
            _groupBox3.Controls.Add(_alphaBox);
            _groupBox3.Controls.Add(_label2);
            _groupBox3.Controls.Add(_learningRateBox);
            _groupBox3.Controls.Add(_label1);
            _groupBox3.Controls.Add(_label8);
            _groupBox3.Controls.Add(_iterationsBox);
            _groupBox3.Controls.Add(_label10);
            _groupBox3.Controls.Add(_label9);
            _groupBox3.Location = new System.Drawing.Point(510, 10);
            _groupBox3.Name = "_groupBox3";
            _groupBox3.Size = new System.Drawing.Size(195, 195);
            _groupBox3.TabIndex = 4;
            _groupBox3.TabStop = false;
            _groupBox3.Text = "Settings";
            // 
            // momentumBox
            // 
            _momentumBox.Location = new System.Drawing.Point(125, 45);
            _momentumBox.Name = "_momentumBox";
            _momentumBox.Size = new System.Drawing.Size(60, 20);
            _momentumBox.TabIndex = 3;
            _momentumBox.Text = "";
            // 
            // label6
            // 
            _label6.Location = new System.Drawing.Point(10, 47);
            _label6.Name = "_label6";
            _label6.Size = new System.Drawing.Size(82, 17);
            _label6.TabIndex = 2;
            _label6.Text = "Momentum:";
            // 
            // alphaBox
            // 
            _alphaBox.Location = new System.Drawing.Point(125, 70);
            _alphaBox.Name = "_alphaBox";
            _alphaBox.Size = new System.Drawing.Size(60, 20);
            _alphaBox.TabIndex = 5;
            _alphaBox.Text = "";
            // 
            // label2
            // 
            _label2.Location = new System.Drawing.Point(10, 72);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(120, 15);
            _label2.TabIndex = 4;
            _label2.Text = "Sigmoid\'s alpha value:";
            // 
            // learningRateBox
            // 
            _learningRateBox.Location = new System.Drawing.Point(125, 20);
            _learningRateBox.Name = "_learningRateBox";
            _learningRateBox.Size = new System.Drawing.Size(60, 20);
            _learningRateBox.TabIndex = 1;
            _learningRateBox.Text = "";
            // 
            // label1
            // 
            _label1.Location = new System.Drawing.Point(10, 22);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(78, 14);
            _label1.TabIndex = 0;
            _label1.Text = "Learning rate:";
            // 
            // label8
            // 
            _label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            _label8.Location = new System.Drawing.Point(10, 147);
            _label8.Name = "_label8";
            _label8.Size = new System.Drawing.Size(175, 2);
            _label8.TabIndex = 22;
            // 
            // iterationsBox
            // 
            _iterationsBox.Location = new System.Drawing.Point(125, 155);
            _iterationsBox.Name = "_iterationsBox";
            _iterationsBox.Size = new System.Drawing.Size(60, 20);
            _iterationsBox.TabIndex = 9;
            _iterationsBox.Text = "";
            // 
            // label10
            // 
            _label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            _label10.Location = new System.Drawing.Point(126, 175);
            _label10.Name = "_label10";
            _label10.Size = new System.Drawing.Size(58, 14);
            _label10.TabIndex = 25;
            _label10.Text = "( 0 - inifinity )";
            // 
            // label9
            // 
            _label9.Location = new System.Drawing.Point(10, 157);
            _label9.Name = "_label9";
            _label9.Size = new System.Drawing.Size(70, 16);
            _label9.TabIndex = 8;
            _label9.Text = "Iterations:";
            // 
            // groupBox4
            // 
            _groupBox4.Controls.Add(_currentErrorBox);
            _groupBox4.Controls.Add(_label3);
            _groupBox4.Controls.Add(_currentIterationBox);
            _groupBox4.Controls.Add(_label5);
            _groupBox4.Location = new System.Drawing.Point(510, 210);
            _groupBox4.Name = "_groupBox4";
            _groupBox4.Size = new System.Drawing.Size(195, 75);
            _groupBox4.TabIndex = 6;
            _groupBox4.TabStop = false;
            _groupBox4.Text = "Current iteration";
            // 
            // currentErrorBox
            // 
            _currentErrorBox.Location = new System.Drawing.Point(125, 45);
            _currentErrorBox.Name = "_currentErrorBox";
            _currentErrorBox.ReadOnly = true;
            _currentErrorBox.Size = new System.Drawing.Size(60, 20);
            _currentErrorBox.TabIndex = 3;
            _currentErrorBox.Text = "";
            // 
            // label3
            // 
            _label3.Location = new System.Drawing.Point(10, 47);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(70, 16);
            _label3.TabIndex = 2;
            _label3.Text = "Error:";
            // 
            // currentIterationBox
            // 
            _currentIterationBox.Location = new System.Drawing.Point(125, 20);
            _currentIterationBox.Name = "_currentIterationBox";
            _currentIterationBox.ReadOnly = true;
            _currentIterationBox.Size = new System.Drawing.Size(60, 20);
            _currentIterationBox.TabIndex = 1;
            _currentIterationBox.Text = "";
            // 
            // label5
            // 
            _label5.Location = new System.Drawing.Point(10, 22);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(70, 16);
            _label5.TabIndex = 0;
            _label5.Text = "Iteration:";
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(630, 305);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 8;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // startButton
            // 
            _startButton.Enabled = false;
            _startButton.Location = new System.Drawing.Point(540, 305);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 7;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
            // 
            // label4
            // 
            _label4.Location = new System.Drawing.Point(10, 97);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(115, 15);
            _label4.TabIndex = 6;
            _label4.Text = "Neurons in first layer:";
            // 
            // neuronsBox
            // 
            _neuronsBox.Location = new System.Drawing.Point(125, 95);
            _neuronsBox.Name = "_neuronsBox";
            _neuronsBox.Size = new System.Drawing.Size(60, 20);
            _neuronsBox.TabIndex = 7;
            _neuronsBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(714, 338);
            Controls.Add(_stopButton);
            Controls.Add(_startButton);
            Controls.Add(_groupBox4);
            Controls.Add(_groupBox3);
            Controls.Add(_groupBox2);
            Controls.Add(_groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Approximation using Multi-Layer Neural Network";
            Closing += new System.ComponentModel.CancelEventHandler(MainForm_Closing);
            _groupBox1.ResumeLayout(false);
            _groupBox2.ResumeLayout(false);
            _groupBox3.ResumeLayout(false);
            _groupBox4.ResumeLayout(false);
            ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        // On main form closing
        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            // check if worker thread is running
            if ((_workerThread != null) && (_workerThread.IsAlive))
            {
                _needToStop = true;
                _workerThread.Join();
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            _learningRateBox.Text = _learningRate.ToString();
            _momentumBox.Text = _momentum.ToString();
            _alphaBox.Text = _sigmoidAlphaValue.ToString();
            _neuronsBox.Text = _neuronsInFirstLayer.ToString();
            _iterationsBox.Text = _iterations.ToString();
        }

        // Load data
        private void loadDataButton_Click(object sender, EventArgs e)
        {
            // show file selection dialog
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                // read maximum 50 points
                var tempData = new double[50, 2];
                var minX = double.MaxValue;
                var maxX = double.MinValue;

                try
                {
                    // open selected file
                    reader = File.OpenText(_openFileDialog.FileName);
                    string str = null;
                    var i = 0;

                    // read the data
                    while ((i < 50) && ((str = reader.ReadLine()) != null))
                    {
                        var strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');
                        // parse X
                        tempData[i, 0] = double.Parse(strs[0]);
                        tempData[i, 1] = double.Parse(strs[1]);

                        // search for min value
                        if (tempData[i, 0] < minX)
                            minX = tempData[i, 0];
                        // search for max value
                        if (tempData[i, 0] > maxX)
                            maxX = tempData[i, 0];

                        i++;
                    }

                    // allocate and set data
                    _data = new double[i, 2];
                    Array.Copy(tempData, 0, _data, 0, i * 2);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }

                // update list and chart
                UpdateDataListView();
                _chart.RangeX = new DoubleRange(minX, maxX);
                _chart.UpdateDataSeries("data", _data);
                _chart.UpdateDataSeries("solution", null);
                // enable "Start" button
                _startButton.Enabled = true;
            }
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all current records
            _dataList.Items.Clear();
            // add new records
            for (int i = 0, n = _data.GetLength(0); i < n; i++)
            {
                _dataList.Items.Add(_data[i, 0].ToString());
                _dataList.Items[i].SubItems.Add(_data[i, 1].ToString());
            }
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            _loadDataButton.Enabled = enable;
            _learningRateBox.Enabled = enable;
            _momentumBox.Enabled = enable;
            _alphaBox.Enabled = enable;
            _neuronsBox.Enabled = enable;
            _iterationsBox.Enabled = enable;

            _startButton.Enabled = enable;
            _stopButton.Enabled = !enable;
        }

        // On button "Start"
        private void startButton_Click(object sender, EventArgs e)
        {
            // get learning rate
            try
            {
                _learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(_learningRateBox.Text)));
            }
            catch
            {
                _learningRate = 0.1;
            }
            // get momentum
            try
            {
                _momentum = Math.Max(0, Math.Min(0.5, double.Parse(_momentumBox.Text)));
            }
            catch
            {
                _momentum = 0;
            }
            // get sigmoid's alpha value
            try
            {
                _sigmoidAlphaValue = Math.Max(0.001, Math.Min(50, double.Parse(_alphaBox.Text)));
            }
            catch
            {
                _sigmoidAlphaValue = 2;
            }
            // get neurons count in first layer
            try
            {
                _neuronsInFirstLayer = Math.Max(5, Math.Min(50, int.Parse(_neuronsBox.Text)));
            }
            catch
            {
                _neuronsInFirstLayer = 20;
            }
            // iterations
            try
            {
                _iterations = Math.Max(0, int.Parse(_iterationsBox.Text));
            }
            catch
            {
                _iterations = 1000;
            }
            // update settings controls
            UpdateSettings();

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            _needToStop = false;
            _workerThread = new Thread(SearchSolution);
            _workerThread.Start();
        }

        // On button "Stop"
        private void stopButton_Click(object sender, EventArgs e)
        {
            // stop worker thread
            _needToStop = true;
            _workerThread.Join();
            _workerThread = null;
        }

        // Worker thread
        void SearchSolution()
        {
            // number of learning samples
            var samples = _data.GetLength(0);
            // data transformation factor
            var yFactor = 1.7 / _chart.RangeY.Length;
            double yMin = _chart.RangeY.Min;
            var xFactor = 2.0 / _chart.RangeX.Length;
            double xMin = _chart.RangeX.Min;

            // prepare learning data
            var input = new double[samples][];
            var output = new double[samples][];

            for (var i = 0; i < samples; i++)
            {
                input[i] = new double[1];
                output[i] = new double[1];

                // set input
                input[i][0] = (_data[i, 0] - xMin) * xFactor - 1.0;
                // set output
                output[i][0] = (_data[i, 1] - yMin) * yFactor - 0.85;
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(_sigmoidAlphaValue),
                1, _neuronsInFirstLayer, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = _learningRate;
            teacher.Momentum = _momentum;

            // iterations
            var iteration = 1;

            // solution array
            var solution = new double[50, 2];
            var networkInput = new double[1];

            // calculate X values to be used with solution function
            for (var j = 0; j < 50; j++)
            {
                solution[j, 0] = _chart.RangeX.Min + (double)j * _chart.RangeX.Length / 49;
            }

            // loop
            while (!_needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;

                // calculate solution
                for (var j = 0; j < 50; j++)
                {
                    networkInput[0] = (solution[j, 0] - xMin) * xFactor - 1.0;
                    solution[j, 1] = (network.Compute(networkInput)[0] + 0.85) / yFactor + yMin;
                }
                _chart.UpdateDataSeries("solution", solution);
                // calculate error
                var learningError = 0.0;
                for (int j = 0, k = _data.GetLength(0); j < k; j++)
                {
                    networkInput[0] = input[j][0];
                    learningError += Math.Abs(_data[j, 1] - ((network.Compute(networkInput)[0] + 0.85) / yFactor + yMin));
                }

                // set current iteration's info
                _currentIterationBox.Text = iteration.ToString();
                _currentErrorBox.Text = learningError.ToString("F3");

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((_iterations != 0) && (iteration > _iterations))
                    break;
            }


            // enable settings controls
            EnableControls(true);
        }
    }
}
