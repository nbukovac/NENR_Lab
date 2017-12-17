package GUI.neuralnet.parser;

import java.io.*;

public class Parser {

    public static ParsedData parseTrainSetFromFile(String filePath) {
        ParsedData data = new ParsedData();

        try (BufferedReader reader = new BufferedReader(new FileReader(new File(filePath)))) {
            String line;

            while ((line = reader.readLine()) !=  null) {
                String[] split = line.split("#");
                double[] x = new double[split.length - 1];
                double[] y = new double[split.length - 1];

                for (int i = 0; i < split.length - 1; i++) {
                    String[] point = split[i].split(" ");
                    x[i] = Double.parseDouble(point[0]);
                    y[i] = Double.parseDouble(point[1]);
                }

                String[] classSplit = split[split.length - 1].split(" ");
                double[] fx = new double[classSplit.length];

                for (int i = 0; i < classSplit.length; i++) {
                    fx[i] = Integer.parseInt(classSplit[i]);
                }

                data.add(x, y, fx);
            }

        } catch (IOException e) {
            e.printStackTrace();
        }

        return data;
    }
}
