using AForge.Core;
using AForge.Neuro.Layers;
using AForge.Neuro.Learning;
using AForge.Neuro.Networks;
using AForge.Neuro.Neurons;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace Color
{
    public class MainForm : Form
    {
        private GroupBox _groupBox1;
        private Panel _mapPanel;
        private GroupBox _groupBox2;
        private Label _label1;
        private TextBox _iterationsBox;
        private Label _label2;
        private TextBox _rateBox;
        private Label _label3;
        private TextBox _radiusBox;
        private Label _label4;
        private Button _startButton;
        private Button _stopButton;
        private Button _randomizeButton;
        private Label _label5;
        private TextBox _currentIterationBox;

        private Container _components = null;

        private DistanceNetwork _network;
        private Bitmap _mapBitmap;
        private Random _rand = new Random();

        private int _iterations = 5000;
        private double _learningRate = 0.1;
        private double _radius = 15;

        private Thread _workerThread;
        private bool _needToStop;

        // Constructor
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Create network
            _network = new DistanceNetwork(3, 100 * 100);

            // Create map bitmap
            _mapBitmap = new Bitmap(200, 200, PixelFormat.Format24bppRgb);

            //
            RandomizeNetwork();
            UpdateSettings();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _components?.Dispose();
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
            _randomizeButton = new System.Windows.Forms.Button();
            _mapPanel = new BufferedPanel();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _currentIterationBox = new System.Windows.Forms.TextBox();
            _label5 = new System.Windows.Forms.Label();
            _stopButton = new System.Windows.Forms.Button();
            _startButton = new System.Windows.Forms.Button();
            _label4 = new System.Windows.Forms.Label();
            _radiusBox = new System.Windows.Forms.TextBox();
            _label3 = new System.Windows.Forms.Label();
            _rateBox = new System.Windows.Forms.TextBox();
            _label2 = new System.Windows.Forms.Label();
            _iterationsBox = new System.Windows.Forms.TextBox();
            _label1 = new System.Windows.Forms.Label();
            _groupBox1.SuspendLayout();
            _groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            _groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _randomizeButton,
                                                                                    _mapPanel});
            _groupBox1.Location = new System.Drawing.Point(10, 10);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(222, 265);
            _groupBox1.TabIndex = 0;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Map";
            // 
            // randomizeButton
            // 
            _randomizeButton.Location = new System.Drawing.Point(10, 230);
            _randomizeButton.Name = "_randomizeButton";
            _randomizeButton.TabIndex = 1;
            _randomizeButton.Text = "&RandomizeCurrentNeuron";
            _randomizeButton.Click += new System.EventHandler(randomizeButton_Click);
            // 
            // mapPanel
            // 
            _mapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _mapPanel.Location = new System.Drawing.Point(10, 20);
            _mapPanel.Name = "_mapPanel";
            _mapPanel.Size = new System.Drawing.Size(202, 202);
            _mapPanel.TabIndex = 0;
            _mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(mapPanel_Paint);
            // 
            // groupBox2
            // 
            _groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _currentIterationBox,
                                                                                    _label5,
                                                                                    _stopButton,
                                                                                    _startButton,
                                                                                    _label4,
                                                                                    _radiusBox,
                                                                                    _label3,
                                                                                    _rateBox,
                                                                                    _label2,
                                                                                    _iterationsBox,
                                                                                    _label1});
            _groupBox2.Location = new System.Drawing.Point(240, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(190, 265);
            _groupBox2.TabIndex = 1;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Neural Network";
            // 
            // currentIterationBox
            // 
            _currentIterationBox.Location = new System.Drawing.Point(110, 120);
            _currentIterationBox.Name = "_currentIterationBox";
            _currentIterationBox.ReadOnly = true;
            _currentIterationBox.Size = new System.Drawing.Size(70, 20);
            _currentIterationBox.TabIndex = 10;
            _currentIterationBox.Text = "";
            // 
            // label5
            // 
            _label5.Location = new System.Drawing.Point(10, 122);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(100, 16);
            _label5.TabIndex = 9;
            _label5.Text = "Curren iteration:";
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(105, 230);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 8;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // startButton
            // 
            _startButton.Location = new System.Drawing.Point(20, 230);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 7;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
            // 
            // label4
            // 
            _label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label4.Location = new System.Drawing.Point(10, 100);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(170, 2);
            _label4.TabIndex = 6;
            // 
            // radiusBox
            // 
            _radiusBox.Location = new System.Drawing.Point(110, 70);
            _radiusBox.Name = "_radiusBox";
            _radiusBox.Size = new System.Drawing.Size(70, 20);
            _radiusBox.TabIndex = 5;
            _radiusBox.Text = "";
            // 
            // label3
            // 
            _label3.Location = new System.Drawing.Point(10, 72);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(100, 16);
            _label3.TabIndex = 4;
            _label3.Text = "Initial radius:";
            // 
            // rateBox
            // 
            _rateBox.Location = new System.Drawing.Point(110, 45);
            _rateBox.Name = "_rateBox";
            _rateBox.Size = new System.Drawing.Size(70, 20);
            _rateBox.TabIndex = 3;
            _rateBox.Text = "";
            // 
            // label2
            // 
            _label2.Location = new System.Drawing.Point(10, 47);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(100, 16);
            _label2.TabIndex = 2;
            _label2.Text = "Initial learning rate:";
            // 
            // iterationsBox
            // 
            _iterationsBox.Location = new System.Drawing.Point(110, 20);
            _iterationsBox.Name = "_iterationsBox";
            _iterationsBox.Size = new System.Drawing.Size(70, 20);
            _iterationsBox.TabIndex = 1;
            _iterationsBox.Text = "";
            // 
            // label1
            // 
            _label1.Location = new System.Drawing.Point(10, 22);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(60, 16);
            _label1.TabIndex = 0;
            _label1.Text = "Iteraions:";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(439, 285);
            Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          _groupBox2,
                                                                          _groupBox1});
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Color Clustering using Kohonen SOM";
            Closing += new System.ComponentModel.CancelEventHandler(MainForm_Closing);
            _groupBox1.ResumeLayout(false);
            _groupBox2.ResumeLayout(false);
            ResumeLayout(false);

        }
        #endregion


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
            _iterationsBox.Text = _iterations.ToString();
            _rateBox.Text = _learningRate.ToString();
            _radiusBox.Text = _radius.ToString();
        }

        // On "Rundomize" button clicked
        private void randomizeButton_Click(object sender, EventArgs e)
        {
            RandomizeNetwork();
        }

        // Radnomize weights of network
        private void RandomizeNetwork()
        {
            NeuronBase.RandRange = new DoubleRange(0, 255);

            // randomize net
            _network.Randomize();

            // update map
            UpdateMap();
        }

        // Update map from network weights
        private void UpdateMap()
        {
            // lock
            Monitor.Enter(this);

            // lock bitmap
            var mapData = _mapBitmap.LockBits(new Rectangle(0, 0, 200, 200),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            var stride = mapData.Stride;
            var offset = stride - 200 * 3;
            Layer layer = _network[0];

            unsafe
            {
                var ptr = (byte*)mapData.Scan0;

                // for all rows
                for (int y = 0, i = 0; y < 100; y++)
                {
                    // for all pixels
                    for (var x = 0; x < 100; x++, i++, ptr += 6)
                    {
                        var neuronBase = layer[i];

                        // red
                        ptr[2] = ptr[2 + 3] = ptr[2 + stride] = ptr[2 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuronBase[0]));
                        // green
                        ptr[1] = ptr[1 + 3] = ptr[1 + stride] = ptr[1 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuronBase[1]));
                        // blue
                        ptr[0] = ptr[0 + 3] = ptr[0 + stride] = ptr[0 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuronBase[2]));
                    }

                    ptr += offset;
                    ptr += stride;
                }
            }

            // unlock image
            _mapBitmap.UnlockBits(mapData);

            // unlock
            Monitor.Exit(this);

            // invalidate maps panel
            _mapPanel.Invalidate();
        }

        // Paint map
        private void mapPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            // lock
            Monitor.Enter(this);

            // drat image
            g.DrawImage(_mapBitmap, 0, 0, 200, 200);

            // unlock
            Monitor.Exit(this);
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            _iterationsBox.Enabled = enable;
            _rateBox.Enabled = enable;
            _radiusBox.Enabled = enable;

            _startButton.Enabled = enable;
            _randomizeButton.Enabled = enable;
            _stopButton.Enabled = !enable;
        }

        // On "Start" button click
        private void startButton_Click(object sender, EventArgs e)
        {
            // get iterations count
            try
            {
                _iterations = Math.Max(10, Math.Min(1000000, int.Parse(_iterationsBox.Text)));
            }
            catch
            {
                _iterations = 5000;
            }
            // get learning rate
            try
            {
                _learningRate = Math.Max(0.00001, Math.Min(1.0, double.Parse(_rateBox.Text)));
            }
            catch
            {
                _learningRate = 0.1;
            }
            // get radius
            try
            {
                _radius = Math.Max(5, Math.Min(75, int.Parse(_radiusBox.Text)));
            }
            catch
            {
                _radius = 15;
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
            // create learning algorithm
            var trainer = new SomLearning(_network);

            // input
            var input = new double[3];

            var fixedLearningRate = _learningRate / 10;
            var driftingLearningRate = fixedLearningRate * 9;

            // iterations
            var i = 0;

            // loop
            while (!_needToStop)
            {
                trainer.LearningRate = driftingLearningRate * (_iterations - i) / _iterations + fixedLearningRate;
                trainer.LearningRadius = _radius * (_iterations - i) / _iterations;

                input[0] = _rand.Next(256);
                input[1] = _rand.Next(256);
                input[2] = _rand.Next(256);

                trainer.Run(input);

                // update map once per 50 iterations
                if ((i % 10) == 9)
                {
                    UpdateMap();
                }

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
