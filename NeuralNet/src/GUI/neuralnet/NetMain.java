package GUI.neuralnet;

import GUI.neuralnet.parser.ParsedData;
import GUI.neuralnet.parser.Parser;

import java.util.Arrays;

public class NetMain {

    public static void main(String[] args) {
        ParsedData data = Parser.parseTrainSetFromFile("trainSet.in");
        String architecture = "38x5x3x5";
        int maxIter = 300_000;
        double learningRate = 0.1;
        double learningRateBias = 0.001;
        double epsilon = 10e-6;
        double momentum = 0.7;

        /*ANN ann = new ANN(architecture, data.getX(), data.getFx(), maxIter, learningRate, learningRateBias, epsilon, momentum, data.getX().size());
        ann.start();

        for (int i = 0; i < data.getX().size(); i++) {
            System.out.println(Arrays.toString(ann.feedForward(data.getX().get(i))));
            System.out.println(Arrays.toString(data.getFx().get(i)));
        }*/

    }
}
