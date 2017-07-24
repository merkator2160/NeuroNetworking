// AForge Neural Net Library
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using AForge.Neuro.Activation_Functions;
using System;

namespace AForge.Neuro.Neurons
{
    /// <summary>
	/// Activation neuronBase
	/// </summary>
	/// 
	/// <remarks>Activation neuronBase computes weighted sum of its inputs, adds
	/// threshold value and then applies activation function. The neuronBase is
	/// usually used in multi-layer neural networks.</remarks>
	/// 
	public class ActivationNeuronBase : NeuronBase
    {
        /// <summary>
        /// Threshold value
        /// </summary>
        /// 
        /// <remarks>The value is added to inputs weighted sum.</remarks>
        /// 
        protected double threshold;

        /// <summary>
        /// Activation function
        /// </summary>
        /// 
        /// <remarks>The function is applied to inputs weighted sum plus
        /// threshold value.</remarks>
        /// 
        protected IActivationFunction Function;

        /// <summary>
        /// Threshold value
        /// </summary>
        /// 
        /// <remarks>The value is added to inputs weighted sum.</remarks>
        /// 
        public double Threshold
        {
            get => threshold;
            set => threshold = value;
        }

        /// <summary>
        /// NeuronBase's activation function
        /// </summary>
        /// 
        public IActivationFunction ActivationFunction => Function;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationNeuronBase"/> class
        /// </summary>
        /// 
        /// <param name="inputs">NeuronBase's inputs count</param>
        /// <param name="function">NeuronBase's activation function</param>
        /// 
        public ActivationNeuronBase(int inputs, IActivationFunction function) : base(inputs)
        {
            this.Function = function;
        }

        /// <summary>
        /// RandomizeCurrentNeuron neuronBase 
        /// </summary>
        /// 
        /// <remarks>Calls base class <see cref="NeuronBase.RandomizeCurrentNeuron">RandomizeCurrentNeuron</see> method
        /// to randomize neuronBase's weights and then randomize threshold's value.</remarks>
        /// 
        public override void RandomizeCurrentNeuron()
        {
            // randomize weights
            base.RandomizeCurrentNeuron();
            // randomize threshold
            threshold = Rand.NextDouble() * (randRange.Length) + randRange.Min;
        }

        /// <summary>
        /// Computes output value of neuronBase
        /// </summary>
        /// 
        /// <param name="input">Input vector</param>
        /// 
        /// <returns>Returns neuron's output value</returns>
        /// 
        /// <remarks>The output value of activation neuronBase is equal to value
        /// of nueron's activation function, which parameter is weighted sum
        /// of its inputs plus threshold value. The output value is also stored
        /// in <see cref="NeuronBase.Output">Output</see> property.</remarks>
        /// 
        public override double Compute(double[] input)
        {
            // check for corrent input vector
            if (input.Length != inputsCount)
                throw new ArgumentException();

            // initial sum value
            var sum = 0.0;

            // compute weighted sum of inputs
            for (var i = 0; i < inputsCount; i++)
            {
                sum += Weights[i] * input[i];
            }
            sum += threshold;

            return (output = Function.Function(sum));
        }
    }
}
