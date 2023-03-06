using Microsoft.ML;
using Microsoft.ML.Trainers;
using MLNet;
using MySql.Data.MySqlClient;

public class ModelCreator
{
    static string connectionToken = "Data Source=localhost,3306;Initial Catalog=movie_recommender;User ID=tudor;Password=tudor;";
    static MySqlConnection connection = new MySqlConnection(connectionToken);

    //at the begginning of times...create the model that we will use...FOREVER
    public void createModel()
    {
        MLContext mlContext = new MLContext();
        (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
        ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);
        EvaluateModel(mlContext, testDataView, model);
        SaveModel(mlContext, trainingDataView.Schema, model);
    }

    //loads the training and testing data sets
    protected (IDataView training, IDataView test) LoadData(MLContext mlContext)
    {
        var trainDataPath = ".\\bin\\Debug\\net6.0\\recommendation-ratings-train.csv";
        var testDataPath = ".\\bin\\Debug\\net6.0\\recommendation-ratings-test.csv";
        //string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //Console.WriteLine(sCurrentDirectory);
        IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(trainDataPath, hasHeader: true, separatorChar: ',');
        IDataView testDataView = mlContext.Data.LoadFromTextFile<ModelInput>(testDataPath, hasHeader: true, separatorChar: ',');

        return (trainingDataView, testDataView);
    }

    //uses matrix factorization to build a model on the training data
    protected ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
    {
        IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "user_id")
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "movie_id"));
        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = "userIdEncoded",
            MatrixRowIndexColumnName = "movieIdEncoded",
            LabelColumnName = "Label",
            NumberOfIterations = 20,
            ApproximationRank = 100
        };

        var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
        Console.WriteLine("=============== Training the model ===============");
        ITransformer model = trainerEstimator.Fit(trainingDataView);
        return model;
    }

    //evaluates the performance of the current model using the test data set
    protected void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
    {
        Console.WriteLine("=============== Evaluating the model ===============");
        var prediction = model.Transform(testDataView);
        var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
        Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
        Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
    }

    //save the model to a .zip
    protected void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
    {
        var modelPath = "MovieRecommenderModel.zip";

        Console.WriteLine("=============== Saving the model to a file ===============");
        mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
    }
}
