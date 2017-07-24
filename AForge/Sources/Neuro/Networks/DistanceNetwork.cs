// AForge Neural Net Library
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using AForge.Neuro.Layers;
using AForge.Neuro.Neurons;

namespace AForge.Neuro.Networks
{
    /// <summary>
	/// Distance network
	/// </summary>
	///
	/// <remarks>Distance network is a neural network of only one <see cref="DistanceLayer">distance
	/// layer</see>. The network is a base for such neural networks as SOM, Elastic net, etc.
	/// </remarks>
	///
	public class DistanceNetwork : Network
    {
        /// <summary>
        /// Network's layers accessor
        /// </summary>
        /// 
        /// <param name="index">Layer index</param>
        /// 
        /// <remarks>Allows to access network's layer.</remarks>
        /// 
        public new DistanceLayer this[int index] => ((DistanceLayer)Layers[index]);

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceNetwork"/> class
        /// </summary>
        /// 
        /// <param name="inputsCount">Network's inputs count</param>
        /// <param name="neuronsCount">Network's NeuronsBase count</param>
        /// 
        /// <remarks>The new network will be randomized (see <see cref="NeuronBase.RandomizeCurrentNeuron"/>
        /// method) after it is created.</remarks>
        /// 
        public DistanceNetwork(int inputsCount, int neuronsCount)
                        : base(inputsCount, 1)
        {
            // create layer
            Layers[0] = new DistanceLayer(neuronsCount, inputsCount);
        }

        /// <summary>
        /// Get winner neuronBase
        /// </summary>
        /// 
        /// <returns>Index of the winner neuronBase</returns>
        /// 
        /// <remarks>The method returns index of the neuronBase, which weights have
        /// the minimum distance from network's input.</remarks>
        /// 
        public int GetWinner()
        {
            // find the MIN value
            var min = output[0];
            var minIndex = 0;

            for (int i = 1, n = output.Length; i < n; i++)
            {
                if (output[i] < min)
                {
                    // found new MIN value
                    min = output[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }
    }
}
