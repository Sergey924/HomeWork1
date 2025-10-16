using System;
using SharpAI.Core.Model;
using SharpAI.Core.Training;

namespace SharpAI.Core.Training;

public class Trainer
{
    public record TrainingStats(int Epoch, double Loss);

    public static TrainingStats TrainEpoch(
        Sequential model,
        (double[] x, double[] y)[] dataset,
        double learningRate,
        bool shuffle,
        Random? rng = null)
    {
        rng ??= new Random(123);
        double totalLoss = 0.0;

        // Optional shuffle
        if (shuffle)
        {
            for (int i = dataset.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = dataset[i];
                dataset[i] = dataset[j];
                dataset[j] = tmp;
            }
        }

        foreach (var (x, y) in dataset)
        {
            var pred = model.Forward(x);
            var (loss, grad) = Losses.MeanSquaredError(pred, y);
            totalLoss += loss;

            model.Backward(grad, learningRate);
        }

        return new TrainingStats(0, totalLoss / dataset.Length);
    }
}