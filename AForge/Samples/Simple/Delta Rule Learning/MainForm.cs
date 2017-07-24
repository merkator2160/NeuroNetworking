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
        private Button _loadButton;
        private OpenFileDialog _openFileDialog;
        private ListView _dataList;
        private GroupBox _groupBox2;
        private TextBox _learningRateBox;
        private Label _label1;
        private Label _label2;
        private TextBox _alphaBox;
        private Label _label3;
        private TextBox _errorLimitBox;
        private Label _label4;
        private TextBox _iterationsBox;
        private Label _label5;
        private Label _label6;
        private TextBox _neuronsBox;
        private CheckBox _oneNeuronForTwoCheck;
        private Label _label7;
        private Label _label8;
        private TextBox _currentIterationBox;
        private Button _stopButton;
        private Button _startButton;
        private Label _label9;
        private Label _label10;
        private TextBox _classesBox;
        private CheckBox _errorLimitCheck;
        private Label _label11;
        private TextBox _currentErrorBox;
        private GroupBox _groupBox3;
        private Label _label12;
        private ListView _weightsList;
        private Label _label13;
        private ColumnHeader _columnHeader1;
        private ColumnHeader _columnHeader2;
        private ColumnHeader _columnHeader3;
        private CheckBox _saveFilesCheck;
        private Chart _errorChart;

        private Container _components = null;

        private int _samples;
        private int _variables;
        private double[,] _data;
        private int[] _classes;
        private int _classesCount;
        private int[] _samplesPerClass;
        private int _neuronsCount;

        private double _learningRate = 0.1;
        private double _sigmoidAlphaValue = 2.0;
        private double _learningErrorLimit = 0.1;
        private double _iterationLimit = 1000;
        private bool _useOneNeuronForTwoClasses;
        private bool _useErrorLimit = true;
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

            // update settings controls
            UpdateSettings();

            // initialize charts
            _errorChart.AddDataSeries("error", Color.Red, Chart.SeriesType.Line, 1);
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
            _classesBox = new System.Windows.Forms.TextBox();
            _label10 = new System.Windows.Forms.Label();
            _dataList = new System.Windows.Forms.ListView();
            _loadButton = new System.Windows.Forms.Button();
            _openFileDialog = new System.Windows.Forms.OpenFileDialog();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _currentErrorBox = new System.Windows.Forms.TextBox();
            _label11 = new System.Windows.Forms.Label();
            _label9 = new System.Windows.Forms.Label();
            _currentIterationBox = new System.Windows.Forms.TextBox();
            _label8 = new System.Windows.Forms.Label();
            _label7 = new System.Windows.Forms.Label();
            _errorLimitCheck = new System.Windows.Forms.CheckBox();
            _oneNeuronForTwoCheck = new System.Windows.Forms.CheckBox();
            _neuronsBox = new System.Windows.Forms.TextBox();
            _label6 = new System.Windows.Forms.Label();
            _label5 = new System.Windows.Forms.Label();
            _iterationsBox = new System.Windows.Forms.TextBox();
            _label4 = new System.Windows.Forms.Label();
            _errorLimitBox = new System.Windows.Forms.TextBox();
            _label3 = new System.Windows.Forms.Label();
            _alphaBox = new System.Windows.Forms.TextBox();
            _label2 = new System.Windows.Forms.Label();
            _label1 = new System.Windows.Forms.Label();
            _learningRateBox = new System.Windows.Forms.TextBox();
            _stopButton = new System.Windows.Forms.Button();
            _startButton = new System.Windows.Forms.Button();
            _groupBox3 = new System.Windows.Forms.GroupBox();
            _saveFilesCheck = new System.Windows.Forms.CheckBox();
            _label13 = new System.Windows.Forms.Label();
            _weightsList = new System.Windows.Forms.ListView();
            _errorChart = new Chart();
            _label12 = new System.Windows.Forms.Label();
            _columnHeader1 = new System.Windows.Forms.ColumnHeader();
            _columnHeader2 = new System.Windows.Forms.ColumnHeader();
            _columnHeader3 = new System.Windows.Forms.ColumnHeader();
            _groupBox1.SuspendLayout();
            _groupBox2.SuspendLayout();
            _groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            _groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _classesBox,
                                                                                    _label10,
                                                                                    _dataList,
                                                                                    _loadButton});
            _groupBox1.Location = new System.Drawing.Point(10, 10);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(230, 330);
            _groupBox1.TabIndex = 0;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Data";
            // 
            // classesBox
            // 
            _classesBox.Location = new System.Drawing.Point(190, 297);
            _classesBox.Name = "_classesBox";
            _classesBox.ReadOnly = true;
            _classesBox.Size = new System.Drawing.Size(30, 20);
            _classesBox.TabIndex = 3;
            _classesBox.Text = "";
            // 
            // label10
            // 
            _label10.Location = new System.Drawing.Point(140, 299);
            _label10.Name = "_label10";
            _label10.Size = new System.Drawing.Size(50, 12);
            _label10.TabIndex = 2;
            _label10.Text = "Classes:";
            // 
            // dataList
            // 
            _dataList.FullRowSelect = true;
            _dataList.GridLines = true;
            _dataList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            _dataList.Location = new System.Drawing.Point(10, 20);
            _dataList.Name = "_dataList";
            _dataList.Size = new System.Drawing.Size(210, 270);
            _dataList.TabIndex = 0;
            _dataList.View = System.Windows.Forms.View.Details;
            // 
            // loadButton
            // 
            _loadButton.Location = new System.Drawing.Point(10, 297);
            _loadButton.Name = "_loadButton";
            _loadButton.TabIndex = 1;
            _loadButton.Text = "&Load";
            _loadButton.Click += new System.EventHandler(loadButton_Click);
            // 
            // openFileDialog
            // 
            _openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            _openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            _groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _currentErrorBox,
                                                                                    _label11,
                                                                                    _label9,
                                                                                    _currentIterationBox,
                                                                                    _label8,
                                                                                    _label7,
                                                                                    _errorLimitCheck,
                                                                                    _oneNeuronForTwoCheck,
                                                                                    _neuronsBox,
                                                                                    _label6,
                                                                                    _label5,
                                                                                    _iterationsBox,
                                                                                    _label4,
                                                                                    _errorLimitBox,
                                                                                    _label3,
                                                                                    _alphaBox,
                                                                                    _label2,
                                                                                    _label1,
                                                                                    _learningRateBox,
                                                                                    _stopButton,
                                                                                    _startButton});
            _groupBox2.Location = new System.Drawing.Point(250, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(185, 330);
            _groupBox2.TabIndex = 1;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Training";
            // 
            // currentErrorBox
            // 
            _currentErrorBox.Location = new System.Drawing.Point(125, 255);
            _currentErrorBox.Name = "_currentErrorBox";
            _currentErrorBox.ReadOnly = true;
            _currentErrorBox.Size = new System.Drawing.Size(50, 20);
            _currentErrorBox.TabIndex = 20;
            _currentErrorBox.Text = "";
            // 
            // label11
            // 
            _label11.Location = new System.Drawing.Point(10, 257);
            _label11.Name = "_label11";
            _label11.Size = new System.Drawing.Size(121, 14);
            _label11.TabIndex = 19;
            _label11.Text = "Current average error:";
            // 
            // label9
            // 
            _label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label9.Location = new System.Drawing.Point(10, 283);
            _label9.Name = "_label9";
            _label9.Size = new System.Drawing.Size(165, 2);
            _label9.TabIndex = 18;
            // 
            // currentIterationBox
            // 
            _currentIterationBox.Location = new System.Drawing.Point(125, 230);
            _currentIterationBox.Name = "_currentIterationBox";
            _currentIterationBox.ReadOnly = true;
            _currentIterationBox.Size = new System.Drawing.Size(50, 20);
            _currentIterationBox.TabIndex = 17;
            _currentIterationBox.Text = "";
            // 
            // label8
            // 
            _label8.Location = new System.Drawing.Point(10, 232);
            _label8.Name = "_label8";
            _label8.Size = new System.Drawing.Size(98, 16);
            _label8.TabIndex = 16;
            _label8.Text = "Current iteration:";
            // 
            // label7
            // 
            _label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label7.Location = new System.Drawing.Point(10, 220);
            _label7.Name = "_label7";
            _label7.Size = new System.Drawing.Size(165, 2);
            _label7.TabIndex = 15;
            // 
            // errorLimitCheck
            // 
            _errorLimitCheck.Location = new System.Drawing.Point(10, 190);
            _errorLimitCheck.Name = "_errorLimitCheck";
            _errorLimitCheck.Size = new System.Drawing.Size(157, 25);
            _errorLimitCheck.TabIndex = 14;
            _errorLimitCheck.Text = "Use error limit (checked) or iterations limit";
            // 
            // oneNeuronForTwoCheck
            // 
            _oneNeuronForTwoCheck.Enabled = false;
            _oneNeuronForTwoCheck.Location = new System.Drawing.Point(10, 165);
            _oneNeuronForTwoCheck.Name = "_oneNeuronForTwoCheck";
            _oneNeuronForTwoCheck.Size = new System.Drawing.Size(168, 15);
            _oneNeuronForTwoCheck.TabIndex = 13;
            _oneNeuronForTwoCheck.Text = "Use 1 neuron for 2 classes";
            _oneNeuronForTwoCheck.CheckedChanged += new System.EventHandler(oneNeuronForTwoCheck_CheckedChanged);
            // 
            // neuronsBox
            // 
            _neuronsBox.Location = new System.Drawing.Point(125, 135);
            _neuronsBox.Name = "_neuronsBox";
            _neuronsBox.ReadOnly = true;
            _neuronsBox.Size = new System.Drawing.Size(50, 20);
            _neuronsBox.TabIndex = 12;
            _neuronsBox.Text = "";
            // 
            // label6
            // 
            _label6.Location = new System.Drawing.Point(10, 137);
            _label6.Name = "_label6";
            _label6.Size = new System.Drawing.Size(59, 12);
            _label6.TabIndex = 11;
            _label6.Text = "Neurons:";
            // 
            // label5
            // 
            _label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            _label5.Location = new System.Drawing.Point(125, 115);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(58, 17);
            _label5.TabIndex = 10;
            _label5.Text = "( 0 - inifinity )";
            // 
            // iterationsBox
            // 
            _iterationsBox.Location = new System.Drawing.Point(125, 95);
            _iterationsBox.Name = "_iterationsBox";
            _iterationsBox.Size = new System.Drawing.Size(50, 20);
            _iterationsBox.TabIndex = 9;
            _iterationsBox.Text = "";
            // 
            // label4
            // 
            _label4.Location = new System.Drawing.Point(10, 97);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(90, 13);
            _label4.TabIndex = 8;
            _label4.Text = "Iterations limit:";
            // 
            // errorLimitBox
            // 
            _errorLimitBox.Location = new System.Drawing.Point(125, 70);
            _errorLimitBox.Name = "_errorLimitBox";
            _errorLimitBox.Size = new System.Drawing.Size(50, 20);
            _errorLimitBox.TabIndex = 7;
            _errorLimitBox.Text = "";
            // 
            // label3
            // 
            _label3.Location = new System.Drawing.Point(10, 72);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(110, 15);
            _label3.TabIndex = 6;
            _label3.Text = "Learning error limit:";
            // 
            // alphaBox
            // 
            _alphaBox.Location = new System.Drawing.Point(125, 45);
            _alphaBox.Name = "_alphaBox";
            _alphaBox.Size = new System.Drawing.Size(50, 20);
            _alphaBox.TabIndex = 5;
            _alphaBox.Text = "";
            // 
            // label2
            // 
            _label2.Location = new System.Drawing.Point(10, 47);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(120, 15);
            _label2.TabIndex = 4;
            _label2.Text = "Sigmoid\'s alpha value:";
            // 
            // label1
            // 
            _label1.Location = new System.Drawing.Point(10, 22);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(75, 16);
            _label1.TabIndex = 2;
            _label1.Text = "Learning rate:";
            // 
            // learningRateBox
            // 
            _learningRateBox.Location = new System.Drawing.Point(125, 20);
            _learningRateBox.Name = "_learningRateBox";
            _learningRateBox.Size = new System.Drawing.Size(50, 20);
            _learningRateBox.TabIndex = 3;
            _learningRateBox.Text = "";
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(100, 297);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 6;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // startButton
            // 
            _startButton.Enabled = false;
            _startButton.Location = new System.Drawing.Point(10, 297);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 5;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
            // 
            // groupBox3
            // 
            _groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    _saveFilesCheck,
                                                                                    _label13,
                                                                                    _weightsList,
                                                                                    _errorChart,
                                                                                    _label12});
            _groupBox3.Location = new System.Drawing.Point(445, 10);
            _groupBox3.Name = "_groupBox3";
            _groupBox3.Size = new System.Drawing.Size(220, 330);
            _groupBox3.TabIndex = 2;
            _groupBox3.TabStop = false;
            _groupBox3.Text = "Solution";
            // 
            // saveFilesCheck
            // 
            _saveFilesCheck.Location = new System.Drawing.Point(10, 305);
            _saveFilesCheck.Name = "_saveFilesCheck";
            _saveFilesCheck.Size = new System.Drawing.Size(195, 15);
            _saveFilesCheck.TabIndex = 4;
            _saveFilesCheck.Text = "Save weights and errors to files";
            // 
            // label13
            // 
            _label13.Location = new System.Drawing.Point(10, 170);
            _label13.Name = "_label13";
            _label13.Size = new System.Drawing.Size(100, 12);
            _label13.TabIndex = 3;
            _label13.Text = "Error\'s dynamics:";
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
            _weightsList.Location = new System.Drawing.Point(10, 35);
            _weightsList.Name = "_weightsList";
            _weightsList.Size = new System.Drawing.Size(200, 130);
            _weightsList.TabIndex = 2;
            _weightsList.View = System.Windows.Forms.View.Details;
            // 
            // errorChart
            // 
            _errorChart.Location = new System.Drawing.Point(10, 185);
            _errorChart.Name = "_errorChart";
            _errorChart.Size = new System.Drawing.Size(200, 110);
            _errorChart.TabIndex = 1;
            _errorChart.Text = "chart1";
            // 
            // label12
            // 
            _label12.Location = new System.Drawing.Point(10, 20);
            _label12.Name = "_label12";
            _label12.Size = new System.Drawing.Size(100, 15);
            _label12.TabIndex = 0;
            _label12.Text = "Network weights:";
            // 
            // columnHeader1
            // 
            _columnHeader1.Text = "Neuron";
            // 
            // columnHeader2
            // 
            _columnHeader2.Text = "Weight";
            // 
            // columnHeader3
            // 
            _columnHeader3.Text = "Value";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(674, 350);
            Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          _groupBox3,
                                                                          _groupBox2,
                                                                          _groupBox1});
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Classifier using Delta Rule Learning";
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

        // Load input data
        private void loadButton_Click(object sender, EventArgs e)
        {
            // data file format:
            // X1, X2, ..., Xn, class

            // load maximum 10 classes !

            // show file selection dialog
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                // temp buffers (for 200 samples only)
                double[,] tempData = null;
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

                        // allocate data array
                        if (_samples == 0)
                        {
                            _variables = strs.Length - 1;
                            tempData = new double[200, _variables];
                        }

                        // parse data
                        for (var j = 0; j < _variables; j++)
                        {
                            tempData[_samples, j] = double.Parse(strs[j]);
                        }
                        tempClasses[_samples] = int.Parse(strs[_variables]);

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
                    _data = new double[_samples, _variables];
                    Array.Copy(tempData, 0, _data, 0, _samples * _variables);
                    _classes = new int[_samples];
                    Array.Copy(tempClasses, 0, _classes, 0, _samples);
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

                _classesBox.Text = _classesCount.ToString();
                _oneNeuronForTwoCheck.Enabled = (_classesCount == 2);

                // set neurons count
                _neuronsCount = ((_classesCount == 2) && (_useOneNeuronForTwoClasses)) ? 1 : _classesCount;
                _neuronsBox.Text = _neuronsCount.ToString();

                ClearSolution();
                _startButton.Enabled = true;
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            _learningRateBox.Text = _learningRate.ToString();
            _alphaBox.Text = _sigmoidAlphaValue.ToString();
            _errorLimitBox.Text = _learningErrorLimit.ToString();
            _iterationsBox.Text = _iterationLimit.ToString();

            _oneNeuronForTwoCheck.Checked = _useOneNeuronForTwoClasses;
            _errorLimitCheck.Checked = _useErrorLimit;
            _saveFilesCheck.Checked = _saveStatisticsToFiles;
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

        // Use or not one neuron to classify two classes
        private void oneNeuronForTwoCheck_CheckedChanged(object sender, EventArgs e)
        {
            _useOneNeuronForTwoClasses = _oneNeuronForTwoCheck.Checked;
            // update neurons count box
            _neuronsCount = ((_classesCount == 2) && (_useOneNeuronForTwoClasses)) ? 1 : _classesCount;
            _neuronsBox.Text = _neuronsCount.ToString();
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            _learningRateBox.Enabled = enable;
            _alphaBox.Enabled = enable;
            _errorLimitBox.Enabled = enable;
            _iterationsBox.Enabled = enable;
            _oneNeuronForTwoCheck.Enabled = ((enable) && (_classesCount == 2));
            _errorLimitCheck.Enabled = enable;
            _saveFilesCheck.Enabled = enable;

            _loadButton.Enabled = enable;
            _startButton.Enabled = enable;
            _stopButton.Enabled = !enable;
        }

        // Clear current solution
        private void ClearSolution()
        {
            _errorChart.UpdateDataSeries("error", null);
            _weightsList.Items.Clear();
            _currentIterationBox.Text = string.Empty;
            _currentErrorBox.Text = string.Empty;
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
            // get sigmoid's alpha value
            try
            {
                _sigmoidAlphaValue = Math.Max(0.01, Math.Min(100, double.Parse(_alphaBox.Text)));
            }
            catch
            {
                _sigmoidAlphaValue = 2;
            }
            // get learning error limit
            try
            {
                _learningErrorLimit = Math.Max(0, double.Parse(_errorLimitBox.Text));
            }
            catch
            {
                _learningErrorLimit = 0.1;
            }
            // get iterations limit
            try
            {
                _iterationLimit = Math.Max(0, int.Parse(_iterationsBox.Text));
            }
            catch
            {
                _iterationLimit = 1000;
            }

            _useOneNeuronForTwoClasses = _oneNeuronForTwoCheck.Checked;
            _useErrorLimit = _errorLimitCheck.Checked;
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
            var reducedNetwork = ((_classesCount == 2) && (_useOneNeuronForTwoClasses));

            // prepare learning data
            var input = new double[_samples][];
            var output = new double[_samples][];

            for (var i = 0; i < _samples; i++)
            {
                input[i] = new double[_variables];
                output[i] = new double[_neuronsCount];

                // set input
                for (var j = 0; j < _variables; j++)
                    input[i][j] = _data[i, j];
                // set output
                if (reducedNetwork)
                {
                    output[i][0] = _classes[i];
                }
                else
                {
                    output[i][_classes[i]] = 1;
                }
            }

            // create perceptron
            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(_sigmoidAlphaValue), _variables, _neuronsCount);
            ActivationLayer layer = network[0];
            // create teacher
            DeltaRuleLearning teacher = new DeltaRuleLearning(network);
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
                        for (var i = 0; i < _neuronsCount; i++)
                        {
                            weightsFile.Write("neuron" + i + ";");
                            for (var j = 0; j < _variables; j++)
                                weightsFile.Write(layer[i][j] + ";");
                            weightsFile.WriteLine(layer[i].Threshold);
                        }
                    }

                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(input, output) / _samples;
                    errorsList.Add(error);

                    // save current error
                    if (errorsFile != null)
                    {
                        errorsFile.WriteLine(error);
                    }

                    // show current iteration & error
                    _currentIterationBox.Text = iteration.ToString();
                    _currentErrorBox.Text = error.ToString();
                    iteration++;

                    // check if we need to stop
                    if ((_useErrorLimit) && (error <= _learningErrorLimit))
                        break;
                    if ((!_useErrorLimit) && (_iterationLimit != 0) && (iteration > _iterationLimit))
                        break;
                }

                // show perceptron's weights
                _weightsList.Items.Clear();
                for (var i = 0; i < _neuronsCount; i++)
                {
                    var neuronName = string.Format("Neuron {0}", i + 1);
                    ListViewItem item = null;

                    // add all weights
                    for (var j = 0; j < _variables; j++)
                    {
                        item = _weightsList.Items.Add(neuronName);
                        item.SubItems.Add(string.Format("Weight {0}", j + 1));
                        item.SubItems.Add(layer[i][0].ToString("F6"));
                    }
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
                errorsFile?.Close();
                weightsFile?.Close();
            }

            // enable settings controls
            EnableControls(true);
        }
    }
}
