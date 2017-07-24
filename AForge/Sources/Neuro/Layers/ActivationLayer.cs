// AForge Neural Net Library
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using AForge.Neuro.Activation_Functions;
using AForge.Neuro.Neurons;

namespace AForge.Neuro.Layers
{
    /// <summary>
	/// Activation layer
	/// </summary>
	/// 
	/// <remarks>Activation layer is a layer of <see cref="ActivationNeuronBase">activation NeuronsBase</see>.
	/// The layer is usually used in multi-layer neural networks.</remarks>
	///
	public class ActivationLayer : Layer
    {
        /// <summary>
        /// Layer's NeuronsBase accessor
        /// </summary>
        /// 
        /// <param name="index">NeuronBase index</param>
        /// 
        /// <remarks>Allows to access layer's NeuronsBase.</remarks>
        /// 
        public new ActivationNeuronBase this[int index] => (ActivationNeuronBase)NeuronsBase[index];


        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationLayer"/> class
        /// </summary>
        /// <param name="neuronsCount">Layer's NeuronsBase count</param>
        /// <param name="inputsCount">Layer's inputs count</param>
        /// <param name="function">Activation function of NeuronsBase of the layer</param>
        /// 
        /// <remarks>The new layet will be randomized (see <see cref="ActivationNeuronBase.RandomizeCurrentNeuron"/>
        /// method) after it is created.</remarks>
        /// 
        public ActivationLayer(int neuronsCount, int inputsCount, IActivationFunction function)
                            : base(neuronsCount, inputsCount)
        {
            // create each neuronBase
            for (var i = 0; i < neuronsCount; i++)
                NeuronsBase[i] = new ActivationNeuronBase(inputsCount, function);
        }
    }
}
