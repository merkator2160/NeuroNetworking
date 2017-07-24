// AForge Neural Net Library
//
// Copyright � Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using AForge.Neuro.Activation_Functions;
using AForge.Neuro.Networks;
using AForge.Neuro.Neurons;
using System;

namespace AForge.Neuro.Learning
{
    /// <summary>
    /// Delta rule learning algorithm
    /// </summary>
    /// 
    /// <remarks>This learning algorithm is used to train one layer neural
    /// network of <see cref="ActivationNeuronBase">Activation Neurons</see>
    /// with continuous activation function, see <see cref="SigmoidFunction"/>
    /// for example.</remarks>
    /// 
    public class DeltaRuleLearning : ISupervisedLearning
    {
        // network to teach
        private ActivationNetwork _network;
        // learning rate
        private double _learningRate = 0.1;

        /// <summary>
        /// Learning rate
        /// </summary>
        /// 
        /// <remarks>The value determines speed of learning  in the range of [0, 1].
        /// Default value equals to 0.1.</remarks>
        /// 
        public double LearningRate
        {
            get => _learningRate;
            set => _learningRate = Math.Max(0.0, Math.Min(1.0, value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaRuleLearning"/> class
        /// </summary>
        /// 
        /// <param name="network">Network to teach</param>
        /// 
        public DeltaRuleLearning(ActivationNetwork network)
        {
            // check layers count
            if (network.LayersCount != 1)
            {
                throw new ArgumentException("Invalid nuaral network. It should have one layer only.");
            }

            this._network = network;
        }

        /// <summary>
        /// Runs learning iteration
        /// </summary>
        /// 
        /// <param name="input">input vector</param>
        /// <param name="output">desired output vector</param>
        /// 
        /// <returns>Returns squared error divided by 2</returns>
        /// 
        /// <remarks>Runs one learning iteration and updates neuronBase's
        /// weights.</remarks>
        ///
        public double Run(double[] input, double[] output)
        {
            // compute output of network
            var networkOutput = _network.Compute(input);

            // get the only layer of the network
            var layer = _network[0];
            // get activation function of the layer
            var activationFunction = layer[0].ActivationFunction;

            // summary network absolute error
            var error = 0.0;

            // update weights of each neuronBase
            for (int j = 0, k = layer.NeuronsCount; j < k; j++)
            {
                // get neuronBase of the layer
                var neuronBase = layer[j];
                // calculate neuronBase's error
                var e = output[j] - networkOutput[j];
                // get activation function's derivative
                var functionDerivative = activationFunction.Derivative2(networkOutput[j]);

                // update weights
                for (int i = 0, n = neuronBase.InputsCount; i < n; i++)
                {
                    neuronBase[i] += _learningRate * e * functionDerivative * input[i];
                }

                // update threshold value
                neuronBase.Threshold += _learningRate * e * functionDerivative;

                // sum error
                error += (e * e);
            }

            return error / 2;
        }

        /// <summary>
        /// Runs learning epoch
        /// </summary>
        /// 
        /// <param name="input">array of input vectors</param>
        /// <param name="output">array of output vectors</param>
        /// 
        /// <returns>Returns sum of squared errors divided by 2</returns>
        /// 
        /// <remarks>Runs series of learning iterations - one iteration
        /// for each input sample. Updates neuronBase's weights after each sample
        /// presented.</remarks>
        /// 
        public double RunEpoch(double[][] input, double[][] output)
        {
            var error = 0.0;

            // run learning procedure for all samples
            for (int i = 0, n = input.Length; i < n; i++)
            {
                error += Run(input[i], output[i]);
            }

            // return summary error
            return error;
        }
    }
}
