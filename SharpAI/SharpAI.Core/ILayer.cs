namespace SharpAI.Core;

public interface ILayer
{
    int InputSize { get; }
    int OutputSize { get; }

    // Forward pass: input -> output
    double[] Forward(double[] input);

    // Backward pass: gradient wrt output -> gradient wrt input; updates internal params using learning rate
    double[] Backward(double[] gradOutput, double learningRate);

    // Reset any accumulated gradients/state
    void ZeroGrad();
}