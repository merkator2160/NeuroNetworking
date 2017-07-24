// AForge Framework
// One-Layer Perceptron Classifier
//
// Copyright © Andrew Kirillov, 2006
// andrew.kirillov@gmail.com
//

using AForge.Controls;
using AForge.Core;
using AForge.Neuro.Activation_Functions;
using AForge.Neuro.Layers;
using AForge.Neuro.Learning;
using AForge.Neuro.Networks;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Classifier
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : Form
    {
        private GroupBox _groupBox1;
        private AForge.Controls.Chart _chart;
        private Button _loadButton;
        private OpenFileDialog _openFileDialog;
        private GroupBox _groupBox2;
        private Label _label1;
        private TextBox _learningRateBox;
        private Label _label2;
        private TextBox _iterationsBox;
        private Button _stopButton;
        private Button _startButton;
        private CheckBox _saveFilesCheck;
        private Label _label3;
        private Label _label4;
        private ListView _weightsList;
        private ColumnHeader _columnHeader1;
        private ColumnHeader _columnHeader2;
        private ColumnHeader _columnHeader3;
        private GroupBox _groupBox3;
        private AForge.Controls.Chart _errorChart;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container _components = null;

        private int _samples;
        private double[,] _data;
        private int[] _classes;
        private int _classesCount;
        private int[] _samplesPerClass;

        private double _learningRate = 0.1;
        private bool _saveStatisticsToFiles;

        private Thread _workerThread;
        private bool _needToStop;

        // color for data series
        private static Color[] _dataSereisColors = new Color[10] {
                                                                     Color.Red,     Color.Blue,
                                                                     Color.Green,   Color.DarkOrange,
                                                                     Color.Violet,  Color.Brown,
                                                                     Color.Black,   Color.Pink,
                                                                     Color.Olive,   Color.Navy };

        // Constructor
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // update some controls
            _saveFilesCheck.Checked = _saveStatisticsToFiles;
            UpdateSettings();

            // initialize charts
            _errorChart.AddDataSeries("error", Color.Red, Chart.SeriesType.ConnectedDots, 3);
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
            _loadButton = new System.Windows.Forms.Button();
            _chart = new AForge.Controls.Chart();
            _openFileDialog = new System.Windows.Forms.OpenFileDialog();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _weightsList = new System.Windows.Forms.ListView();
            _columnHeader1 = new System.Windows.Forms.ColumnHeader();
            _columnHeader2 = new System.Windows.Forms.ColumnHeader();
            _columnHeader3 = new System.Windows.Forms.ColumnHeader();
            _label4 = new System.Windows.Forms.Label();
            _label3 = new System.Windows.Forms.Label();
            _saveFilesCheck = new System.Windows.Forms.CheckBox();
            _stopButton = new System.Windows.Forms.Button();
            _startButton = new System.Windows.Forms.Button();
            _iterationsBox = new System.Windows.Forms.TextBox();
            _label2 = new System.Windows.Forms.Label();
            _learningRateBox = new System.Windows.Forms.TextBox();
            _label1 = new System.Windows.Forms.Label();
            _groupBox3 = new System.Windows.Forms.GroupBox();
            _errorChart = new AForge.Controls.Chart();
            _groupBox1.SuspendLayout();
            _groupBox2.SuspendLayout();
            _groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            _groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _loadButton,
                                                                                    _chart});
            _groupBox1.Location = new System.Drawing.Point(10, 10);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(220, 255);
            _groupBox1.TabIndex = 0;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Data";
            // 
            // loadButton
            // 
            _loadButton.Location = new System.Drawing.Point(10, 225);
            _loadButton.Name = "_loadButton";
            _loadButton.TabIndex = 1;
            _loadButton.Text = "&Load";
            _loadButton.Click += new System.EventHandler(loadButton_Click);
            // 
            // chart
            // 
            _chart.Location = new System.Drawing.Point(10, 20);
            _chart.Name = "_chart";
            _chart.Size = new System.Drawing.Size(200, 200);
            _chart.TabIndex = 0;
            // 
            // openFileDialog
            // 
            _openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            _openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            _groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _weightsList,
                                                                                    _label4,
                                                                                    _label3,
                                                                                    _saveFilesCheck,
                                                                                    _stopButton,
                                                                                    _startButton,
                                                                                    _iterationsBox,
                                                                                    _label2,
                                                                                    _learningRateBox,
                                                                                    _label1});
            _groupBox2.Location = new System.Drawing.Point(240, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(240, 410);
            _groupBox2.TabIndex = 1;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Training";
            // 
            // weightsList
            // 
            _weightsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                          _columnHeader1,
                                                                                          _columnHeader2,
                                                                                          _columnHeader3});
            _weightsList.FullRowSelect = true;
            _weightsList.GridLines = true;
            _weightsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            _weightsList.Location = new System.Drawing.Point(10, 130);
            _weightsList.Name = "_weightsList";
            _weightsList.Size = new System.Drawing.Size(220, 270);
            _weightsList.TabIndex = 14;
            _weightsList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            _columnHeader1.Text = "Neuron";
            // 
            // columnHeader2
            // 
            _columnHeader2.Text = "Weigh";
            // 
            // columnHeader3
            // 
            _columnHeader3.Text = "Value";
            _columnHeader3.Width = 65;
            // 
            // label4
            // 
            _label4.Location = new System.Drawing.Point(10, 110);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(55, 16);
            _label4.TabIndex = 13;
            _label4.Text = "Weights:";
            // 
            // label3
            // 
            _label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label3.Location = new System.Drawing.Point(10, 100);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(220, 2);
            _label3.TabIndex = 12;
            // 
            // saveFilesCheck
            // 
            _saveFilesCheck.Location = new System.Drawing.Point(10, 80);
            _saveFilesCheck.Name = "_saveFilesCheck";
            _saveFilesCheck.Size = new System.Drawing.Size(150, 16);
            _saveFilesCheck.TabIndex = 11;
            _saveFilesCheck.Text = "Save weights and errors to files";
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(155, 49);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 10;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // startButton
            // 
            _startButton.Enabled = false;
            _startButton.Location = new System.Drawing.Point(155, 19);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 9;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
            // 
            // iterationsBox
            // 
            _iterationsBox.Location = new System.Drawing.Point(90, 50);
            _iterationsBox.Name = "_iterationsBox";
            _iterationsBox.ReadOnly = true;
            _iterationsBox.Size = new System.Drawing.Size(50, 20);
            _iterationsBox.TabIndex = 3;
            _iterationsBox.Text = "";
            // 
            // label2
            // 
            _label2.Location = new System.Drawing.Point(10, 52);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(55, 13);
            _label2.TabIndex = 2;
            _label2.Text = "Iterations:";
            // 
            // learningRateBox
            // 
            _learningRateBox.Location = new System.Drawing.Point(90, 20);
            _learningRateBox.Name = "_learningRateBox";
            _learningRateBox.Size = new System.Drawing.Size(50, 20);
            _learningRateBox.TabIndex = 1;
            _learningRateBox.Text = "";
            // 
            // label1
            // 
            _label1.Location = new System.Drawing.Point(10, 22);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(80, 17);
            _label1.TabIndex = 0;
            _label1.Text = "Learning rate:";
            // 
            // groupBox3
            // 
            _groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _errorChart});
            _groupBox3.Location = new System.Drawing.Point(10, 270);
            _groupBox3.Name = "_groupBox3";
            _groupBox3.Size = new System.Drawing.Size(220, 150);
            _groupBox3.TabIndex = 2;
            _groupBox3.TabStop = false;
            _groupBox3.Text = "Error\'s dynamics";
            // 
            // errorChart
            // 
            _errorChart.Location = new System.Drawing.Point(10, 20);
            _errorChart.Name = "_errorChart";
            _errorChart.Size = new System.Drawing.Size(200, 120);
            _errorChart.TabIndex = 0;
            _errorChart.Text = "chart1";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(489, 430);
            Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          _groupBox3,
                                                                          _groupBox2,
                                                                          _groupBox1});
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "One-Layer Perceptron Classifier";
            Closing += new System.ComponentModel.CancelEventHandler(MainForm_Closing);
            Load += new System.EventHandler(MainForm_Load);
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

        // Load input data
        private void loadButton_Click(object sender, EventArgs e)
        {
            // data file format:
            // X1, X2, class

            // load maximum 10 classes !

            // show file selection dialog
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                // temp buffers (for 200 samples only)
                var tempData = new double[200, 2];
                var tempClasses = new int[200];

                // min and max X values
                var minX = double.MaxValue;
                var maxX = double.MinValue;

                // samples count
                _samples = 0;
                // classes count
                _classesCount = 0;
                _samplesPerClass = new int[10];

                try
                {
                    string str = null;

                    // open selected file
                    reader = File.OpenText(_openFileDialog.FileName);

                    // read the data
                    while ((_samples < 200) && ((str = reader.ReadLine()) != null))
                    {
                        // split the string
                        var strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');

                        // check tokens count
                        if (strs.Length != 3)
                            throw new ApplicationException("Invalid file format");

                        // parse tokens
                        tempData[_samples, 0] = double.Parse(strs[0]);
                        tempData[_samples, 1] = double.Parse(strs[1]);
                        tempClasses[_samples] = int.Parse(strs[2]);

                        // skip classes over 10, except only first 10 classes
                        if (tempClasses[_samples] >= 10)
                            continue;

                        // count the amount of different classes
                        if (tempClasses[_samples] >= _classesCount)
                            _classesCount = tempClasses[_samples] + 1;
                        // count samples per class
                        _samplesPerClass[tempClasses[_samples]]++;

                        // search for min value
                        if (tempData[_samples, 0] < minX)
                            minX = tempData[_samples, 0];
                        // search for max value
                        if (tempData[_samples, 0] > maxX)
                            maxX = tempData[_samples, 0];

                        _samples++;
                    }

                    // allocate and set data
                    _data = new double[_samples, 2];
                    Array.Copy(tempData, 0, _data, 0, _samples * 2);
                    _classes = new int[_samples];
                    Array.Copy(tempClasses, 0, _classes, 0, _samples);

                    // clear current result
                    _weightsList.Items.Clear();
                    _errorChart.UpdateDataSeries("error", null);
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

                // update chart
                _chart.RangeX = new DoubleRange(minX, maxX);
                ShowTrainingData();

                // enable start button
                _startButton.Enabled = true;
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            _learningRateBox.Text = _learningRate.ToString();
        }

        // Show training data on chart
        private void ShowTrainingData()
        {
            var dataSeries = new double[_classesCount][,];
            var indexes = new int[_classesCount];

            // allocate data arrays
            for (var i = 0; i < _classesCount; i++)
            {
                dataSeries[i] = new double[_samplesPerClass[i], 2];
            }

            // fill data arrays
            for (var i = 0; i < _samples; i++)
            {
                // get sample's class
                var dataClass = _classes[i];
                // copy data into appropriate array
                dataSeries[dataClass][indexes[dataClass], 0] = _data[i, 0];
                dataSeries[dataClass][indexes[dataClass], 1] = _data[i, 1];
                indexes[dataClass]++;
            }

            // remove all previous data series from chart control
            _chart.RemoveAllDataSeries();

            // add new data series
            for (var i = 0; i < _classesCount; i++)
            {
                var className = string.Format("class" + i);

                // add data series
                _chart.AddDataSeries(className, _dataSereisColors[i], Chart.SeriesType.Dots, 5);
                _chart.UpdateDataSeries(className, dataSeries[i]);
                // add classifier
                _chart.AddDataSeries(string.Format("classifier" + i), Color.Gray, Chart.SeriesType.Line, 1, false);
            }
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            _learningRateBox.Enabled = enable;
            _loadButton.Enabled = enable;
            _startButton.Enabled = enable;
            _saveFilesCheck.Enabled = enable;
            _stopButton.Enabled = !enable;
        }

        // On "Start" button click
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
            _saveStatisticsToFiles = _saveFilesCheck.Checked;

            // update settings controls
            UpdateSettings();

            // disable all settings controls
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
            // prepare learning data
            var input = new double[_samples][];
            var output = new double[_samples][];

            for (var i = 0; i < _samples; i++)
            {
                input[i] = new double[2];
                output[i] = new double[_classesCount];

                // set input
                input[i][0] = _data[i, 0];
                input[i][1] = _data[i, 1];
                // set output
                output[i][_classes[i]] = 1;
            }

            // create perceptron
            ActivationNetwork network = new ActivationNetwork(new ThresholdFunction(), 2, _classesCount);
            ActivationLayer layer = network[0];
            // create teacher
            PerceptronLearning teacher = new PerceptronLearning(network);
            // set learning rate
            teacher.LearningRate = _learningRate;

            // iterations
            var iteration = 1;

            // statistic files
            StreamWriter errorsFile = null;
            StreamWriter weightsFile = null;

            try
            {
                // check if we need to save statistics to files
                if (_saveStatisticsToFiles)
                {
                    // open files
                    errorsFile = File.CreateText("errors.csv");
                    weightsFile = File.CreateText("weights.csv");
                }

                // erros list
                var errorsList = new ArrayList();

                // loop
                while (!_needToStop)
                {
                    // save current weights
                    if (weightsFile != null)
                    {
                        for (var i = 0; i < _classesCount; i++)
                        {
                            weightsFile.Write("neuron" + i + ";");
                            weightsFile.Write(layer[i][0] + ";");
                            weightsFile.Write(layer[i][1] + ";");
                            weightsFile.WriteLine(layer[i].Threshold);
                        }
                    }

                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(input, output);
                    errorsList.Add(error);

                    // save current error
                    if (errorsFile != null)
                    {
                        errorsFile.WriteLine(error);
                    }

                    // show current iteration
                    _iterationsBox.Text = iteration.ToString();

                    // stop if no error
                    if (error == 0)
                        break;

                    // show classifiers
                    for (var j = 0; j < _classesCount; j++)
                    {
                        double k = -layer[j][0] / layer[j][1];
                        double b = -layer[j].Threshold / layer[j][1];

                        var classifier = new double[2, 2] {
                            { _chart.RangeX.Min, _chart.RangeX.Min * k + b },
                            { _chart.RangeX.Max, _chart.RangeX.Max * k + b }
                                                                };

                        // update chart
                        _chart.UpdateDataSeries(string.Format("classifier" + j), classifier);
                    }

                    iteration++;
                }

                // show perceptron's weights
                _weightsList.Items.Clear();
                for (var i = 0; i < _classesCount; i++)
                {
                    var neuronName = string.Format("Neuron {0}", i + 1);

                    // weight 0
                    var item = _weightsList.Items.Add(neuronName);
                    item.SubItems.Add("Weight 1");
                    item.SubItems.Add(layer[i][0].ToString("F6"));
                    // weight 1
                    item = _weightsList.Items.Add(neuronName);
                    item.SubItems.Add("Weight 2");
                    item.SubItems.Add(layer[i][1].ToString("F6"));
                    // threshold
                    item = _weightsList.Items.Add(neuronName);
                    item.SubItems.Add("Threshold");
                    item.SubItems.Add(layer[i].Threshold.ToString("F6"));
                }

                // show error's dynamics
                var errors = new double[errorsList.Count, 2];

                for (int i = 0, n = errorsList.Count; i < n; i++)
                {
                    errors[i, 0] = i;
                    errors[i, 1] = (double)errorsList[i];
                }

                _errorChart.RangeX = new DoubleRange(0, errorsList.Count - 1);
                _errorChart.UpdateDataSeries("error", errors);
            }
            catch (IOException)
            {
                MessageBox.Show("Failed writing file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // close files
                if (errorsFile != null)
                    errorsFile.Close();
                if (weightsFile != null)
                    weightsFile.Close();
            }

            // enable settings controls
            EnableControls(true);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
