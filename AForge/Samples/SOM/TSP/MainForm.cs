// AForge Framework
// Traveling Salesman Problem using Elastic Net
//
// Copyright © Andrew Kirillov, 2006
// andrew.kirillov@gmail.com
//

using AForge.Controls;
using AForge.Core;
using AForge.Neuro.Learning;
using AForge.Neuro.Networks;
using AForge.Neuro.Neurons;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TSP
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : Form
    {
        private GroupBox _groupBox1;
        private Button _generateMapButton;
        private TextBox _citiesCountBox;
        private Label _label1;
        private GroupBox _groupBox2;
        private Label _label2;
        private TextBox _neuronsBox;
        private Label _label3;
        private TextBox _currentIterationBox;
        private Label _label8;
        private Label _label7;
        private TextBox _rateBox;
        private Label _label5;
        private TextBox _iterationsBox;
        private Label _label6;
        private Button _stopButton;
        private Button _startButton;
        private Chart _chart;
        private Label _label4;
        private TextBox _radiusBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container _components = null;

        private int _citiesCount = 10;
        private int _neurons = 20;
        private int _iterations = 500;
        private double _learningRate = 0.5;
        private double _learningRadius = 0.5;

        private double[,] _map;
        private Random _rand = new Random();

        private Thread _workerThread;
        private bool _needToStop;

        // Constructor
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // initialize chart
            _chart.AddDataSeries("cities", Color.Red, Chart.SeriesType.Dots, 5, false);
            _chart.AddDataSeries("path", Color.Blue, Chart.SeriesType.Line, 1, false);
            _chart.RangeX = new DoubleRange(0, 1000);
            _chart.RangeY = new DoubleRange(0, 1000);

            //
            UpdateSettings();
            GenerateMap();
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
            _generateMapButton = new System.Windows.Forms.Button();
            _citiesCountBox = new System.Windows.Forms.TextBox();
            _label1 = new System.Windows.Forms.Label();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _stopButton = new System.Windows.Forms.Button();
            _startButton = new System.Windows.Forms.Button();
            _currentIterationBox = new System.Windows.Forms.TextBox();
            _label8 = new System.Windows.Forms.Label();
            _label7 = new System.Windows.Forms.Label();
            _rateBox = new System.Windows.Forms.TextBox();
            _label5 = new System.Windows.Forms.Label();
            _iterationsBox = new System.Windows.Forms.TextBox();
            _label6 = new System.Windows.Forms.Label();
            _label3 = new System.Windows.Forms.Label();
            _neuronsBox = new System.Windows.Forms.TextBox();
            _label2 = new System.Windows.Forms.Label();
            _label4 = new System.Windows.Forms.Label();
            _radiusBox = new System.Windows.Forms.TextBox();
            _groupBox1.SuspendLayout();
            _groupBox2.SuspendLayout();
            SuspendLayout();
            _chart = new AForge.Controls.Chart();
            // 
            // groupBox1
            // 
            _groupBox1.Controls.Add(_generateMapButton);
            _groupBox1.Controls.Add(_citiesCountBox);
            _groupBox1.Controls.Add(_label1);
            _groupBox1.Controls.Add(_chart);
            _groupBox1.Location = new System.Drawing.Point(10, 10);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(300, 340);
            _groupBox1.TabIndex = 1;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Map";
            // 
            // generateMapButton
            // 
            _generateMapButton.Location = new System.Drawing.Point(110, 309);
            _generateMapButton.Name = "_generateMapButton";
            _generateMapButton.Size = new System.Drawing.Size(75, 22);
            _generateMapButton.TabIndex = 3;
            _generateMapButton.Text = "&Generate";
            _generateMapButton.Click += new System.EventHandler(generateMapButton_Click);
            // 
            // citiesCountBox
            // 
            _citiesCountBox.Location = new System.Drawing.Point(50, 310);
            _citiesCountBox.Name = "_citiesCountBox";
            _citiesCountBox.Size = new System.Drawing.Size(50, 20);
            _citiesCountBox.TabIndex = 2;
            _citiesCountBox.Text = "";
            // 
            // label1
            // 
            _label1.Location = new System.Drawing.Point(10, 312);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(40, 16);
            _label1.TabIndex = 1;
            _label1.Text = "Cities:";
            // 
            // chart
            // 
            _chart.Location = new System.Drawing.Point(10, 20);
            _chart.Name = "_chart";
            _chart.Size = new System.Drawing.Size(280, 280);
            _chart.TabIndex = 4;
            // 
            // groupBox2
            // 
            _groupBox2.Controls.Add(_radiusBox);
            _groupBox2.Controls.Add(_label4);
            _groupBox2.Controls.Add(_stopButton);
            _groupBox2.Controls.Add(_startButton);
            _groupBox2.Controls.Add(_currentIterationBox);
            _groupBox2.Controls.Add(_label8);
            _groupBox2.Controls.Add(_label7);
            _groupBox2.Controls.Add(_rateBox);
            _groupBox2.Controls.Add(_label5);
            _groupBox2.Controls.Add(_iterationsBox);
            _groupBox2.Controls.Add(_label6);
            _groupBox2.Controls.Add(_label3);
            _groupBox2.Controls.Add(_neuronsBox);
            _groupBox2.Controls.Add(_label2);
            _groupBox2.Location = new System.Drawing.Point(320, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(180, 340);
            _groupBox2.TabIndex = 2;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Neural Network";
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(95, 305);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 23;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // startButton
            // 
            _startButton.Location = new System.Drawing.Point(10, 305);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 22;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
            // 
            // currentIterationBox
            // 
            _currentIterationBox.Location = new System.Drawing.Point(110, 150);
            _currentIterationBox.Name = "_currentIterationBox";
            _currentIterationBox.ReadOnly = true;
            _currentIterationBox.Size = new System.Drawing.Size(60, 20);
            _currentIterationBox.TabIndex = 21;
            _currentIterationBox.Text = "";
            // 
            // label8
            // 
            _label8.Location = new System.Drawing.Point(10, 152);
            _label8.Name = "_label8";
            _label8.Size = new System.Drawing.Size(100, 16);
            _label8.TabIndex = 20;
            _label8.Text = "Curren iteration:";
            // 
            // label7
            // 
            _label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label7.Location = new System.Drawing.Point(10, 139);
            _label7.Name = "_label7";
            _label7.Size = new System.Drawing.Size(160, 2);
            _label7.TabIndex = 19;
            // 
            // rateBox
            // 
            _rateBox.Location = new System.Drawing.Point(110, 85);
            _rateBox.Name = "_rateBox";
            _rateBox.Size = new System.Drawing.Size(60, 20);
            _rateBox.TabIndex = 18;
            _rateBox.Text = "";
            // 
            // label5
            // 
            _label5.Location = new System.Drawing.Point(10, 87);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(100, 16);
            _label5.TabIndex = 17;
            _label5.Text = "Initial learning rate:";
            // 
            // iterationsBox
            // 
            _iterationsBox.Location = new System.Drawing.Point(110, 60);
            _iterationsBox.Name = "_iterationsBox";
            _iterationsBox.Size = new System.Drawing.Size(60, 20);
            _iterationsBox.TabIndex = 16;
            _iterationsBox.Text = "";
            // 
            // label6
            // 
            _label6.Location = new System.Drawing.Point(10, 62);
            _label6.Name = "_label6";
            _label6.Size = new System.Drawing.Size(60, 16);
            _label6.TabIndex = 15;
            _label6.Text = "Iteraions:";
            // 
            // label3
            // 
            _label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label3.Location = new System.Drawing.Point(10, 48);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(160, 2);
            _label3.TabIndex = 4;
            // 
            // neuronsBox
            // 
            _neuronsBox.Location = new System.Drawing.Point(110, 20);
            _neuronsBox.Name = "_neuronsBox";
            _neuronsBox.Size = new System.Drawing.Size(60, 20);
            _neuronsBox.TabIndex = 1;
            _neuronsBox.Text = "";
            // 
            // label2
            // 
            _label2.Location = new System.Drawing.Point(10, 22);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(60, 16);
            _label2.TabIndex = 0;
            _label2.Text = "Neurons:";
            // 
            // label4
            // 
            _label4.Location = new System.Drawing.Point(10, 112);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(100, 16);
            _label4.TabIndex = 24;
            _label4.Text = "Learning radius:";
            // 
            // radiusBox
            // 
            _radiusBox.Location = new System.Drawing.Point(110, 110);
            _radiusBox.Name = "_radiusBox";
            _radiusBox.Size = new System.Drawing.Size(60, 20);
            _radiusBox.TabIndex = 25;
            _radiusBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(509, 360);
            Controls.Add(_groupBox2);
            Controls.Add(_groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Traveling Salesman Problem using Elastic Net";
            Closing += new System.ComponentModel.CancelEventHandler(MainForm_Closing);
            _groupBox1.ResumeLayout(false);
            _groupBox2.ResumeLayout(false);
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
            _citiesCountBox.Text = _citiesCount.ToString();
            _neuronsBox.Text = _neurons.ToString();
            _iterationsBox.Text = _iterations.ToString();
            _rateBox.Text = _learningRate.ToString();
            _radiusBox.Text = _learningRadius.ToString();
        }

        // Generate new map for the Traivaling Salesman problem
        private void GenerateMap()
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            // create coordinates array
            _map = new double[_citiesCount, 2];

            for (var i = 0; i < _citiesCount; i++)
            {
                _map[i, 0] = rand.Next(1001);
                _map[i, 1] = rand.Next(1001);
            }

            // set the map
            _chart.UpdateDataSeries("cities", _map);
            // erase path if it is
            _chart.UpdateDataSeries("path", null);
        }

        // On "Generate" button click - generate map
        private void generateMapButton_Click(object sender, EventArgs e)
        {
            // get cities count
            try
            {
                _citiesCount = Math.Max(5, Math.Min(50, int.Parse(_citiesCountBox.Text)));
            }
            catch
            {
                _citiesCount = 20;
            }
            _citiesCountBox.Text = _citiesCount.ToString();

            // regenerate map
            GenerateMap();
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            _neuronsBox.Enabled = enable;
            _iterationsBox.Enabled = enable;
            _rateBox.Enabled = enable;
            _radiusBox.Enabled = enable;

            _startButton.Enabled = enable;
            _generateMapButton.Enabled = enable;
            _stopButton.Enabled = !enable;
        }

        // On "Start" button click
        private void startButton_Click(object sender, EventArgs e)
        {
            // get network size
            try
            {
                _neurons = Math.Max(5, Math.Min(50, int.Parse(_neuronsBox.Text)));
            }
            catch
            {
                _neurons = 20;
            }
            // get iterations count
            try
            {
                _iterations = Math.Max(10, Math.Min(1000000, int.Parse(_iterationsBox.Text)));
            }
            catch
            {
                _iterations = 500;
            }
            // get learning rate
            try
            {
                _learningRate = Math.Max(0.00001, Math.Min(1.0, double.Parse(_rateBox.Text)));
            }
            catch
            {
                _learningRate = 0.5;
            }
            // get learning radius
            try
            {
                _learningRadius = Math.Max(0.00001, Math.Min(1.0, double.Parse(_radiusBox.Text)));
            }
            catch
            {
                _learningRadius = 0.5;
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

        // On "Stop" button click
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
            // set random generators range
            NeuronBase.RandRange = new DoubleRange(0, 1000);

            // create network
            var network = new DistanceNetwork(2, _neurons);

            // create learning algorithm
            var trainer = new ElasticNetworkLearning(network);

            var fixedLearningRate = _learningRate / 20;
            var driftingLearningRate = fixedLearningRate * 19;

            // path
            var path = new double[_neurons + 1, 2];

            // input
            var input = new double[2];

            // iterations
            var i = 0;

            // loop
            while (!_needToStop)
            {
                // update learning speed & radius
                trainer.LearningRate = driftingLearningRate * (_iterations - i) / _iterations + fixedLearningRate;
                trainer.LearningRadius = _learningRadius * (_iterations - i) / _iterations;

                // set network input
                var currentCity = _rand.Next(_citiesCount);
                input[0] = _map[currentCity, 0];
                input[1] = _map[currentCity, 1];

                // run one training iteration
                trainer.Run(input);

                // show current path
                for (var j = 0; j < _neurons; j++)
                {
                    path[j, 0] = network[0][j][0];
                    path[j, 1] = network[0][j][1];
                }
                path[_neurons, 0] = network[0][0][0];
                path[_neurons, 1] = network[0][0][1];

                _chart.UpdateDataSeries("path", path);

                // increase current iteration
                i++;

                // set current iteration's info
                _currentIterationBox.Text = i.ToString();

                // stop ?
                if (i >= _iterations)
                    break;
            }

            // enable settings controls
            EnableControls(true);
        }
    }
}