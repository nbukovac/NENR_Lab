package GUI.neuralnet;

import GUI.helpers.Constants;
import GUI.neuralnet.transferfunctions.SigmoidalTranferFunction;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class ANN {

    private List<Integer> neuronsPerLayer;
    private List<double[][]> weights;
    private List<double[]> outputs;
    private List<double[]> bias;

    private List<double[]> trainSetInput;
    private List<double[]> trainSetOutput;

    private int iterationMaximum;
    private double learningRate;
    private double learningRateBias;
    private double errorLimit;
    private double momentum;
    private int sampleSize;

    public ANN(String layerArchitecture, List<double[]> trainSetInput, List<double[]> trainSetOutput, int iterationMaximum,
               double learningRate, double learningRateBias, double errorLimit, double momentum, int sampleSize) {

        this.iterationMaximum = iterationMaximum;
        this.learningRate = learningRate;
        this.learningRateBias = learningRateBias;
        this.errorLimit = errorLimit;
        this.momentum = momentum;
        this.sampleSize = sampleSize;

        this.trainSetInput = trainSetInput;
        this.trainSetOutput = trainSetOutput;

        String[] split = layerArchitecture.split("x");

        neuronsPerLayer = new ArrayList<>();
        for (String s : split) {
            neuronsPerLayer.add(Integer.parseInt(s));
        }

        weights = new ArrayList<>();

        for (int i = 0; i < neuronsPerLayer.size() - 1; i++) {
            double[][] temp = new double[neuronsPerLayer.get(i)][neuronsPerLayer.get(i + 1)];

            for (int row = 0; row < temp.length; row++) {
                for (int column = 0; column < temp[0].length; column++) {
                    temp[row][column] = Constants.RANDOM.nextDouble();
                }
            }

            weights.add(temp);
        }

        outputs = new ArrayList<>();
        outputs.add(new double[neuronsPerLayer.get(0)]);
        bias = new ArrayList<>();

        for (int i = 1; i <  neuronsPerLayer.size(); i++) {
            outputs.add(new double[neuronsPerLayer.get(i)]);
            bias.add(new double[neuronsPerLayer.get(i)]);
        }
    }

    public void start() {
        for (int iteration = 1; iteration <= iterationMaximum; iteration++) {
            List<List<double[]>> errors = new ArrayList<>();
            List<List<double[]>> yOuts = new ArrayList<>();

            for (int i = 0; i < trainSetInput.size(); i++) {
                double[] x = trainSetInput.get(i);
                double[] y = trainSetOutput.get(i);

                errors.add(backPropagation(x, y));
                yOuts.add(new ArrayList<>(outputs));

                if ((i + 1) % sampleSize == 0) {
                    weightsUpdate(errors, yOuts);
                    errors.clear();
                    yOuts.clear();
                }
            }

            double errorSum = 0.0;

            for (int i = 0; i < trainSetInput.size(); i++) {
                double[] x = trainSetInput.get(i);
                double[] y = trainSetOutput.get(i);
                double[] yPredict = feedForward(x);

                for(int j = 0; j < y.length; j++) {
                    errorSum += (y[j] - yPredict[j]) * (y[j] - yPredict[j]);
                }
            }

            System.out.println("Iteration => " + iteration + "\t\tError => " +errorSum / trainSetInput.size());

            if (errorSum / trainSetInput.size() < errorLimit) {
                break;
            }
        }
    }

    private void weightsUpdate(List<List<double[]>> errors, List<List<double[]>> yOuts) {

        for (int i = 0; i < neuronsPerLayer.size() - 1; i++) {
            double[][] layerWeights = weights.get(i);
            double[] layerBias = bias.get(i);
            double[][] change = new double[layerWeights.length][layerWeights[0].length];
            double[] biasChange = new double[layerBias.length];
            double[][] prevChange = new double[layerWeights.length][layerWeights[0].length];

            for (int l = 0; l < errors.size(); l++) {
                double[] yOut = yOuts.get(l).get(i);
                double[] error = errors.get(l).get(i);

                for (int j = 0; j < layerWeights.length; j++) {
                    for (int k = 0; k < layerWeights[j].length; k++) {
                        change[j][k] += error[k] * yOut[j];
                    }
                }

                for (int j = 0; j < layerBias.length; j++) {
                    biasChange[j] += error[i];
                }
            }

            for (int j = 0; j < layerWeights.length; j++) {
                for (int k = 0; k < layerWeights[j].length; k++) {
                    layerWeights[j][k] += momentum * prevChange[j][k] + (1 - momentum) * learningRate * change[j][k] / errors.size();
                }
            }

            for (int j = 0; j < layerBias.length; j++) {
                layerBias[i] += learningRateBias * biasChange[i] / errors.size();
            }
        }
    }

    private List<double[]> backPropagation(double[] x, double[] y) {
        double[] yOut = feedForward(x);
        double[] error = new double[yOut.length];
        List<double[]> outErrors = new ArrayList<>();

        for (int i = 0; i < error.length; i++) {
            error[i] = yOut[i] * (1 - yOut[i]) * (y[i] - yOut[i]);
        }

        outErrors.add(error);

        for (int i = neuronsPerLayer.size() - 2; i > 0; i--) {
            yOut = outputs.get(i);
            double[] newError = new double[yOut.length];

            for (int j = 0; j < yOut.length; j++) {
                double errorSum = 0.0;

                for (int o = 0; o < error.length; o++) {
                    errorSum += error[o] * weights.get(i)[j][o];
                }

                newError[j] = yOut[j] * (1 - yOut[j]) * errorSum;
            }

            outErrors.add(0, newError);
            error = Arrays.copyOf(newError, newError.length);
        }

        return outErrors;
    }

    private double[] layerOutput(double[] x, int layer) {
        double[] layerOutput;

        if (layer == 0) {
            layerOutput =  Arrays.copyOf(x, x.length);
        } else {
            layerOutput = SigmoidalTranferFunction.calculate(neuronsPerLayer.get(layer), weights.get(layer - 1), x, bias.get(layer - 1));
        }

        outputs.set(layer, layerOutput);

        return layerOutput;
    }

    public double[] feedForward(double[] x) {
        double[] next = Arrays.copyOf(x, x.length);

        for (int i = 0; i < neuronsPerLayer.size(); i++) {
            next = layerOutput(next, i);
        }

        return next;
    }

}
