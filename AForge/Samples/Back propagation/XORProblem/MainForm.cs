// AForge Framework
// XOR Problem solution using Multi-Layer Neural Network
//
// Copyright © Andrew Kirillov, 2006
// andrew.kirillov@gmail.com
//

using System;
using System.Collections;
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

namespace XORProblem
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : Form
    {
        private GroupBox _groupBox1;
        private Label _label1;
        private TextBox _learningRateBox;
        private Label _label2;
        private TextBox _alphaBox;
        private Label _label3;
        private TextBox _errorLimitBox;
        private Label _label4;
        private ComboBox _sigmoidTypeCombo;
        private TextBox _currentErrorBox;
        private Label _label11;
        private TextBox _currentIterationBox;
        private Label _label8;
        private Label _label7;
        private Label _label5;
        private Button _stopButton;
        private Button _startButton;
        private GroupBox _groupBox2;
        private AForge.Controls.Chart _errorChart;
        private CheckBox _saveFilesCheck;
        private Label _label6;
        private TextBox _momentumBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container _components = null;

        private double _learningRate = 0.1;
        private double _momentum;
        private double _sigmoidAlphaValue = 2.0;
        private double _learningErrorLimit = 0.1;
        private int _sigmoidType;
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

            // update controls
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
            _stopButton = new System.Windows.Forms.Button();
            _startButton = new System.Windows.Forms.Button();
            _label5 = new System.Windows.Forms.Label();
            _currentErrorBox = new System.Windows.Forms.TextBox();
            _label11 = new System.Windows.Forms.Label();
            _currentIterationBox = new System.Windows.Forms.TextBox();
            _label8 = new System.Windows.Forms.Label();
            _label7 = new System.Windows.Forms.Label();
            _sigmoidTypeCombo = new System.Windows.Forms.ComboBox();
            _errorLimitBox = new System.Windows.Forms.TextBox();
            _label3 = new System.Windows.Forms.Label();
            _alphaBox = new System.Windows.Forms.TextBox();
            _label2 = new System.Windows.Forms.Label();
            _learningRateBox = new System.Windows.Forms.TextBox();
            _label1 = new System.Windows.Forms.Label();
            _label4 = new System.Windows.Forms.Label();
            _groupBox2 = new System.Windows.Forms.GroupBox();
            _errorChart = new AForge.Controls.Chart();
            _saveFilesCheck = new System.Windows.Forms.CheckBox();
            _label6 = new System.Windows.Forms.Label();
            _momentumBox = new System.Windows.Forms.TextBox();
            _groupBox1.SuspendLayout();
            _groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            _groupBox1.Controls.Add(_momentumBox);
            _groupBox1.Controls.Add(_label6);
            _groupBox1.Controls.Add(_stopButton);
            _groupBox1.Controls.Add(_startButton);
            _groupBox1.Controls.Add(_label5);
            _groupBox1.Controls.Add(_currentErrorBox);
            _groupBox1.Controls.Add(_label11);
            _groupBox1.Controls.Add(_currentIterationBox);
            _groupBox1.Controls.Add(_label8);
            _groupBox1.Controls.Add(_label7);
            _groupBox1.Controls.Add(_sigmoidTypeCombo);
            _groupBox1.Controls.Add(_errorLimitBox);
            _groupBox1.Controls.Add(_label3);
            _groupBox1.Controls.Add(_alphaBox);
            _groupBox1.Controls.Add(_label2);
            _groupBox1.Controls.Add(_learningRateBox);
            _groupBox1.Controls.Add(_label1);
            _groupBox1.Controls.Add(_label4);
            _groupBox1.Location = new System.Drawing.Point(10, 10);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(195, 260);
            _groupBox1.TabIndex = 0;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Neural Network";
            // 
            // stopButton
            // 
            _stopButton.Enabled = false;
            _stopButton.Location = new System.Drawing.Point(110, 225);
            _stopButton.Name = "_stopButton";
            _stopButton.TabIndex = 28;
            _stopButton.Text = "S&top";
            _stopButton.Click += new System.EventHandler(stopButton_Click);
            // 
            // startButton
            // 
            _startButton.Location = new System.Drawing.Point(25, 225);
            _startButton.Name = "_startButton";
            _startButton.TabIndex = 27;
            _startButton.Text = "&Start";
            _startButton.Click += new System.EventHandler(startButton_Click);
            // 
            // label5
            // 
            _label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label5.Location = new System.Drawing.Point(10, 211);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(175, 2);
            _label5.TabIndex = 26;
            // 
            // currentErrorBox
            // 
            _currentErrorBox.Location = new System.Drawing.Point(125, 185);
            _currentErrorBox.Name = "_currentErrorBox";
            _currentErrorBox.ReadOnly = true;
            _currentErrorBox.Size = new System.Drawing.Size(60, 20);
            _currentErrorBox.TabIndex = 25;
            _currentErrorBox.Text = "";
            // 
            // label11
            // 
            _label11.Location = new System.Drawing.Point(10, 187);
            _label11.Name = "_label11";
            _label11.Size = new System.Drawing.Size(121, 14);
            _label11.TabIndex = 24;
            _label11.Text = "Current average error:";
            // 
            // currentIterationBox
            // 
            _currentIterationBox.Location = new System.Drawing.Point(125, 160);
            _currentIterationBox.Name = "_currentIterationBox";
            _currentIterationBox.ReadOnly = true;
            _currentIterationBox.Size = new System.Drawing.Size(60, 20);
            _currentIterationBox.TabIndex = 23;
            _currentIterationBox.Text = "";
            // 
            // label8
            // 
            _label8.Location = new System.Drawing.Point(10, 162);
            _label8.Name = "_label8";
            _label8.Size = new System.Drawing.Size(98, 16);
            _label8.TabIndex = 22;
            _label8.Text = "Current iteration:";
            // 
            // label7
            // 
            _label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _label7.Location = new System.Drawing.Point(10, 150);
            _label7.Name = "_label7";
            _label7.Size = new System.Drawing.Size(175, 2);
            _label7.TabIndex = 21;
            // 
            // sigmoidTypeCombo
            // 
            _sigmoidTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _sigmoidTypeCombo.Items.AddRange(new object[] {
                                                                  "Unipolar",
                                                                  "Bipolar"});
            _sigmoidTypeCombo.Location = new System.Drawing.Point(125, 120);
            _sigmoidTypeCombo.Name = "_sigmoidTypeCombo";
            _sigmoidTypeCombo.Size = new System.Drawing.Size(60, 21);
            _sigmoidTypeCombo.TabIndex = 9;
            // 
            // errorLimitBox
            // 
            _errorLimitBox.Location = new System.Drawing.Point(125, 95);
            _errorLimitBox.Name = "_errorLimitBox";
            _errorLimitBox.Size = new System.Drawing.Size(60, 20);
            _errorLimitBox.TabIndex = 7;
            _errorLimitBox.Text = "";
            // 
            // label3
            // 
            _label3.Location = new System.Drawing.Point(10, 97);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(110, 15);
            _label3.TabIndex = 6;
            _label3.Text = "Learning error limit:";
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
            // label4
            // 
            _label4.Location = new System.Drawing.Point(10, 122);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(100, 15);
            _label4.TabIndex = 8;
            _label4.Text = "Sigmoid\'s type:";
            // 
            // groupBox2
            // 
            _groupBox2.Controls.Add(_saveFilesCheck);
            _groupBox2.Controls.Add(_errorChart);
            _groupBox2.Location = new System.Drawing.Point(215, 10);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new System.Drawing.Size(220, 260);
            _groupBox2.TabIndex = 1;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Error\'s dynamics";
            // 
            // errorChart
            // 
            _errorChart.Location = new System.Drawing.Point(10, 20);
            _errorChart.Name = "_errorChart";
            _errorChart.Size = new System.Drawing.Size(200, 205);
            _errorChart.TabIndex = 0;
            _errorChart.Text = "chart1";
            // 
            // saveFilesCheck
            // 
            _saveFilesCheck.Location = new System.Drawing.Point(10, 233);
            _saveFilesCheck.Name = "_saveFilesCheck";
            _saveFilesCheck.Size = new System.Drawing.Size(200, 18);
            _saveFilesCheck.TabIndex = 1;
            _saveFilesCheck.Text = "Save errors to files";
            // 
            // label6
            // 
            _label6.Location = new System.Drawing.Point(10, 47);
            _label6.Name = "_label6";
            _label6.Size = new System.Drawing.Size(82, 17);
            _label6.TabIndex = 2;
            _label6.Text = "Momentum:";
            // 
            // momentumBox
            // 
            _momentumBox.Location = new System.Drawing.Point(125, 45);
            _momentumBox.Name = "_momentumBox";
            _momentumBox.Size = new System.Drawing.Size(60, 20);
            _momentumBox.TabIndex = 3;
            _momentumBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(452, 278);
            Controls.Add(_groupBox2);
            Controls.Add(_groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "XOR Problem";
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
            _learningRateBox.Text = _learningRate.ToString();
            _momentumBox.Text = _momentum.ToString();
            _alphaBox.Text = _sigmoidAlphaValue.ToString();
            _errorLimitBox.Text = _learningErrorLimit.ToString();
            _sigmoidTypeCombo.SelectedIndex = _sigmoidType;

            _saveFilesCheck.Checked = _saveStatisticsToFiles;
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            _learningRateBox.Enabled = enable;
            _momentumBox.Enabled = enable;
            _alphaBox.Enabled = enable;
            _errorLimitBox.Enabled = enable;
            _sigmoidTypeCombo.Enabled = enable;
            _saveFilesCheck.Enabled = enable;

            _startButton.Enabled = enable;
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
            // get sigmoid's type
            _sigmoidType = _sigmoidTypeCombo.SelectedIndex;

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
            // initialize input and output values
            double[][] input = null;
            double[][] output = null;

            if (_sigmoidType == 0)
            {
                // unipolar data
                input = new double[4][] {
                                            new double[] {0, 0},
                                            new double[] {0, 1},
                                            new double[] {1, 0},
                                            new double[] {1, 1}
                                        };
                output = new double[4][] {
                                             new double[] {0},
                                             new double[] {1},
                                             new double[] {1},
                                             new double[] {0}
                                         };
            }
            else
            {
                // biipolar data
                input = new double[4][] {
                                            new double[] {-1, -1},
                                            new double[] {-1,  1},
                                            new double[] { 1, -1},
                                            new double[] { 1,  1}
                                        };
                output = new double[4][] {
                                             new double[] {-1},
                                             new double[] { 1},
                                             new double[] { 1},
                                             new double[] {-1}
                                         };
            }

            // create perceptron
            ActivationNetwork network = new ActivationNetwork(
                (_sigmoidType == 0) ?
                    (IActivationFunction)new SigmoidFunction(_sigmoidAlphaValue) :
                    (IActivationFunction)new BipolarSigmoidFunction(_sigmoidAlphaValue),
                2, 2, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = _learningRate;
            teacher.Momentum = _momentum;

            // iterations
            var iteration = 1;

            // statistic files
            StreamWriter errorsFile = null;

            try
            {
                // check if we need to save statistics to files
                if (_saveStatisticsToFiles)
                {
                    // open files
                    errorsFile = File.CreateText("errors.csv");
                }

                // erros list
                var errorsList = new ArrayList();

                // loop
                while (!_needToStop)
                {
                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(input, output);
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
                    if (error <= _learningErrorLimit)
                        break;
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
            }

            // enable settings controls
            EnableControls(true);
        }
    }
}
