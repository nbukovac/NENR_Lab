package GUI.helpers;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

public class Constants {

    public static final List<Integer> AlphaClassification = new ArrayList<>(Arrays.asList(1, 0, 0, 0, 0));
    public static final List<Integer> BetaClassification = new ArrayList<>(Arrays.asList(0, 1, 0, 0, 0));
    public static final List<Integer> GammaClassification = new ArrayList<>(Arrays.asList(0, 0, 1, 0, 0));
    public static final List<Integer> DeltaClassification = new ArrayList<>(Arrays.asList(0, 0, 0, 1, 0));
    public static final List<Integer> EpsilonClassification = new ArrayList<>(Arrays.asList(0, 0, 0, 0, 1));

    public static final String ALPHA = "Alpha";
    public static final String BETA = "Beta";
    public static final String GAMMA = "Gamma";
    public static final String DELTA = "Delta";
    public static final String EPSILON = "Epsilon";

    public static final ObservableList<String> CLASS_CHOICES = FXCollections.observableArrayList(ALPHA, BETA, GAMMA, DELTA, EPSILON);

    public static final String DRAW = "Draw";
    public static final String CLASSIFY = "Classify";

    public static final ObservableList<String> MODE_CHOICES = FXCollections.observableArrayList(DRAW, CLASSIFY);

    public static final Random RANDOM = new Random();
}
