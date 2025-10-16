using System;
using SharpAI.Core.Layers;
using SharpAI.Core.Model;
using SharpAI.Core.Training;

// Build a small network for XOR: 2 -> 8 -> 1 with Sigmoid output
var model = new Sequential()
    .Add(new Dense(2, 8))
    .Add(new Activation(Activation.Kind.Relu, 8))
    .Add(new Dense(8, 1))
    .Add(new Activation(Activation.Kind.Sigmoid, 1));

// XOR dataset
var data = new (double[] x, double[] y)[]
{
    (new double[]{0,0}, new double[]{0}),
    (new double[]{0,1}, new double[]{1}),
    (new double[]{1,0}, new double[]{1}),
    (new double[]{1,1}, new double[]{0}),
};

var rng = new Random(7);
int epochs = 2000;
double lr = 0.1;

for (int epoch = 1; epoch <= epochs; epoch++)
{
    var stats = Trainer.TrainEpoch(model, data, lr, shuffle: true, rng: rng);
    if (epoch % 200 == 0)
    {
        Console.WriteLine($"Epoch {epoch}, loss: {stats.Loss:F6}");
    }
}

Console.WriteLine("--- Predictions after training ---");
foreach (var (x, y) in data)
{
    var pred = model.Forward(x);
    Console.WriteLine($"{x[0]} XOR {x[1]} => pred {pred[0]:F4}, target {y[0]} ");
}
