// AForge Neural Net Library
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using AForge.Neuro.Neurons;
using System;

namespace AForge.Neuro.Layers
{
    /// <summary>
	/// Base neural layer class
	/// </summary>
	/// 
	/// <remarks>This is a base neural layer class, which represents
	/// collection of NeuronsBase.</remarks>
	/// 
	public abstract class Layer
    {
        /// <summary>
        /// Layer's inputs count
        /// </summary>
        protected int inputsCount;

        /// <summary>
        /// Layer's NeuronsBase count
        /// </summary>
        protected int neuronsCount;

        /// <summary>
        /// Layer's NeuronsBase
        /// </summary>
        protected NeuronBase[] NeuronsBase;

        /// <summary>
        /// Layer's output vector
        /// </summary>
        protected double[] output;

        /// <summary>
        /// Layer's inputs count
        /// </summary>
        public int InputsCount => inputsCount;

        /// <summary>
        /// Layer's NeuronsBase count
        /// </summary>
        public int NeuronsCount => neuronsCount;

        /// <summary>
        /// Layer's output vector
        /// </summary>
        /// 
        /// <remarks>The calculation way of layer's output vector is determined by
        /// inherited class.</remarks>
        /// 
        public double[] Output => output;

        /// <summary>
        /// Layer's NeuronsBase accessor
        /// </summary>
        /// 
        /// <param name="index">NeuronBase index</param>
        /// 
        /// <remarks>Allows to access layer's NeuronsBase.</remarks>
        /// 
        public NeuronBase this[int index] => NeuronsBase[index];


        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class
        /// </summary>
        /// 
        /// <param name="neuronsCount">Layer's NeuronsBase count</param>
        /// <param name="inputsCount">Layer's inputs count</param>
        /// 
        /// <remarks>Protected contructor, which initializes <see cref="inputsCount"/>,
        /// <see cref="neuronsCount"/>, <see cref="NeuronsBase"/> and <see cref="output"/>
        /// members.</remarks>
        /// 
        protected Layer(int neuronsCount, int inputsCount)
        {
            this.inputsCount = Math.Max(1, inputsCount);
            this.neuronsCount = Math.Max(1, neuronsCount);
            // create collection of NeuronsBase
            NeuronsBase = new NeuronBase[this.neuronsCount];
            // allocate output array
            output = new double[this.neuronsCount];
        }


        /// <summary>
        /// Compute output vector of the layer 
        /// </summary>
        /// 
        /// <param name="input">Input vector</param>
        /// 
        /// <returns>Returns layer's output vector</returns>
        /// 
        /// <remarks>The actual layer's output vector is determined by inherited class and it
        /// consists of output values of layer's NeuronsBase. The output vector is also stored in
        /// <see cref="Output"/> property.</remarks>
        /// 
        public virtual double[] Compute(double[] input)
        {
            // compute each neuronBase
            for (var i = 0; i < neuronsCount; i++)
                output[i] = NeuronsBase[i].Compute(input);

            return output;
        }


        /// <summary>
        /// RandomizeCurrentNeuron NeuronsBase of the layer
        /// </summary>
        /// 
        /// <remarks>Randomizes layer's NeuronsBase by calling <see cref="NeuronBase.RandomizeCurrentNeuron"/> method
        /// of each neuronBase.</remarks>
        /// 
        public virtual void Randomize()
        {
            foreach (var neuron in NeuronsBase)
                neuron.RandomizeCurrentNeuron();
        }
    }
}
