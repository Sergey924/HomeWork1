using System;

namespace SharpAI.Core.Layers;

public class Dense : ILayer
{
    private readonly int inputSize;
    private readonly int outputSize;

    private readonly double[,] weights; // [output, input]
    private readonly double[] biases;   // [output]

    private double[,] gradWeights; // accumulated gradient
    private double[] gradBiases;   // accumulated gradient

    private double[] lastInput = Array.Empty<double>();

    private static readonly Random rng = new Random(42);

    public Dense(int inputSize, int outputSize)
    {
        if (inputSize <= 0 || outputSize <= 0) throw new ArgumentException("Invalid layer sizes");
        this.inputSize = inputSize;
        this.outputSize = outputSize;

        weights = new double[outputSize, inputSize];
        biases = new double[outputSize];
        gradWeights = new double[outputSize, inputSize];
        gradBiases = new double[outputSize];

        // Xavier/Glorot uniform initialization
        var limit = Math.Sqrt(6.0 / (inputSize + outputSize));
        for (int o = 0; o < outputSize; o++)
        {
            for (int i = 0; i < inputSize; i++)
            {
                weights[o, i] = (rng.NextDouble() * 2 - 1) * limit;
            }
            biases[o] = 0.0;
        }
    }

    public int InputSize => inputSize;
    public int OutputSize => outputSize;

    public void ZeroGrad()
    {
        Array.Clear(gradBiases, 0, gradBiases.Length);
        Array.Clear(gradWeights, 0, gradWeights.Length);
    }

    public double[] Forward(double[] input)
    {
        if (input.Length != inputSize)
            throw new ArgumentException($"Expected input size {inputSize}, got {input.Length}");
        lastInput = (double[])input.Clone();
        var output = new double[outputSize];
        for (int o = 0; o < outputSize; o++)
        {
            double sum = biases[o];
            for (int i = 0; i < inputSize; i++)
            {
                sum += weights[o, i] * input[i];
            }
            output[o] = sum;
        }
        return output;
    }

    public double[] Backward(double[] gradOutput, double learningRate)
    {
        if (gradOutput.Length != outputSize)
            throw new ArgumentException($"Expected grad size {outputSize}");

        var gradInput = new double[inputSize];

        // dL/dW = gradOutput[o] * lastInput[i]
        // dL/db = gradOutput[o]
        // dL/dx = sum_o gradOutput[o] * W[o,i]
        for (int o = 0; o < outputSize; o++)
        {
            double go = gradOutput[o];
            gradBiases[o] += go;
            for (int i = 0; i < inputSize; i++)
            {
                gradWeights[o, i] += go * lastInput[i];
                gradInput[i] += go * weights[o, i];
            }
        }

        // SGD step
        for (int o = 0; o < outputSize; o++)
        {
            biases[o] -= learningRate * gradBiases[o];
            for (int i = 0; i < inputSize; i++)
            {
                weights[o, i] -= learningRate * gradWeights[o, i];
            }
        }

        // zero grads after update
        ZeroGrad();
        return gradInput;
    }
}