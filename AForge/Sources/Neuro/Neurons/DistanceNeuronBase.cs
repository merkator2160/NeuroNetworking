// AForge Neural Net Library
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

using System;

namespace AForge.Neuro.Neurons
{
    /// <summary>
	/// Distance neuronBase
	/// </summary>
	/// 
	/// <remarks>Distance neuronBase computes its output as distance between
	/// its weights and inputs. The neuronBase is usually used in Kohonen
	/// Self Organizing Map.</remarks>
	/// 
	public class DistanceNeuronBase : NeuronBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceNeuronBase"/> class
        /// </summary>
        /// 
        /// <param name="inputs">NeuronBase's inputs count</param>
        /// 
        public DistanceNeuronBase(int inputs) : base(inputs) { }


        /// <summary>
        /// Computes output value of neuronBase
        /// </summary>
        /// 
        /// <param name="input">Input vector</param>
        /// 
        /// <returns>The output value of distance neuronBase is equal to distance
        /// between its weights and inputs - sum of absolute differences.
        /// The output value is also stored in <see cref="NeuronBase.Output">Output</see>
        /// property.</returns>
        /// 
        public override double Compute(double[] input)
        {
            output = 0.0;

            // compute distance between inputs and weights
            for (var i = 0; i < inputsCount; i++)
            {
                output += Math.Abs(Weights[i] - input[i]);
            }
            return output;
        }
    }
}
