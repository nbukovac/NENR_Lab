package GUI.helpers;

import GUI.neuralnet.data.TrainingData;
import javafx.geometry.Point2D;

import java.io.*;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class Helper {

    public static List<Point2D> normalizePoints(List<Point2D> points) {
        int numberOfPoints = points.size();
        double xSum = 0.0;
        double ySum = 0.0;

        for (Point2D point : points) {
            xSum += point.getX();
            ySum += point.getY();
        }

        Point2D tc = new Point2D(xSum / numberOfPoints, ySum / numberOfPoints);

        for (int i = 0; i < numberOfPoints; i++) {
            Point2D point = points.get(i);
            points.set(i, point.subtract(tc));
        }

        double maxX = 0.0;
        double maxY = 0.0;

        for (int i = 0; i < numberOfPoints; i++) {
            for (int j = i + 1; j < numberOfPoints; j++) {
                Point2D point1 = points.get(i);
                Point2D point2 = points.get(j);

                double xDistance = Math.abs(point1.getX() - point2.getX());
                double yDistance = Math.abs(point1.getY() - point2.getY());

                if (maxX < xDistance) {
                    maxX = xDistance;
                }
                if (maxY < yDistance) {
                    maxY = yDistance;
                }
            }
        }

        double m = Math.max(maxY, maxX);

        for (int i = 0; i < numberOfPoints; i++) {
            Point2D point =  points.get(i);
            points.set(i, point.multiply(1 / m));
        }

        return points;
    }

    public static List<Point2D> getRepresentativePoints(List<Point2D> points, int numberOfRepresentatives) {
        int numberOfPoints = points.size();
        double totalDistance = 0.0;

        for (int i = 0; i < numberOfPoints - 1; i++) {
            totalDistance += points.get(i).distance(points.get(i + 1));
        }

        List<Point2D> representativePoints = new ArrayList<>(numberOfRepresentatives);
        representativePoints.add(points.get(0));

        double distance = 0.0;
        Iterator<Point2D> pointsIterator = points.iterator();
        Point2D nextPoint = null;

        for (int k = 1; k < numberOfRepresentatives; k++) {
            double thresholdDistance = k * totalDistance / (numberOfRepresentatives - 1);

            while (pointsIterator.hasNext()) {
                nextPoint = pointsIterator.next();

                double distanceGap = nextPoint.distance(representativePoints.get(k - 1));

                if (distanceGap + distance >= thresholdDistance) {
                    distance += distanceGap;
                    representativePoints.add(nextPoint);
                    break;
                }
            }

            if (!pointsIterator.hasNext() && representativePoints.size() < numberOfRepresentatives) {
                representativePoints.add(nextPoint);
            }
        }

        return representativePoints;
    }

    public static void writeGesturesToFile(String filePath, List<List<Point2D>> points, List<List<Integer>> classification) {
        try (BufferedWriter writer = new BufferedWriter(new FileWriter(new File(filePath), true))) {
            for(int i = 0; i < points.size(); i++) {
                StringBuilder sb = new StringBuilder();
                List<Point2D> set = points.get(i);

                for (int j = 0; j < set.size(); j++) {
                    sb.append(set.get(j).getX() + " " + set.get(j).getY() + "#");
                }

                List<Integer> classified = classification.get(i);

                for (int j = 0; j < classified.size(); j++) {
                    sb.append(classified.get(j)).append(" ");
                }

                sb.append(System.lineSeparator());
                writer.write(sb.toString());
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static String getGestureClass(double[] predict) {
        double max = 0.0;
        int index = 0;

        for (int i = 0; i < predict.length; i++) {
            if (predict[i] > max) {
                max = predict[i];
                index = i;
            }
        }

        return Constants.CLASS_CHOICES.get(index);
    }
}
