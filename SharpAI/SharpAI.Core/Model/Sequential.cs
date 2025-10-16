using System;
using SharpAI.Core.Layers;

namespace SharpAI.Core.Model;

public class Sequential
{
    private readonly System.Collections.Generic.List<ILayer> layers = new();

    public Sequential Add(ILayer layer)
    {
        if (layers.Count > 0)
        {
            var prev = layers[^1];
            if (prev.OutputSize != layer.InputSize)
            {
                throw new InvalidOperationException($"Layer size mismatch: prev {prev.OutputSize} -> next {layer.InputSize}");
            }
        }
        layers.Add(layer);
        return this;
    }

    public int InputSize => layers.Count == 0 ? 0 : layers[0].InputSize;
    public int OutputSize => layers.Count == 0 ? 0 : layers[^1].OutputSize;

    public double[] Forward(double[] input)
    {
        var x = input;
        foreach (var layer in layers)
        {
            x = layer.Forward(x);
        }
        return x;
    }

    public void ZeroGrad()
    {
        foreach (var layer in layers) layer.ZeroGrad();
    }

    public double[] Backward(double[] gradOutput, double learningRate)
    {
        var g = gradOutput;
        for (int i = layers.Count - 1; i >= 0; i--)
        {
            g = layers[i].Backward(g, learningRate);
        }
        return g;
    }
}