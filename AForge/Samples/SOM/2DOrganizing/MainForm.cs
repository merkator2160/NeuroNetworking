// AForge Framework
// Kohonen SOM 2D Organizing
//
// Copyright © Andrew Kirillov, 2006
// andrew.kirillov@gmail.com
//

using AForge.Core;
using AForge.Neuro.Layers;
using AForge.Neuro.Learning;
using AForge.Neuro.Networks;
using AForge.Neuro.Neurons;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _2DOrganizing
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : Form
    {
        private GroupBox _groupBox1;
        private Button _generateButton;
        private Panel _pointsPanel;
        private GroupBox _groupBox2;
        private Panel _mapPanel;
        private CheckBox _showConnectionsCheck;
        private CheckBox _showInactiveCheck;
        private GroupBox _groupBox3;
        private Label _label1;
        private TextBox _sizeBox;
        private Label _label2;
        private Label _label3;
        private TextBox _radiusBox;
        private Label _label4;
        private TextBox _rateBox;
        private Label _label5;
        private TextBox _iterationsBox;
        private Label _label6;
        private Label _label7;
        private TextBox _currentIterationBox;
        private Label _label8;
        private Button _stopButton;
        private Button _startButton;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container _components = null;


        private const int _groupRadius = 20;
        private const int _pointsCount = 100;
        private int[,] _points = new int[_pointsCount, 2];    // x, y
        private double[][] _trainingSet = new double[_pointsCount][];
        private int[,,] _map;

        private int _networkSize = 15;
        private int _iterations = 500;
        private double _learningRate = 0.3;
        private int _learningRadius = 3;

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

            //
            GeneratePoints();
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
            _generateButton = new System.Windows.Forms.Button();
            _pointsPanel = new BufferedPanel();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _showInactiveCheck = new System.Windows.Forms.CheckBox();
            _showConnectionsCheck = new System.Windows.Forms.CheckBox();
            _mapPanel = new BufferedPanel();
            _groupBox3 = new System.Windows.Forms.GroupBox();
            _stopButton = new System.Windows.Forms.Button();
            _startButton = new System.Windows.Forms.Button();
            _currentIterationBox = new System.Windows.Forms.TextBox();
            _label8 = new System.Windows.Forms.Label();
            _label7 = new System.Windows.Forms.Label();
            _radiusBox = new System.Windows.Forms.TextBox();
            _label4 = new System.Windows.Forms.Label();
            _rateBox = new System.Windows.Forms.TextBox();
            _label5 = new System.Windows.Forms.Label();
            _iterationsBox = new System.Windows.Forms.TextBox();
            _label6 = new System.Windows.Forms.Label();
            _label3 = new System.Windows.Forms.Label();
            _label2 = new System.Windows.Forms.Label();
            _sizeBox = new System.Windows.Forms.TextBox();
            _label1 = new System.Windows.Forms.Label();
            _groupBox1.SuspendLayout();
            _groupBox2.SuspendLayout();
            _groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            _groupBox1.Controls.Add(_generateButton);
            _groupBox1.Controls.Add(_pointsPanel);
            _groupBox1.Location = new System.Drawing.Point(10, 10);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(220, 295);
            _groupBox1.TabIndex = 0;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Points";
            // 
            // generateButton
            // 
            _generateButton.Location = new System.Drawing.Point(10, 260);
            _generateButton.Name = "_generateButton";
            _generateButton.TabIndex = 1;
            _generateButton.Text = "&Generate";
            _generateButton.Click += new System.EventHandler(generateButton_Click);
            // 
            // pointsPanel
            // 
            _pointsPanel.BackColor = System.Drawing.Color.White;
            _pointsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _pointsPanel.Location = new System.Drawing.Point(10, 20);
            _pointsPanel.Name = "_pointsPanel";
            _pointsPanel.Size = new System.Drawing.Size(200, 200);
            _pointsPanel.TabIndex = 0;
            _pointsPanel.Paint += new System.Windows.Forms.PaintEventHandler(pointsPanel_Paint);
            // 
            // groupBox2
            // 
            _groupBox2.Controls.Add(_showInactiveCheck);
            _groupBox2.Controls.Add(_showConnectionsCheck);
            _groupBox2.Controls.Add(_mapPanel);
            _groupBox2.Location = new System.Drawing.Point(240, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(220, 295);
            _groupBox2.TabIndex = 1;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Map";
            // 
            // showInactiveCheck
            // 
            _showInactiveCheck.Checked = true;
            _showInactiveCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            _showInactiveCheck.Location = new System.Drawing.Point(10, 265);
            _showInactiveCheck.Name = "_showInactiveCheck";
            _showInactiveCheck.Size = new System.Drawing.Size(160, 16);
            _showInactiveCheck.TabIndex = 2;
            _showInactiveCheck.Text = "Show Inactive Neurons";
            _showInactiveCheck.CheckedChanged += new System.EventHandler(showInactiveCheck_CheckedChanged);
            // 
            // showConnectionsCheck
            // 
            _showConnectionsCheck.Checked = true;
            _showConnectionsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            _showConnectionsCheck.Location = new System.Drawing.Point(10, 240);
            _showConnectionsCheck.Name = "_showConnectionsCheck";
            _showConnectionsCheck.Size = new System.Drawing.Size(150, 16);
            _showConnectionsCheck.TabIndex = 1;
            _showConnectionsCheck.Text = "Show Connections";
            _showConnectionsCheck.CheckedChanged += new System.EventHandler(showConnectionsCheck_CheckedChanged);
            // 
            // mapPanel
            // 
            _mapPanel.BackColor = System.Drawing.Color.White;
            _mapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _mapPanel.Location = new System.Drawing.Point(10, 20);
            _mapPanel.Name = "_mapPanel";
            _mapPanel.Size = new System.Drawing.Size(200, 200);
            _mapPanel.TabIndex = 0;
            _mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(mapPanel_Paint);
            // 
            // groupBox3
            // 
            _groupBox3.Controls.Add(_stopButton);
            _groupBox3.Controls.Add(_startButton);
            _groupBox3.Controls.Add(_currentIterationBox);
            _groupBox3.Controls.Add(_label8);
            _groupBox3.Controls.Add(_label7);
            _groupBox3.Controls.Add(_radiusBox);
            _groupBox3.Controls.Add(_label4);
            _groupBox3.Controls.Add(_rateBox);
            _groupBox3.Controls.Add(_label5);
            _groupBox3.Controls.Add(_iterationsBox);
            _groupBox3.Controls.Add(_label6);
            _groupBox3.Controls.Add(_label3);
            _groupBox3.Controls.Add(_label2);
            _groupBox3.Controls.Add(_sizeBox);
            _groupBox3.Controls.Add(_label1);
            _groupBox3.Location = new System.Drawing.Point(470, 10);
            _groupBox3.Name = "_groupBox3";
            _groupBox3.Size = new System.Drawing.Size(180, 295);
            _groupBox3.TabIndex = 2;
            _groupBox3.TabStop = false;
            _groupBox3.Text = "Neural Network";
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(95, 260);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 16;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // startButton
            // 
            _startButton.Location = new System.Drawing.Point(10, 260);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 15;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
            // 
            // currentIterationBox
            // 
            _currentIterationBox.Location = new System.Drawing.Point(110, 160);
            _currentIterationBox.Name = "_currentIterationBox";
            _currentIterationBox.ReadOnly = true;
            _currentIterationBox.Size = new System.Drawing.Size(60, 20);
            _currentIterationBox.TabIndex = 14;
            _currentIterationBox.Text = "";
            // 
            // label8
            // 
            _label8.Location = new System.Drawing.Point(10, 162);
            _label8.Name = "_label8";
            _label8.Size = new System.Drawing.Size(100, 16);
            _label8.TabIndex = 13;
            _label8.Text = "Curren iteration:";
            // 
            // label7
            // 
            _label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label7.Location = new System.Drawing.Point(10, 148);
            _label7.Name = "_label7";
            _label7.Size = new System.Drawing.Size(160, 2);
            _label7.TabIndex = 12;
            // 
            // radiusBox
            // 
            _radiusBox.Location = new System.Drawing.Point(110, 120);
            _radiusBox.Name = "_radiusBox";
            _radiusBox.Size = new System.Drawing.Size(60, 20);
            _radiusBox.TabIndex = 11;
            _radiusBox.Text = "";
            // 
            // label4
            // 
            _label4.Location = new System.Drawing.Point(10, 122);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(100, 16);
            _label4.TabIndex = 10;
            _label4.Text = "Initial radius:";
            // 
            // rateBox
            // 
            _rateBox.Location = new System.Drawing.Point(110, 95);
            _rateBox.Name = "_rateBox";
            _rateBox.Size = new System.Drawing.Size(60, 20);
            _rateBox.TabIndex = 9;
            _rateBox.Text = "";
            // 
            // label5
            // 
            _label5.Location = new System.Drawing.Point(10, 97);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(100, 16);
            _label5.TabIndex = 8;
            _label5.Text = "Initial learning rate:";
            // 
            // iterationsBox
            // 
            _iterationsBox.Location = new System.Drawing.Point(110, 70);
            _iterationsBox.Name = "_iterationsBox";
            _iterationsBox.Size = new System.Drawing.Size(60, 20);
            _iterationsBox.TabIndex = 7;
            _iterationsBox.Text = "";
            // 
            // label6
            // 
            _label6.Location = new System.Drawing.Point(10, 72);
            _label6.Name = "_label6";
            _label6.Size = new System.Drawing.Size(60, 16);
            _label6.TabIndex = 6;
            _label6.Text = "Iteraions:";
            // 
            // label3
            // 
            _label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label3.Location = new System.Drawing.Point(10, 60);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(160, 2);
            _label3.TabIndex = 3;
            // 
            // label2
            // 
            _label2.Location = new System.Drawing.Point(10, 41);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(150, 15);
            _label2.TabIndex = 2;
            _label2.Text = "(neurons count = size * size)";
            // 
            // sizeBox
            // 
            _sizeBox.Location = new System.Drawing.Point(110, 20);
            _sizeBox.Name = "_sizeBox";
            _sizeBox.Size = new System.Drawing.Size(60, 20);
            _sizeBox.TabIndex = 1;
            _sizeBox.Text = "";
            // 
            // label1
            // 
            _label1.Location = new System.Drawing.Point(10, 22);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(54, 16);
            _label1.TabIndex = 0;
            _label1.Text = "Size:";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(659, 315);
            Controls.Add(_groupBox3);
            Controls.Add(_groupBox2);
            Controls.Add(_groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Kohonen SOM 2D Organizing";
            Closing += new System.ComponentModel.CancelEventHandler(MainForm_Closing);
            _groupBox1.ResumeLayout(false);
            _groupBox2.ResumeLayout(false);
            _groupBox3.ResumeLayout(false);
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
            _sizeBox.Text = _networkSize.ToString();
            _iterationsBox.Text = _iterations.ToString();
            _rateBox.Text = _learningRate.ToString();
            _radiusBox.Text = _learningRadius.ToString();
        }

        // On "Generate" button click
        private void generateButton_Click(object sender, EventArgs e)
        {
            GeneratePoints();
        }

        // Generate point
        private void GeneratePoints()
        {
            var width = _pointsPanel.ClientRectangle.Width;
            var height = _pointsPanel.ClientRectangle.Height;
            var diameter = _groupRadius * 2;

            // generate groups of ten points
            for (var i = 0; i < _pointsCount;)
            {
                var cx = _rand.Next(width);
                var cy = _rand.Next(height);

                // generate group
                for (var j = 0; (i < _pointsCount) && (j < 10);)
                {
                    var x = cx + _rand.Next(diameter) - _groupRadius;
                    var y = cy + _rand.Next(diameter) - _groupRadius;

                    // check if wee are not out
                    if ((x < 0) || (y < 0) || (x >= width) || (y >= height))
                    {
                        continue;
                    }

                    // add point
                    _points[i, 0] = x;
                    _points[i, 1] = y;

                    j++;
                    i++;
                }
            }

            _map = null;
            _pointsPanel.Invalidate();
            _mapPanel.Invalidate();
        }

        // Paint points
        private void pointsPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            using (Brush brush = new SolidBrush(Color.Blue))
            {
                // draw all points
                for (int i = 0, n = _points.GetLength(0); i < n; i++)
                {
                    g.FillEllipse(brush, _points[i, 0] - 2, _points[i, 1] - 2, 5, 5);
                }
            }
        }

        // Paint map
        private void mapPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (_map != null)
            {
                //
                var showConnections = _showConnectionsCheck.Checked;
                var showInactive = _showInactiveCheck.Checked;

                // pens and brushes
                Brush brush = new SolidBrush(Color.Blue);
                Brush brushGray = new SolidBrush(Color.FromArgb(192, 192, 192));
                var pen = new Pen(Color.Blue, 1);
                var penGray = new Pen(Color.FromArgb(192, 192, 192), 1);

                // lock
                Monitor.Enter(this);

                if (showConnections)
                {
                    // draw connections
                    for (int i = 0, n = _map.GetLength(0); i < n; i++)
                    {
                        for (int j = 0, k = _map.GetLength(1); j < k; j++)
                        {
                            if ((!showInactive) && (_map[i, j, 2] == 0))
                                continue;

                            // left
                            if ((i > 0) && ((showInactive) || (_map[i - 1, j, 2] == 1)))
                            {
                                g.DrawLine(((_map[i, j, 2] == 0) || (_map[i - 1, j, 2] == 0)) ? penGray : pen, _map[i, j, 0], _map[i, j, 1], _map[i - 1, j, 0], _map[i - 1, j, 1]);
                            }

                            // right
                            if ((i < n - 1) && ((showInactive) || (_map[i + 1, j, 2] == 1)))
                            {
                                g.DrawLine(((_map[i, j, 2] == 0) || (_map[i + 1, j, 2] == 0)) ? penGray : pen, _map[i, j, 0], _map[i, j, 1], _map[i + 1, j, 0], _map[i + 1, j, 1]);
                            }

                            // top
                            if ((j > 0) && ((showInactive) || (_map[i, j - 1, 2] == 1)))
                            {
                                g.DrawLine(((_map[i, j, 2] == 0) || (_map[i, j - 1, 2] == 0)) ? penGray : pen, _map[i, j, 0], _map[i, j, 1], _map[i, j - 1, 0], _map[i, j - 1, 1]);
                            }

                            // bottom
                            if ((j < k - 1) && ((showInactive) || (_map[i, j + 1, 2] == 1)))
                            {
                                g.DrawLine(((_map[i, j, 2] == 0) || (_map[i, j + 1, 2] == 0)) ? penGray : pen, _map[i, j, 0], _map[i, j, 1], _map[i, j + 1, 0], _map[i, j + 1, 1]);
                            }
                        }
                    }
                }

                // draw the map
                for (int i = 0, n = _map.GetLength(0); i < n; i++)
                {
                    for (int j = 0, k = _map.GetLength(1); j < k; j++)
                    {
                        if ((!showInactive) && (_map[i, j, 2] == 0))
                            continue;

                        // draw the point
                        g.FillEllipse((_map[i, j, 2] == 0) ? brushGray : brush, _map[i, j, 0] - 2, _map[i, j, 1] - 2, 5, 5);
                    }
                }

                // unlock
                Monitor.Exit(this);

                brush.Dispose();
                brushGray.Dispose();
                pen.Dispose();
                penGray.Dispose();
            }
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            _sizeBox.Enabled = enable;
            _iterationsBox.Enabled = enable;
            _rateBox.Enabled = enable;
            _radiusBox.Enabled = enable;

            _startButton.Enabled = enable;
            _generateButton.Enabled = enable;
            _stopButton.Enabled = !enable;
        }

        // Show/hide connections on map
        private void showConnectionsCheck_CheckedChanged(object sender, EventArgs e)
        {
            _mapPanel.Invalidate();
        }

        // Show/hide inactive neurons on map
        private void showInactiveCheck_CheckedChanged(object sender, EventArgs e)
        {
            _mapPanel.Invalidate();
        }

        // On "Start" button click
        private void startButton_Click(object sender, EventArgs e)
        {
            // get network size
            try
            {
                _networkSize = Math.Max(5, Math.Min(50, int.Parse(_sizeBox.Text)));
            }
            catch
            {
                _networkSize = 15;
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
                _learningRate = 0.3;
            }
            // get radius
            try
            {
                _learningRadius = Math.Max(1, Math.Min(30, int.Parse(_radiusBox.Text)));
            }
            catch
            {
                _learningRadius = 3;
            }
            // update settings controls
            UpdateSettings();

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // generate training set
            for (var i = 0; i < _pointsCount; i++)
            {
                // create new training sample
                _trainingSet[i] = new double[2] { _points[i, 0], _points[i, 1] };
            }

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
            NeuronBase.RandRange = new DoubleRange(0, Math.Max(_pointsPanel.ClientRectangle.Width, _pointsPanel.ClientRectangle.Height));

            // create network
            var network = new DistanceNetwork(2, _networkSize * _networkSize);

            // create learning algorithm
            var trainer = new SomLearning(network, _networkSize, _networkSize);

            // create map
            _map = new int[_networkSize, _networkSize, 3];

            var fixedLearningRate = _learningRate / 10;
            var driftingLearningRate = fixedLearningRate * 9;

            // iterations
            var i = 0;

            // loop
            while (!_needToStop)
            {
                trainer.LearningRate = driftingLearningRate * (_iterations - i) / _iterations + fixedLearningRate;
                trainer.LearningRadius = (double)_learningRadius * (_iterations - i) / _iterations;

                // run training epoch
                trainer.RunEpoch(_trainingSet);

                // update map
                UpdateMap(network);

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

        // Update map
        private void UpdateMap(DistanceNetwork network)
        {
            // get first layer
            Layer layer = network[0];

            // lock
            Monitor.Enter(this);

            // run through all neurons
            for (int i = 0, n = layer.NeuronsCount; i < n; i++)
            {
                var neuron = layer[i];

                var x = i % _networkSize;
                var y = i / _networkSize;

                _map[y, x, 0] = (int)neuron[0];
                _map[y, x, 1] = (int)neuron[1];
                _map[y, x, 2] = 0;
            }

            // collect active neurons
            for (var i = 0; i < _pointsCount; i++)
            {
                network.Compute(_trainingSet[i]);
                var w = network.GetWinner();

                _map[w / _networkSize, w % _networkSize, 2] = 1;
            }

            // unlock
            Monitor.Exit(this);

            //
            _mapPanel.Invalidate();
        }
    }
}
