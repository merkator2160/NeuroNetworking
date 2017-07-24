// AForge Neural Net Library
//
// Copyright � Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using AForge.Neuro.Layers;
using AForge.Neuro.Networks;
using System;

namespace AForge.Neuro.Learning
{
    /// <summary>
    /// Elastic network learning algorithm
    /// </summary>
    ///
    /// <remarks>This class implements elastic network's learning algorithm and
    /// allows to train <see cref="DistanceNetwork">Distance Networks</see>.
    /// </remarks> 
    ///
    public class ElasticNetworkLearning : IUnsupervisedLearning
    {
        // neural network to train
        private DistanceNetwork _network;

        // array of distances between NeuronsBase
        private double[] _distance;

        // learning rate
        private double _learningRate = 0.1;
        // learning radius
        private double _learningRadius = 0.5;

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
        /// Default value equals to 0.5.</remarks>
        /// 
        public double LearningRadius
        {
            get => _learningRadius;
            set
            {
                _learningRadius = Math.Max(0, Math.Min(1.0, value));
                _squaredRadius2 = 2 * _learningRadius * _learningRadius;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticNetworkLearning"/> class
        /// </summary>
        /// 
        /// <param name="network">Neural network to train</param>
        /// 
        public ElasticNetworkLearning(DistanceNetwork network)
        {
            this._network = network;

            // precalculate distances array
            var neurons = network[0].NeuronsCount;
            var deltaAlpha = Math.PI * 2.0 / neurons;
            var alpha = deltaAlpha;

            _distance = new double[neurons];
            _distance[0] = 0.0;

            // calculate all distance values
            for (var i = 1; i < neurons; i++)
            {
                var dx = 0.5 * Math.Cos(alpha) - 0.5;
                var dy = 0.5 * Math.Sin(alpha);

                _distance[i] = dx * dx + dy * dy;

                alpha += deltaAlpha;
            }
        }


        /// <summary>
        /// Runs learning iteration
        /// </summary>
        /// 
        /// <param name="input">input vector</param>
        /// 
        /// <returns>Returns learning error - summary absolute difference between updated
        /// weights and according inputs. The difference is measured according to the NeuronsBase
        /// distance to the winner neuronBase.</returns>
        /// 
        public double Run(double[] input)
        {
            var error = 0.0;

            // compute the network
            _network.Compute(input);
            var winner = _network.GetWinner();

            // get layer of the network
            Layer layer = _network[0];

            // walk through all NeuronsBase of the layer
            for (int j = 0, m = layer.NeuronsCount; j < m; j++)
            {
                var neuronBase = layer[j];

                // update factor
                var factor = Math.Exp(-_distance[Math.Abs(j - winner)] / _squaredRadius2);

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
