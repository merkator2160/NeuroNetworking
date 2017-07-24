// AForge Neural Net Library
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using AForge.Neuro.Layers;
using AForge.Neuro.Networks;
using System;

namespace AForge.Neuro.Learning
{
    /// <summary>
    /// Kohonen Self Organizing Map (SOM) learning algorithm
    /// </summary>
    /// 
    /// <remarks>This class implements Kohonen's SOM learning algorithm and
    /// is widelí used in clusterization tasks. The class allows to train
    /// <see cref="DistanceNetwork">Distance Networks</see>.</remarks>
    /// 
    public class SomLearning : IUnsupervisedLearning
    {
        // neural network to train
        private DistanceNetwork _network;
        // network's dimension
        private int _width;
        private int _height;

        // learning rate
        private double _learningRate = 0.1;
        // learning radius
        private double _learningRadius = 7;

        // squared learning radius multiplied by 2 (precalculated value to speed up computations)
        private double _squaredRadius2 = 2 * 7 * 7;

        /// <summary>
        /// Learning rate
        /// </summary>
        /// 
        /// <remarks>Determines speed of learning. Value range is [0, 1].
        /// Default value equals to 0.1.</remarks>
        /// 
        public double LearningRate
        {
            get => _learningRate;
            set => _learningRate = Math.Max(0.0, Math.Min(1.0, value));
        }

        /// <summary>
        /// Learning radius
        /// </summary>
        /// 
        /// <remarks>Determines the amount of NeuronsBase to be updated around
        /// winner neuronBase. Neurons, which are in the circle of specified radius,
        /// are updated during the learning procedure. Neurons, which are closer
        /// to the winner neuronBase, get more update.<br /><br />
        /// Default value equals to 7.</remarks>
        /// 
        public double LearningRadius
        {
            get => _learningRadius;
            set
            {
                _learningRadius = Math.Max(0, value);
                _squaredRadius2 = 2 * _learningRadius * _learningRadius;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SomLearning"/> class
        /// </summary>
        /// 
        /// <param name="network">Neural network to train</param>
        /// 
        /// <remarks>This constructor supposes that a square network will be passed for training -
        /// it should be possible to get square root of network's NeuronsBase amount.</remarks>
        /// 
        public SomLearning(DistanceNetwork network)
        {
            // network's dimension was not specified, let's try to guess
            var neuronsCount = network[0].NeuronsCount;
            _width = (int)Math.Sqrt(neuronsCount);

            if (_width * _width != neuronsCount)
            {
                throw new ArgumentException("Invalid network size");
            }

            // ok, we got it
            this._network = network;
            _width = _width;
            _height = _height;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SomLearning"/> class
        /// </summary>
        /// 
        /// <param name="network">Neural network to train</param>
        /// <param name="width">Neural network's width</param>
        /// <param name="height">Neural network's height</param>
        ///
        /// <remarks>The constructor allows to pass network of arbitrary rectangular shape.
        /// The amount of NeuronsBase in the network should be equal to <b>width</b> * <b>height</b>.
        /// </remarks> 
        ///
        public SomLearning(DistanceNetwork network, int width, int height)
        {
            // check network size
            if (network[0].NeuronsCount != width * height)
            {
                throw new ArgumentException("Invalid network size");
            }

            this._network = network;
            this._width = width;
            this._height = height;
        }

        /// <summary>
        /// Runs learning iteration
        /// </summary>
        /// 
        /// <param name="input">input vector</param>
        /// 
        /// <returns>Returns learning error - summary absolute difference between updated
        /// weights and according inputs. The difference is measured according to the NeuronsBase
        /// distance to the  winner neuronBase.</returns>
        /// 
        public double Run(double[] input)
        {
            var error = 0.0;

            // compute the network
            _network.Compute(input);
            var winner = _network.GetWinner();

            // get layer of the network
            Layer layer = _network[0];

            // check learning radius
            if (_learningRadius == 0)
            {
                var neuronBase = layer[winner];

                // update weight of the winner only
                for (int i = 0, n = neuronBase.InputsCount; i < n; i++)
                {
                    neuronBase[i] += (input[i] - neuronBase[i]) * _learningRate;
                }
            }
            else
            {
                // winner's X and Y
                var wx = winner % _width;
                var wy = winner / _width;

                // walk through all NeuronsBase of the layer
                for (int j = 0, m = layer.NeuronsCount; j < m; j++)
                {
                    var neuronBase = layer[j];

                    var dx = (j % _width) - wx;
                    var dy = (j / _width) - wy;

                    // update factor ( Gaussian based )
                    var factor = Math.Exp(-(double)(dx * dx + dy * dy) / _squaredRadius2);

                    // update weight of the neuronBase
                    for (int i = 0, n = neuronBase.InputsCount; i < n; i++)
                    {
                        // calculate the error
                        var e = (input[i] - neuronBase[i]) * factor;
                        error += Math.Abs(e);
                        // update weight
                        neuronBase[i] += e * _learningRate;
                    }
                }
            }
            return error;
        }

        /// <summary>
        /// Runs learning epoch
        /// </summary>
        /// 
        /// <param name="input">array of input vectors</param>
        /// 
        /// <returns>Returns summary learning error for the epoch. See <see cref="Run"/>
        /// method for details about learning error calculation.</returns>
        /// 
        public double RunEpoch(double[][] input)
        {
            var error = 0.0;

            // walk through all training samples
            foreach (var sample in input)
            {
                error += Run(sample);
            }

            // return summary error
            return error;
        }
    }
}
