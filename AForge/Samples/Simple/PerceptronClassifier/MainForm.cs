using AForge.Controls;
using AForge.Core;
using AForge.Neuro.Activation_Functions;
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
    public class MainForm : Form
    {
        private GroupBox _groupBox1;
        private ListView _dataList;
        private Button _loadButton;
        private OpenFileDialog _openFileDialog;
        private Chart _chart;
        private GroupBox _groupBox2;
        private Label _label1;
        private TextBox _learningRateBox;
        private Button _startButton;
        private Label _noVisualizationLabel;
        private Label _label2;
        private Label _label3;
        private ListView _weightsList;
        private ColumnHeader _columnHeader1;
        private ColumnHeader _columnHeader2;
        private Label _label4;
        private TextBox _iterationsBox;
        private Button _stopButton;
        private Label _label5;
        private Chart _errorChart;
        private CheckBox _saveFilesCheck;

        private Container _components = null;

        private int _samples;
        private int _variables;
        private double[,] _data;
        private int[] _classes;

        private double _learningRate = 0.1;
        private bool _saveStatisticsToFiles;

        private Thread _workerThread;
        private bool _needToStop;

        // Constructor
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // initialize charts
            _chart.AddDataSeries("class1", Color.Red, Chart.SeriesType.Dots, 5);
            _chart.AddDataSeries("class2", Color.Blue, Chart.SeriesType.Dots, 5);
            _chart.AddDataSeries("classifier", Color.Gray, Chart.SeriesType.Line, 1, false);

            _errorChart.AddDataSeries("error", Color.Red, Chart.SeriesType.ConnectedDots, 3, false);

            // update some controls
            _saveFilesCheck.Checked = _saveStatisticsToFiles;
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
            _chart = new AForge.Controls.Chart();
            _loadButton = new System.Windows.Forms.Button();
            _dataList = new System.Windows.Forms.ListView();
            _noVisualizationLabel = new System.Windows.Forms.Label();
            _openFileDialog = new System.Windows.Forms.OpenFileDialog();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _errorChart = new AForge.Controls.Chart();
            _label5 = new System.Windows.Forms.Label();
            _stopButton = new System.Windows.Forms.Button();
            _iterationsBox = new System.Windows.Forms.TextBox();
            _label4 = new System.Windows.Forms.Label();
            _weightsList = new System.Windows.Forms.ListView();
            _columnHeader1 = new System.Windows.Forms.ColumnHeader();
            _columnHeader2 = new System.Windows.Forms.ColumnHeader();
            _label3 = new System.Windows.Forms.Label();
            _label2 = new System.Windows.Forms.Label();
            _startButton = new System.Windows.Forms.Button();
            _learningRateBox = new System.Windows.Forms.TextBox();
            _label1 = new System.Windows.Forms.Label();
            _saveFilesCheck = new System.Windows.Forms.CheckBox();
            _groupBox1.SuspendLayout();
            _groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            _groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _chart,
                                                                                    _loadButton,
                                                                                    _dataList,
                                                                                    _noVisualizationLabel});
            _groupBox1.Location = new System.Drawing.Point(10, 10);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(190, 420);
            _groupBox1.TabIndex = 0;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Data";
            // 
            // chart
            // 
            _chart.Location = new System.Drawing.Point(10, 215);
            _chart.Name = "_chart";
            _chart.Size = new System.Drawing.Size(170, 170);
            _chart.TabIndex = 2;
            _chart.Text = "chart1";
            // 
            // loadButton
            // 
            _loadButton.Location = new System.Drawing.Point(10, 390);
            _loadButton.Name = "_loadButton";
            _loadButton.TabIndex = 1;
            _loadButton.Text = "&Load";
            _loadButton.Click += new System.EventHandler(loadButton_Click);
            // 
            // dataList
            // 
            _dataList.FullRowSelect = true;
            _dataList.GridLines = true;
            _dataList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            _dataList.Location = new System.Drawing.Point(10, 20);
            _dataList.Name = "_dataList";
            _dataList.Size = new System.Drawing.Size(170, 190);
            _dataList.TabIndex = 0;
            _dataList.View = System.Windows.Forms.View.Details;
            // 
            // noVisualizationLabel
            // 
            _noVisualizationLabel.Location = new System.Drawing.Point(10, 215);
            _noVisualizationLabel.Name = "_noVisualizationLabel";
            _noVisualizationLabel.Size = new System.Drawing.Size(170, 170);
            _noVisualizationLabel.TabIndex = 2;
            _noVisualizationLabel.Text = "Visualization is not available.";
            _noVisualizationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            _noVisualizationLabel.Visible = false;
            // 
            // openFileDialog
            // 
            _openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            _openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            _groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _saveFilesCheck,
                                                                                    _errorChart,
                                                                                    _label5,
                                                                                    _stopButton,
                                                                                    _iterationsBox,
                                                                                    _label4,
                                                                                    _weightsList,
                                                                                    _label3,
                                                                                    _label2,
                                                                                    _startButton,
                                                                                    _learningRateBox,
                                                                                    _label1});
            _groupBox2.Location = new System.Drawing.Point(210, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(240, 420);
            _groupBox2.TabIndex = 1;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Training";
            // 
            // errorChart
            // 
            _errorChart.Location = new System.Drawing.Point(10, 270);
            _errorChart.Name = "_errorChart";
            _errorChart.Size = new System.Drawing.Size(220, 140);
            _errorChart.TabIndex = 10;
            // 
            // label5
            // 
            _label5.Location = new System.Drawing.Point(10, 250);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(101, 15);
            _label5.TabIndex = 9;
            _label5.Text = "Error\'s dynamics:";
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(155, 49);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 8;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // iterationsBox
            // 
            _iterationsBox.Location = new System.Drawing.Point(90, 50);
            _iterationsBox.Name = "_iterationsBox";
            _iterationsBox.ReadOnly = true;
            _iterationsBox.Size = new System.Drawing.Size(50, 20);
            _iterationsBox.TabIndex = 7;
            _iterationsBox.Text = "";
            // 
            // label4
            // 
            _label4.Location = new System.Drawing.Point(10, 52);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(65, 16);
            _label4.TabIndex = 6;
            _label4.Text = "Iterations:";
            // 
            // weightsList
            // 
            _weightsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                          _columnHeader1,
                                                                                          _columnHeader2});
            _weightsList.FullRowSelect = true;
            _weightsList.GridLines = true;
            _weightsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            _weightsList.Location = new System.Drawing.Point(10, 130);
            _weightsList.Name = "_weightsList";
            _weightsList.Size = new System.Drawing.Size(220, 110);
            _weightsList.TabIndex = 5;
            _weightsList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            _columnHeader1.Text = "Weight";
            _columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            _columnHeader2.Text = "Value";
            _columnHeader2.Width = 100;
            // 
            // label3
            // 
            _label3.Location = new System.Drawing.Point(10, 110);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(112, 16);
            _label3.TabIndex = 4;
            _label3.Text = "Perceptron weights:";
            // 
            // label2
            // 
            _label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label2.Location = new System.Drawing.Point(10, 100);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(220, 2);
            _label2.TabIndex = 3;
            // 
            // startButton
            // 
            _startButton.Enabled = false;
            _startButton.Location = new System.Drawing.Point(155, 19);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 2;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
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
            _label1.Size = new System.Drawing.Size(75, 16);
            _label1.TabIndex = 0;
            _label1.Text = "Learning rate:";
            // 
            // saveFilesCheck
            // 
            _saveFilesCheck.Location = new System.Drawing.Point(10, 80);
            _saveFilesCheck.Name = "_saveFilesCheck";
            _saveFilesCheck.Size = new System.Drawing.Size(182, 16);
            _saveFilesCheck.TabIndex = 11;
            _saveFilesCheck.Text = "Save weights and errors to files";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(459, 440);
            Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          _groupBox2,
                                                                          _groupBox1});
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Perceptron Classifier";
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

        // On "Load" button click - load data
        private void loadButton_Click(object sender, EventArgs e)
        {
            // data file format:
            // X1, X2, ... Xn, class (0|1)

            // show file selection dialog
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                // temp buffers (for 50 samples only)
                double[,] tempData = null;
                var tempClasses = new int[50];

                // min and max X values
                var minX = double.MaxValue;
                var maxX = double.MinValue;

                // samples count
                _samples = 0;

                try
                {
                    string str = null;

                    // open selected file
                    reader = File.OpenText(_openFileDialog.FileName);

                    // read the data
                    while ((_samples < 50) && ((str = reader.ReadLine()) != null))
                    {
                        // split the string
                        var strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');

                        // allocate data array
                        if (_samples == 0)
                        {
                            _variables = strs.Length - 1;
                            tempData = new double[50, _variables];
                        }

                        // parse data
                        for (var j = 0; j < _variables; j++)
                        {
                            tempData[_samples, j] = double.Parse(strs[j]);
                        }
                        tempClasses[_samples] = int.Parse(strs[_variables]);

                        // search for min value
                        if (tempData[_samples, 0] < minX)
                            minX = tempData[_samples, 0];
                        // search for max value
                        if (tempData[_samples, 0] > maxX)
                            maxX = tempData[_samples, 0];

                        _samples++;
                    }

                    // allocate and set data
                    _data = new double[_samples, _variables];
                    Array.Copy(tempData, 0, _data, 0, _samples * _variables);
                    _classes = new int[_samples];
                    Array.Copy(tempClasses, 0, _classes, 0, _samples);

                    // clear current result
                    ClearCurrentSolution();
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

                // show chart or not
                var showChart = (_variables == 2);

                if (showChart)
                {
                    _chart.RangeX = new DoubleRange(minX, maxX);
                    ShowTrainingData();
                }

                _chart.Visible = showChart;
                _noVisualizationLabel.Visible = !showChart;

                // enable start button
                _startButton.Enabled = true;
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            _learningRateBox.Text = _learningRate.ToString();
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all curent data and columns
            _dataList.Items.Clear();
            _dataList.Columns.Clear();

            // add columns
            for (int i = 0, n = _variables; i < n; i++)
            {
                _dataList.Columns.Add(string.Format("X{0}", i + 1),
                    50, HorizontalAlignment.Left);
            }
            _dataList.Columns.Add("Class", 50, HorizontalAlignment.Left);

            // add items
            for (var i = 0; i < _samples; i++)
            {
                _dataList.Items.Add(_data[i, 0].ToString());

                for (var j = 1; j < _variables; j++)
                {
                    _dataList.Items[i].SubItems.Add(_data[i, j].ToString());
                }
                _dataList.Items[i].SubItems.Add(_classes[i].ToString());
            }
        }

        // Show training data on chart
        private void ShowTrainingData()
        {
            var class1Size = 0;
            var class2Size = 0;

            // calculate number of samples in each class
            for (int i = 0, n = _samples; i < n; i++)
            {
                if (_classes[i] == 0)
                    class1Size++;
                else
                    class2Size++;
            }

            // allocate classes arrays
            var class1 = new double[class1Size, 2];
            var class2 = new double[class2Size, 2];

            // fill classes arrays
            for (int i = 0, c1 = 0, c2 = 0; i < _samples; i++)
            {
                if (_classes[i] == 0)
                {
                    // class 1
                    class1[c1, 0] = _data[i, 0];
                    class1[c1, 1] = _data[i, 1];
                    c1++;
                }
                else
                {
                    // class 2
                    class2[c2, 0] = _data[i, 0];
                    class2[c2, 1] = _data[i, 1];
                    c2++;
                }
            }

            // updata chart control
            _chart.UpdateDataSeries("class1", class1);
            _chart.UpdateDataSeries("class2", class2);
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

        // Clear current solution
        private void ClearCurrentSolution()
        {
            _chart.UpdateDataSeries("classifier", null);
            _errorChart.UpdateDataSeries("error", null);
            _weightsList.Items.Clear();
        }

        // On button "Start" - start learning procedure
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

        // On button "Stop" - stop learning procedure
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
                input[i] = new double[_variables];
                output[i] = new double[1];

                // copy input
                for (var j = 0; j < _variables; j++)
                    input[i][j] = _data[i, j];
                // copy output
                output[i][0] = _classes[i];
            }

            // create perceptron
            var network = new ActivationNetwork(new ThresholdFunction(), _variables, 1);
            var neuron = network[0][0];
            // create teacher
            var teacher = new PerceptronLearning(network);
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
                        for (var i = 0; i < _variables; i++)
                        {
                            weightsFile.Write(neuron[i] + ";");
                        }
                        weightsFile.WriteLine(neuron.Threshold);
                    }

                    // run epoch of learning procedure
                    var error = teacher.RunEpoch(input, output);
                    errorsList.Add(error);

                    // show current iteration
                    _iterationsBox.Text = iteration.ToString();

                    // save current error
                    if (errorsFile != null)
                    {
                        errorsFile.WriteLine(error);
                    }

                    // show classifier in the case of 2 dimensional data
                    if ((neuron.InputsCount == 2) && (neuron[1] != 0))
                    {
                        var k = -neuron[0] / neuron[1];
                        var b = -neuron.Threshold / neuron[1];

                        var classifier = new double[2, 2] {
                            { _chart.RangeX.Min, _chart.RangeX.Min * k + b },
                            { _chart.RangeX.Max, _chart.RangeX.Max * k + b }
                                                                };
                        // update chart
                        _chart.UpdateDataSeries("classifier", classifier);
                    }

                    // stop if no error
                    if (error == 0)
                        break;

                    iteration++;
                }

                // show perceptron's weights
                _weightsList.Items.Clear();
                for (var i = 0; i < _variables; i++)
                {
                    _weightsList.Items.Add(string.Format("Weight {0}", i + 1));
                    _weightsList.Items[i].SubItems.Add(neuron[i].ToString("F6"));
                }
                _weightsList.Items.Add("Threshold");
                _weightsList.Items[_variables].SubItems.Add(neuron.Threshold.ToString("F6"));

                // show error's dynamics
                var errors = new double[errorsList.Count, 2];

                for (int i = 0, n = errorsList.Count; i < n; i++)
                {
                    errors[i, 0] = i;
                    errors[i, 1] = (double)errorsList[i];
                }

                _errorChart.RangeX = new DoubleRange(0, errorsList.Count - 1);
                _errorChart.RangeY = new DoubleRange(0, _samples);
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
    }
}
