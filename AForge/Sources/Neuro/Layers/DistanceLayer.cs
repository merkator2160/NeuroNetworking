// AForge Neural Net Library
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using AForge.Neuro.Neurons;

namespace AForge.Neuro.Layers
{
    /// <summary>
	/// Distance layer
	/// </summary>
	/// 
	/// <remarks>Distance layer is a layer of <see cref="DistanceNeuronBase">distance NeuronsBase</see>.
	/// The layer is usually a single layer of such networks as Kohonen Self
	/// Organizing Map, Elastic Net, Hamming Memory Net.</remarks>
	/// 
	public class DistanceLayer : Layer
    {
        /// <summary>
        /// Layer's NeuronsBase accessor
        /// </summary>
        /// 
        /// <param name="index">NeuronBase index</param>
        /// 
        /// <remarks>Allows to access layer's NeuronsBase.</remarks>
        /// 
        public new DistanceNeuronBase this[int index] => (DistanceNeuronBase)NeuronsBase[index];


        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceLayer"/> class
        /// </summary>
        /// 
        /// <param name="neuronsCount">Layer's NeuronsBase count</param>
        /// <param name="inputsCount">Layer's inputs count</param>
        /// 
        /// <remarks>The new layet will be randomized (see <see cref="NeuronBase.RandomizeCurrentNeuron"/>
        /// method) after it is created.</remarks>
        /// 
        public DistanceLayer(int neuronsCount, int inputsCount)
                        : base(neuronsCount, inputsCount)
        {
            // create each neuronBase
            for (var i = 0; i < neuronsCount; i++)
                NeuronsBase[i] = new DistanceNeuronBase(inputsCount);
        }
    }
}
