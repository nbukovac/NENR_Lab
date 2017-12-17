package GUI.neuralnet.transferfunctions;


import org.ejml.simple.SimpleMatrix;

public class SigmoidalTranferFunction {

    public static double[] calculate(int outputSize, double[][] weights, double[] x, double[] bias) {
        int wRows = weights.length;
        int wColumns = weights[0].length;

        SimpleMatrix xMatrix = new SimpleMatrix(x.length, 1);

        for (int i = 0; i < x.length; i++) {
            xMatrix.set(i, 0, x[i]);
        }

        SimpleMatrix wMatrix = new SimpleMatrix(wRows, wColumns);

        for (int i = 0; i < wRows; i++) {
            for (int j = 0; j < wColumns; j++) {
                wMatrix.set(i, j, weights[i][j]);
            }
        }

        SimpleMatrix wT = wMatrix.transpose();
        SimpleMatrix wTx = wT.mult(xMatrix);

        double[] output = new double[outputSize];

        for (int i = 0; i < wTx.numRows(); i++) {
            double temp = wTx.get(i, 0);

            output[i] = 1. / (1 + Math.exp(-temp - bias[i]));
        }

        return output;
    }
}
