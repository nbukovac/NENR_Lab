package GUI.neuralnet.data;

import javafx.geometry.Point2D;

import java.util.ArrayList;
import java.util.List;

public class TrainingData {

    private List<double[]> input;
    private List<double[]> output;

    public TrainingData() {
        input = new ArrayList<>();
        output = new ArrayList<>();
    }

    public void add(List<Point2D> in, double[] out) {
        double[] insert = new double[in.size() * 2];

        for (int i = 0; i < in.size(); i++) {
            Point2D point = in.get(i);
            insert[i * 2] = point.getX();
            insert[i * 2 + 1] = point.getY();
        }

        input.add(insert);
        output.add(out);
    }

    public List<double[]> getInput() {
        return input;
    }

    public List<double[]> getOutput() {
        return output;
    }
}
