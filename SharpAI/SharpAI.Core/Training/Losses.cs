using System;

namespace SharpAI.Core.Training;

public static class Losses
{
    // Mean Squared Error
    public static (double loss, double[] grad) MeanSquaredError(double[] prediction, double[] target)
    {
        if (prediction.Length != target.Length)
            throw new ArgumentException("Prediction/target size mismatch");
        int n = prediction.Length;
        double loss = 0.0;
        var grad = new double[n];
        for (int i = 0; i < n; i++)
        {
            double diff = prediction[i] - target[i];
            loss += diff * diff;
            grad[i] = 2.0 * diff / n; // d/dy (1/n * (y - t)^2) = 2*(y-t)/n
        }
        loss /= n;
        return (loss, grad);
    }
}