package GUI.neuralnet.parser;

import javafx.geometry.Point2D;

import java.util.ArrayList;
import java.util.List;

public class ParsedData {

    private List<Point2D[]> points;
    private List<double[]> classified;

    public ParsedData() {
        points = new ArrayList<>();
        classified= new ArrayList<>();
    }

    public void add(double[] x, double[] y, double[] fx) {
        Point2D[] gesturePoints = new Point2D[x.length];

        for (int i = 0; i< x.length; i++) {
            Point2D point = new Point2D(x[i], y[i]);
            gesturePoints[i] = point;
        }

        this.points.add(gesturePoints);
        this.classified.add(fx);
    }

    public int size() {
        return classified.size();
    }

    public List<double[]> getClassified() {
        return classified;
    }

    public List<Point2D[]> getPoints() {
        return points;
    }
}
