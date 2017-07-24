using AForge.Core;
using System;

namespace AForge.Neuro.Neurons
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NeuronBase
    {
        /// <summary>
        /// NeuronBase's inputs count
        /// </summary>
        protected int inputsCount = 0;

        /// <summary>
        /// Nouron's wieghts
        /// </summary>
        protected double[] Weights;

        /// <summary>
        /// NeuronBase's output value
        /// </summary>
        protected double output = 0;

        /// <summary>
        /// Random number generator
        /// </summary>
        /// 
        /// <remarks>The generator is used for neuronBase's weights randomization</remarks>
        /// 
        protected static Random Rand = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Random generator range
        /// </summary>
        /// 
        /// <remarks>Sets the range of random generator. Affects initial values of neuronBase's weight.
        /// Default value is [0, 1].</remarks>
        /// 
        protected static DoubleRange randRange = new DoubleRange(0.0, 1.0);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputsCount"></param>
        protected NeuronBase(int inputsCount)
        {
            AllocateWeights(inputsCount);
            RandomizeCurrentNeuron();
        }





        /// <summary>
        /// Random number generator
        /// </summary>
        /// 
        /// <remarks>The property allows to initialize random generator with a custom seed. The generator is
        /// used for neuronBase's weights randomization.</remarks>
        /// 
        public static Random RandGenerator
        {
            get => Rand;
            set
            {
                if (value != null)
                {
                    Rand = value;
                }
            }
        }

        /// <summary>
        /// Random generator range
        /// </summary>
        public static DoubleRange RandRange
        {
            get => randRange;
            set
            {
                if (value != null)
                {
                    randRange = value;
                }
            }
        }

        /// <summary>
        /// NeuronBase's inputs count
        /// </summary>
        public int InputsCount => inputsCount;

        /// <summary>
        /// NeuronBase's output value
        /// </summary>
        /// 
        /// <remarks>The calculation way of neuronBase's output value is determined by inherited class.</remarks>
        /// 
        public double Output => output;

        /// <summary>
        /// NeuronBase's weights accessor
        /// </summary>
        /// 
        /// <param name="index">Weight index</param>
        /// 
        /// <remarks>Allows to access neuronBase's weights.</remarks>
        /// 
        public double this[int index]
        {
            get => Weights[index];
            set => Weights[index] = value;
        }

        /// <summary>
        /// RandomizeCurrentNeuron neuronBase 
        /// </summary>
        /// 
        /// <remarks>Initialize neuronBase's weights with random values within the range specified
        /// by <see cref="RandRange"/>.</remarks>
        /// 
        public virtual void RandomizeCurrentNeuron()
        {
            var d = randRange.Length;

            // randomize weights
            for (var i = 0; i < inputsCount; i++)
                Weights[i] = Rand.NextDouble() * d + randRange.Min;
        }

        /// <summary>
        /// Computes output value of neuronBase
        /// </summary>
        /// 
        /// <param name="input">Input vector</param>
        /// 
        /// <returns>Returns neuronBase's output value</returns>
        /// 
        /// <remarks>The actual neuronBase's output value is determined by inherited class.
        /// The output value is also stored in <see cref="Output"/> property.</remarks>
        /// 
        public abstract double Compute(double[] input);
        private void AllocateWeights(int inputsCount)
        {
            inputsCount = Math.Max(1, inputsCount);
            Weights = new double[inputsCount];
        }
    }
}
