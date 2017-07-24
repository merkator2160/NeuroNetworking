// AForge Framework
// Time Series Prediction using Multi-Layer Neural Network
//
// Copyright © Andrew Kirillov, 2006
// andrew.kirillov@gmail.com
//

using AForge.Controls;
using AForge.Core;
using AForge.Neuro.Activation_Functions;
using AForge.Neuro.Learning;
using AForge.Neuro.Networks;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace TimeSeries
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : Form
    {
        private GroupBox _groupBox1;
        private ListView _dataList;
        private ColumnHeader _yColumnHeader;
        private ColumnHeader _estimatedYColumnHeader;
        private Button _loadDataButton;
        private GroupBox _groupBox2;
        private Chart _chart;
        private OpenFileDialog _openFileDialog;
        private GroupBox _groupBox3;
        private TextBox _momentumBox;
        private Label _label6;
        private TextBox _alphaBox;
        private Label _label2;
        private TextBox _learningRateBox;
        private Label _label1;
        private Label _label10;
        private TextBox _iterationsBox;
        private Label _label9;
        private Label _label8;
        private TextBox _predictionSizeBox;
        private Label _label7;
        private TextBox _windowSizeBox;
        private Label _label3;
        private Label _label5;
        private Button _stopButton;
        private Button _startButton;
        private GroupBox _groupBox4;
        private TextBox _currentPredictionErrorBox;
        private Label _label13;
        private TextBox _currentLearningErrorBox;
        private Label _label12;
        private TextBox _currentIterationBox;
        private Label _label11;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container _components = null;

        private double[] _data;
        private double[,] _dataToShow;

        private double _learningRate = 0.1;
        private double _momentum;
        private double _sigmoidAlphaValue = 2.0;
        private int _windowSize = 5;
        private int _predictionSize = 1;
        private int _iterations = 1000;

        private Thread _workerThread;
        private bool _needToStop;

        private double[,] _windowDelimiter = new double[2, 2] { { 0, 0 }, { 0, 0 } };
        private double[,] _predictionDelimiter = new double[2, 2] { { 0, 0 }, { 0, 0 } };

        // Constructor
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // initializa chart control
            _chart.AddDataSeries("data", Color.Red, Chart.SeriesType.Dots, 5);
            _chart.AddDataSeries("solution", Color.Blue, Chart.SeriesType.Line, 1);
            _chart.AddDataSeries("window", Color.LightGray, Chart.SeriesType.Line, 1, false);
            _chart.AddDataSeries("prediction", Color.Gray, Chart.SeriesType.Line, 1, false);

            // update controls
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
            _yColumnHeader = new System.Windows.Forms.ColumnHeader();
            _estimatedYColumnHeader = new System.Windows.Forms.ColumnHeader();
            _loadDataButton = new System.Windows.Forms.Button();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _chart = new AForge.Controls.Chart();
            _openFileDialog = new System.Windows.Forms.OpenFileDialog();
            _groupBox3 = new System.Windows.Forms.GroupBox();
            _momentumBox = new System.Windows.Forms.TextBox();
            _label6 = new System.Windows.Forms.Label();
            _alphaBox = new System.Windows.Forms.TextBox();
            _label2 = new System.Windows.Forms.Label();
            _learningRateBox = new System.Windows.Forms.TextBox();
            _label1 = new System.Windows.Forms.Label();
            _label8 = new System.Windows.Forms.Label();
            _iterationsBox = new System.Windows.Forms.TextBox();
            _predictionSizeBox = new System.Windows.Forms.TextBox();
            _label7 = new System.Windows.Forms.Label();
            _windowSizeBox = new System.Windows.Forms.TextBox();
            _label3 = new System.Windows.Forms.Label();
            _label10 = new System.Windows.Forms.Label();
            _label9 = new System.Windows.Forms.Label();
            _label5 = new System.Windows.Forms.Label();
            _stopButton = new System.Windows.Forms.Button();
            _startButton = new System.Windows.Forms.Button();
            _groupBox4 = new System.Windows.Forms.GroupBox();
            _currentPredictionErrorBox = new System.Windows.Forms.TextBox();
            _label13 = new System.Windows.Forms.Label();
            _currentLearningErrorBox = new System.Windows.Forms.TextBox();
            _label12 = new System.Windows.Forms.Label();
            _currentIterationBox = new System.Windows.Forms.TextBox();
            _label11 = new System.Windows.Forms.Label();
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
            _groupBox1.Size = new System.Drawing.Size(180, 380);
            _groupBox1.TabIndex = 0;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Data";
            // 
            // dataList
            // 
            _dataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                       _yColumnHeader,
                                                                                       _estimatedYColumnHeader});
            _dataList.FullRowSelect = true;
            _dataList.GridLines = true;
            _dataList.Location = new System.Drawing.Point(10, 20);
            _dataList.Name = "_dataList";
            _dataList.Size = new System.Drawing.Size(160, 315);
            _dataList.TabIndex = 3;
            _dataList.View = System.Windows.Forms.View.Details;
            // 
            // yColumnHeader
            // 
            _yColumnHeader.Text = "Y:Real";
            _yColumnHeader.Width = 70;
            // 
            // estimatedYColumnHeader
            // 
            _estimatedYColumnHeader.Text = "Y:Estimated";
            _estimatedYColumnHeader.Width = 70;
            // 
            // loadDataButton
            // 
            _loadDataButton.Location = new System.Drawing.Point(10, 345);
            _loadDataButton.Name = "_loadDataButton";
            _loadDataButton.TabIndex = 2;
            _loadDataButton.Text = "&Load";
            _loadDataButton.Click += new System.EventHandler(loadDataButton_Click);
            // 
            // groupBox2
            // 
            _groupBox2.Controls.Add(_chart);
            _groupBox2.Location = new System.Drawing.Point(200, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(300, 380);
            _groupBox2.TabIndex = 2;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Function";
            // 
            // chart
            // 
            _chart.Location = new System.Drawing.Point(10, 20);
            _chart.Name = "_chart";
            _chart.Size = new System.Drawing.Size(280, 350);
            _chart.TabIndex = 0;
            // 
            // openFileDialog
            // 
            _openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            _openFileDialog.Title = "Select data file";
            // 
            // groupBox3
            // 
            _groupBox3.Controls.Add(_momentumBox);
            _groupBox3.Controls.Add(_label6);
            _groupBox3.Controls.Add(_alphaBox);
            _groupBox3.Controls.Add(_label2);
            _groupBox3.Controls.Add(_learningRateBox);
            _groupBox3.Controls.Add(_label1);
            _groupBox3.Controls.Add(_label8);
            _groupBox3.Controls.Add(_iterationsBox);
            _groupBox3.Controls.Add(_predictionSizeBox);
            _groupBox3.Controls.Add(_label7);
            _groupBox3.Controls.Add(_windowSizeBox);
            _groupBox3.Controls.Add(_label3);
            _groupBox3.Controls.Add(_label10);
            _groupBox3.Controls.Add(_label9);
            _groupBox3.Controls.Add(_label5);
            _groupBox3.Location = new System.Drawing.Point(510, 10);
            _groupBox3.Name = "_groupBox3";
            _groupBox3.Size = new System.Drawing.Size(195, 205);
            _groupBox3.TabIndex = 3;
            _groupBox3.TabStop = false;
            _groupBox3.Text = "Settings";
            // 
            // momentumBox
            // 
            _momentumBox.Location = new System.Drawing.Point(125, 45);
            _momentumBox.Name = "_momentumBox";
            _momentumBox.Size = new System.Drawing.Size(60, 20);
            _momentumBox.TabIndex = 9;
            _momentumBox.Text = "";
            // 
            // label6
            // 
            _label6.Location = new System.Drawing.Point(10, 47);
            _label6.Name = "_label6";
            _label6.Size = new System.Drawing.Size(82, 17);
            _label6.TabIndex = 8;
            _label6.Text = "Momentum:";
            // 
            // alphaBox
            // 
            _alphaBox.Location = new System.Drawing.Point(125, 70);
            _alphaBox.Name = "_alphaBox";
            _alphaBox.Size = new System.Drawing.Size(60, 20);
            _alphaBox.TabIndex = 11;
            _alphaBox.Text = "";
            // 
            // label2
            // 
            _label2.Location = new System.Drawing.Point(10, 72);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(120, 15);
            _label2.TabIndex = 10;
            _label2.Text = "Sigmoid\'s alpha value:";
            // 
            // learningRateBox
            // 
            _learningRateBox.Location = new System.Drawing.Point(125, 20);
            _learningRateBox.Name = "_learningRateBox";
            _learningRateBox.Size = new System.Drawing.Size(60, 20);
            _learningRateBox.TabIndex = 7;
            _learningRateBox.Text = "";
            // 
            // label1
            // 
            _label1.Location = new System.Drawing.Point(10, 22);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(78, 14);
            _label1.TabIndex = 6;
            _label1.Text = "Learning rate:";
            // 
            // label8
            // 
            _label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            _label8.Location = new System.Drawing.Point(10, 157);
            _label8.Name = "_label8";
            _label8.Size = new System.Drawing.Size(175, 2);
            _label8.TabIndex = 22;
            // 
            // iterationsBox
            // 
            _iterationsBox.Location = new System.Drawing.Point(125, 165);
            _iterationsBox.Name = "_iterationsBox";
            _iterationsBox.Size = new System.Drawing.Size(60, 20);
            _iterationsBox.TabIndex = 24;
            _iterationsBox.Text = "";
            // 
            // predictionSizeBox
            // 
            _predictionSizeBox.Location = new System.Drawing.Point(125, 130);
            _predictionSizeBox.Name = "_predictionSizeBox";
            _predictionSizeBox.Size = new System.Drawing.Size(60, 20);
            _predictionSizeBox.TabIndex = 21;
            _predictionSizeBox.Text = "";
            _predictionSizeBox.TextChanged += new System.EventHandler(predictionSizeBox_TextChanged);
            // 
            // label7
            // 
            _label7.Location = new System.Drawing.Point(10, 132);
            _label7.Name = "_label7";
            _label7.Size = new System.Drawing.Size(90, 16);
            _label7.TabIndex = 20;
            _label7.Text = "Prediction size:";
            // 
            // windowSizeBox
            // 
            _windowSizeBox.Location = new System.Drawing.Point(125, 105);
            _windowSizeBox.Name = "_windowSizeBox";
            _windowSizeBox.Size = new System.Drawing.Size(60, 20);
            _windowSizeBox.TabIndex = 19;
            _windowSizeBox.Text = "";
            _windowSizeBox.TextChanged += new System.EventHandler(windowSizeBox_TextChanged);
            // 
            // label3
            // 
            _label3.Location = new System.Drawing.Point(10, 107);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(80, 16);
            _label3.TabIndex = 18;
            _label3.Text = "Window size:";
            // 
            // label10
            // 
            _label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            _label10.Location = new System.Drawing.Point(126, 185);
            _label10.Name = "_label10";
            _label10.Size = new System.Drawing.Size(58, 14);
            _label10.TabIndex = 25;
            _label10.Text = "( 0 - inifinity )";
            // 
            // label9
            // 
            _label9.Location = new System.Drawing.Point(10, 167);
            _label9.Name = "_label9";
            _label9.Size = new System.Drawing.Size(70, 16);
            _label9.TabIndex = 23;
            _label9.Text = "Iterations:";
            // 
            // label5
            // 
            _label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            _label5.Location = new System.Drawing.Point(10, 97);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(175, 2);
            _label5.TabIndex = 17;
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(630, 360);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 6;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // startButton
            // 
            _startButton.Enabled = false;
            _startButton.Location = new System.Drawing.Point(540, 360);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 5;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
            // 
            // groupBox4
            // 
            _groupBox4.Controls.Add(_currentPredictionErrorBox);
            _groupBox4.Controls.Add(_label13);
            _groupBox4.Controls.Add(_currentLearningErrorBox);
            _groupBox4.Controls.Add(_label12);
            _groupBox4.Controls.Add(_currentIterationBox);
            _groupBox4.Controls.Add(_label11);
            _groupBox4.Location = new System.Drawing.Point(510, 225);
            _groupBox4.Name = "_groupBox4";
            _groupBox4.Size = new System.Drawing.Size(195, 100);
            _groupBox4.TabIndex = 7;
            _groupBox4.TabStop = false;
            _groupBox4.Text = "Current iteration:";
            // 
            // currentPredictionErrorBox
            // 
            _currentPredictionErrorBox.Location = new System.Drawing.Point(125, 70);
            _currentPredictionErrorBox.Name = "_currentPredictionErrorBox";
            _currentPredictionErrorBox.ReadOnly = true;
            _currentPredictionErrorBox.Size = new System.Drawing.Size(60, 20);
            _currentPredictionErrorBox.TabIndex = 5;
            _currentPredictionErrorBox.Text = "";
            // 
            // label13
            // 
            _label13.Location = new System.Drawing.Point(10, 72);
            _label13.Name = "_label13";
            _label13.Size = new System.Drawing.Size(100, 16);
            _label13.TabIndex = 4;
            _label13.Text = "Prediction error:";
            // 
            // currentLearningErrorBox
            // 
            _currentLearningErrorBox.Location = new System.Drawing.Point(125, 45);
            _currentLearningErrorBox.Name = "_currentLearningErrorBox";
            _currentLearningErrorBox.ReadOnly = true;
            _currentLearningErrorBox.Size = new System.Drawing.Size(60, 20);
            _currentLearningErrorBox.TabIndex = 3;
            _currentLearningErrorBox.Text = "";
            // 
            // label12
            // 
            _label12.Location = new System.Drawing.Point(10, 47);
            _label12.Name = "_label12";
            _label12.Size = new System.Drawing.Size(80, 16);
            _label12.TabIndex = 2;
            _label12.Text = "Learning error:";
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
            // label11
            // 
            _label11.Location = new System.Drawing.Point(10, 22);
            _label11.Name = "_label11";
            _label11.Size = new System.Drawing.Size(70, 16);
            _label11.TabIndex = 0;
            _label11.Text = "Iteration:";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(715, 398);
            Controls.Add(_groupBox4);
            Controls.Add(_stopButton);
            Controls.Add(_startButton);
            Controls.Add(_groupBox3);
            Controls.Add(_groupBox2);
            Controls.Add(_groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Time Series Prediction using Multi-Layer Neural Network";
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
            _windowSizeBox.Text = _windowSize.ToString();
            _predictionSizeBox.Text = _predictionSize.ToString();
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
                var tempData = new double[50];

                try
                {
                    // open selected file
                    reader = File.OpenText(_openFileDialog.FileName);
                    string str = null;
                    var i = 0;

                    // read the data
                    while ((i < 50) && ((str = reader.ReadLine()) != null))
                    {
                        // parse the value
                        tempData[i] = double.Parse(str);

                        i++;
                    }

                    // allocate and set data
                    _data = new double[i];
                    _dataToShow = new double[i, 2];
                    Array.Copy(tempData, 0, _data, 0, i);
                    for (var j = 0; j < i; j++)
                    {
                        _dataToShow[j, 0] = j;
                        _dataToShow[j, 1] = _data[j];
                    }
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
                _chart.RangeX = new DoubleRange(0, _data.Length - 1);
                _chart.UpdateDataSeries("data", _dataToShow);
                _chart.UpdateDataSeries("solution", null);
                // set delimiters
                UpdateDelimiters();
                // enable "Start" button
                _startButton.Enabled = true;
            }
        }

        // Update delimiters on the chart
        private void UpdateDelimiters()
        {
            // window delimiter
            _windowDelimiter[0, 0] = _windowDelimiter[1, 0] = _windowSize;
            _windowDelimiter[0, 1] = _chart.RangeY.Min;
            _windowDelimiter[1, 1] = _chart.RangeY.Max;
            _chart.UpdateDataSeries("window", _windowDelimiter);
            // prediction delimiter
            _predictionDelimiter[0, 0] = _predictionDelimiter[1, 0] = _data.Length - 1 - _predictionSize;
            _predictionDelimiter[0, 1] = _chart.RangeY.Min;
            _predictionDelimiter[1, 1] = _chart.RangeY.Max;
            _chart.UpdateDataSeries("prediction", _predictionDelimiter);
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all current records
            _dataList.Items.Clear();
            // add new records
            for (int i = 0, n = _data.GetLength(0); i < n; i++)
            {
                _dataList.Items.Add(_data[i].ToString());
            }
        }

        // Enable/disable controls
        private void EnableControls(bool enable)
        {
            _loadDataButton.Enabled = enable;
            _learningRateBox.Enabled = enable;
            _momentumBox.Enabled = enable;
            _alphaBox.Enabled = enable;
            _windowSizeBox.Enabled = enable;
            _predictionSizeBox.Enabled = enable;
            _iterationsBox.Enabled = enable;

            _startButton.Enabled = enable;
            _stopButton.Enabled = !enable;
        }

        // On window size changed
        private void windowSizeBox_TextChanged(object sender, EventArgs e)
        {
            UpdateWindowSize();
        }

        // On prediction changed
        private void predictionSizeBox_TextChanged(object sender, EventArgs e)
        {
            UpdatePredictionSize();
        }

        // Update window size
        private void UpdateWindowSize()
        {
            if (_data != null)
            {
                // get new window size value
                try
                {
                    _windowSize = Math.Max(1, Math.Min(15, int.Parse(_windowSizeBox.Text)));
                }
                catch
                {
                    _windowSize = 5;
                }
                // check if we have too few data
                if (_windowSize >= _data.Length)
                    _windowSize = 1;
                // update delimiters
                UpdateDelimiters();
            }
        }

        // Update prediction size
        private void UpdatePredictionSize()
        {
            if (_data != null)
            {
                // get new prediction size value
                try
                {
                    _predictionSize = Math.Max(1, Math.Min(10, int.Parse(_predictionSizeBox.Text)));
                }
                catch
                {
                    _predictionSize = 1;
                }
                // check if we have too few data
                if (_data.Length - _predictionSize - 1 < _windowSize)
                    _predictionSize = 1;
                // update delimiters
                UpdateDelimiters();
            }
        }

        // On button "Start"
        private void startButton_Click(object sender, EventArgs e)
        {
            // clear previous solution
            for (int j = 0, n = _data.Length; j < n; j++)
            {
                if (_dataList.Items[j].SubItems.Count > 1)
                    _dataList.Items[j].SubItems.RemoveAt(1);
            }

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
            var samples = _data.Length - _predictionSize - _windowSize;
            // data transformation factor
            var factor = 1.7 / _chart.RangeY.Length;
            double yMin = _chart.RangeY.Min;
            // prepare learning data
            var input = new double[samples][];
            var output = new double[samples][];

            for (var i = 0; i < samples; i++)
            {
                input[i] = new double[_windowSize];
                output[i] = new double[1];

                // set input
                for (var j = 0; j < _windowSize; j++)
                {
                    input[i][j] = (_data[i + j] - yMin) * factor - 0.85;
                }
                // set output
                output[i][0] = (_data[i + _windowSize] - yMin) * factor - 0.85;
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(_sigmoidAlphaValue),
                _windowSize, _windowSize * 2, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = _learningRate;
            teacher.Momentum = _momentum;

            // iterations
            var iteration = 1;

            // solution array
            var solutionSize = _data.Length - _windowSize;
            var solution = new double[solutionSize, 2];
            var networkInput = new double[_windowSize];

            // calculate X values to be used with solution function
            for (var j = 0; j < solutionSize; j++)
            {
                solution[j, 0] = j + _windowSize;
            }

            // loop
            while (!_needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;

                // calculate solution and learning and prediction errors
                var learningError = 0.0;
                var predictionError = 0.0;
                // go through all the data
                for (int i = 0, n = _data.Length - _windowSize; i < n; i++)
                {
                    // put values from current window as network's input
                    for (var j = 0; j < _windowSize; j++)
                    {
                        networkInput[j] = (_data[i + j] - yMin) * factor - 0.85;
                    }

                    // evalue the function
                    solution[i, 1] = (network.Compute(networkInput)[0] + 0.85) / factor + yMin;

                    // calculate prediction error
                    if (i >= n - _predictionSize)
                    {
                        predictionError += Math.Abs(solution[i, 1] - _data[_windowSize + i]);
                    }
                    else
                    {
                        learningError += Math.Abs(solution[i, 1] - _data[_windowSize + i]);
                    }
                }
                // update solution on the chart
                _chart.UpdateDataSeries("solution", solution);

                // set current iteration's info
                _currentIterationBox.Text = iteration.ToString();
                _currentLearningErrorBox.Text = learningError.ToString("F3");
                _currentPredictionErrorBox.Text = predictionError.ToString("F3");

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((_iterations != 0) && (iteration > _iterations))
                    break;
            }

            // show new solution
            for (int j = _windowSize, k = 0, n = _data.Length; j < n; j++, k++)
            {
                _dataList.Items[j].SubItems.Add(solution[k, 1].ToString());
            }

            // enable settings controls
            EnableControls(true);
        }
    }
}
