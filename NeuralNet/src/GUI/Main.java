package GUI;

import GUI.helpers.Constants;
import GUI.helpers.Helper;
import GUI.neuralnet.ANN;
import GUI.neuralnet.data.TrainingData;
import GUI.neuralnet.parser.ParsedData;
import GUI.neuralnet.parser.Parser;
import javafx.application.Application;
import javafx.event.EventHandler;
import javafx.geometry.Point2D;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.control.*;
import javafx.scene.layout.HBox;
import javafx.scene.layout.Pane;
import javafx.scene.paint.Paint;
import javafx.stage.Stage;
import javafx.scene.input.MouseEvent;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Main extends Application {

    private static final int WIDTH = 860;
    private static final int HEIGHT = 680;
    private static final String TRAINING_SET = "trainSet.in";
    private static final int M = 50;
    private static final String ARCHITECTURE = "100x5x4x5";
    private static final double LEARNING_RATE = 0.15;
    private static final int ITERATION_LIMIT = 150_000;
    private static final int BATCH_SIZE = 20;

    private static int m = M;
    private static String architecture = ARCHITECTURE;
    private static double learningRate = LEARNING_RATE;
    private static int iterationLimit = ITERATION_LIMIT;
    private static int batchSize = BATCH_SIZE;

    private static Canvas canvas = new Canvas(WIDTH, HEIGHT);
    private List<Point2D> createdPoints;
    private static List<List<Point2D>> pointsStorage = new ArrayList<>();
    private static List<List<Integer>> classificationStorage = new ArrayList<>();

    private static TextField mTextField;
    private static TextField archTextField;
    private static TextField rateTextField;
    private static TextField iterationTextField;
    private static TextField batchSizeTextField;

    private static ANN ann;

    private static boolean annLearned = false;

    @Override
    public void start(Stage primaryStage) throws Exception{
        canvas.setStyle("-fx-background-color: white;");

        ComboBox<String> classChoice = new ComboBox<>(Constants.CLASS_CHOICES);
        classChoice.getSelectionModel().select(0);

        ComboBox<String> modeChoice = new ComboBox<>(Constants.MODE_CHOICES);
        classChoice.getSelectionModel().select(0);

        mTextField = new TextField(M + "");
        mTextField.setMaxWidth(50);
        archTextField = new TextField(ARCHITECTURE);
        archTextField.setMaxWidth(100);
        rateTextField = new TextField(LEARNING_RATE + "");
        rateTextField.setMaxWidth(50);
        iterationTextField = new TextField(ITERATION_LIMIT + "");
        iterationTextField.setMaxWidth(80);
        batchSizeTextField = new TextField(BATCH_SIZE + "");
        batchSizeTextField.setMaxWidth(50);


        Button button = new Button("Start");

        HBox hbox = new HBox(modeChoice, classChoice, new Label("M"), mTextField, new Label("Architecture"),
                archTextField, new Label("eta"), rateTextField, new Label("iteration"), iterationTextField,
                new Label("Batch size"), batchSizeTextField, button);

        Pane pane = new Pane(canvas, hbox);
        pane.setPrefSize(WIDTH, HEIGHT);
        pane.setStyle("-fx-background-color: white;");

        EventHandler<MouseEvent> mouseDragStart = new EventHandler<MouseEvent>() {
            @Override
            public void handle(MouseEvent mouseEvent) {
                createdPoints = new ArrayList<>();
                createdPoints.add(new Point2D(mouseEvent.getX(), mouseEvent.getY()));

                GraphicsContext gc = canvas.getGraphicsContext2D();
                gc.closePath();
                gc.clearRect(0, 0, canvas.getWidth(), canvas.getHeight());

                gc.setLineWidth(1.0);
                gc.moveTo(mouseEvent.getX(), mouseEvent.getY());
                gc.beginPath();
                gc.setStroke(Paint.valueOf("Red"));
            }
        };

        EventHandler<MouseEvent> mouseDragMotion = new EventHandler<MouseEvent>() {
            @Override
            public void handle(MouseEvent e) {
                createdPoints.add(new Point2D(e.getX(), e.getY()));

                GraphicsContext gc = canvas.getGraphicsContext2D();

                gc.lineTo(e.getX(), e.getY());
                gc.stroke();
            }
        };

        EventHandler<MouseEvent> mouseDragRelease = new EventHandler<MouseEvent>() {
            @Override
            public void handle(MouseEvent mouseEvent) {
                createdPoints = Helper.normalizePoints(createdPoints);
                pointsStorage.add(new ArrayList<Point2D>(createdPoints));

                switch (classChoice.getSelectionModel().getSelectedIndex()) {
                    case 0: classificationStorage.add(Constants.AlphaClassification);
                            break;
                    case 1: classificationStorage.add(Constants.BetaClassification);
                            break;
                    case 2: classificationStorage.add(Constants.GammaClassification);
                            break;
                    case 3: classificationStorage.add(Constants.DeltaClassification);
                            break;
                    case 4: classificationStorage.add(Constants.EpsilonClassification);
                            break;
                }

                createdPoints.clear();
            }
        };

        EventHandler<MouseEvent> mouseClick = new EventHandler<MouseEvent>() {
            @Override
            public void handle(MouseEvent e) {
                int selection = modeChoice.getSelectionModel().getSelectedIndex();

                if (pointsStorage.size() != 0 && selection == 0) {
                    Helper.writeGesturesToFile(TRAINING_SET, pointsStorage, classificationStorage);
                    pointsStorage.clear();
                    classificationStorage.clear();
                } else if (pointsStorage.size() != 0 && selection == 1) {
                    if (updateParameters() || !annLearned) {
                        ParsedData data = Parser.parseTrainSetFromFile(TRAINING_SET);
                        TrainingData trainData = new TrainingData();

                        for (int i = 0; i < data.size(); i++) {
                            trainData.add(Helper.getRepresentativePoints(Arrays.asList(data.getPoints().get(i)), m), data.getClassified().get(i));
                        }

                        ann = new ANN(architecture, trainData.getInput(), trainData.getOutput(),
                                iterationLimit, learningRate, 0.001, 10e-6, 0.7, batchSize);

                        ann.start();
                        annLearned = true;
                    }

                    List<Point2D> points = pointsStorage.get(pointsStorage.size() - 1);
                    points = Helper.normalizePoints(points);
                    points = Helper.getRepresentativePoints(points, m);

                    TrainingData testData = new TrainingData();
                    testData.add(points, null);

                    double[] predict = ann.feedForward(testData.getInput().get(0));
                    System.out.println(Arrays.toString(predict));

                    Alert alert = new Alert(Alert.AlertType.NONE, Helper.getGestureClass(predict), ButtonType.CLOSE);
                    alert.showAndWait();

                    pointsStorage.clear();
                    classificationStorage.clear();

                }
            }
        };


        canvas.setOnMousePressed(mouseDragStart);
        canvas.setOnMouseDragged(mouseDragMotion);
        canvas.setOnMouseReleased(mouseDragRelease);

        button.setOnMouseClicked(mouseClick);

        primaryStage.setTitle("Neural Net");
        primaryStage.setScene(new Scene(pane, WIDTH, HEIGHT));
        primaryStage.show();
    }


    public static void main(String[] args) {
        launch(args);
    }

    private boolean updateParameters() {
        boolean update = false;

        if (!archTextField.getText().equals(architecture)) {
            architecture = archTextField.getText();
            update = true;
        }
        if (!batchSizeTextField.getText().equals(batchSize + "")) {
            batchSize = Integer.parseInt(batchSizeTextField.getText().trim());
            update = true;
        }
        if (!mTextField.getText().equals(m + "")) {
            m = Integer.parseInt(mTextField.getText().trim());
            update = true;
        }
        if (!rateTextField.getText().equals(learningRate + "")) {
            learningRate = Double.parseDouble(rateTextField.getText().trim());
            update = true;
        }
        if (!iterationTextField.getText().equals(iterationLimit + "")) {
            iterationLimit = Integer.parseInt(iterationTextField.getText().trim());
            update = true;
        }

        return update;
    }
}
