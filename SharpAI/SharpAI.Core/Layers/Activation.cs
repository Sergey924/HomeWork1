using System;

namespace SharpAI.Core.Layers;

public class Activation : ILayer
{
    public enum Kind { Relu, Sigmoid, Tanh, Linear }

    private readonly Kind kind;
    private readonly int size;
    private double[] lastInput = Array.Empty<double>();

    public Activation(Kind kind, int size)
    {
        if (size <= 0) throw new ArgumentException("Invalid size");
        this.kind = kind;
        this.size = size;
    }

    public int InputSize => size;
    public int OutputSize => size;

    public void ZeroGrad() { /* stateless */ }

    public double[] Forward(double[] input)
    {
        if (input.Length != size) throw new ArgumentException("Bad input size");
        lastInput = (double[])input.Clone();
        var output = new double[size];
        for (int i = 0; i < size; i++)
        {
            output[i] = kind switch
            {
                Kind.Relu => Math.Max(0, input[i]),
                Kind.Sigmoid => 1.0 / (1.0 + Math.Exp(-input[i])),
                Kind.Tanh => Math.Tanh(input[i]),
                _ => input[i],
            };
        }
        return output;
    }

    public double[] Backward(double[] gradOutput, double learningRate)
    {
        if (gradOutput.Length != size) throw new ArgumentException("Bad grad size");
        var gradInput = new double[size];
        for (int i = 0; i < size; i++)
        {
            double derivative;
            switch (kind)
            {
                case Kind.Relu:
                    derivative = lastInput[i] > 0 ? 1.0 : 0.0;
                    break;
                case Kind.Sigmoid:
                    {
                        double s = 1.0 / (1.0 + Math.Exp(-lastInput[i]));
                        derivative = s * (1.0 - s);
                        break;
                    }
                case Kind.Tanh:
                    {
                        double t = Math.Tanh(lastInput[i]);
                        derivative = 1.0 - t * t;
                        break;
                    }
                default:
                    derivative = 1.0;
                    break;
            }
            gradInput[i] = gradOutput[i] * derivative;
        }
        return gradInput;
    }
}