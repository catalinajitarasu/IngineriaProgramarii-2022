using Microsoft.ML;
using MySql.Data.MySqlClient;
using System.Data;

public class ModelRetrainer : ModelCreator
{
    private ThreadSemaphore threadSemaphore;
    private MySqlConnection connection;

    public ModelRetrainer(ThreadSemaphore threadSemaphore, MySqlConnection connection)
    {
        this.threadSemaphore = threadSemaphore;
        this.connection = connection;
    }

    //runs as a thread and checks wherever the model needs retraining
    //coordinates with GetTopPredictions(...) if the model can be updated in the zip file
    public void RetrainModel()
    {
        MLContext mlContext = new MLContext();
        ITransformer model;

        while (true)
        {
            
            if (VerifyUpdate())
            {
                (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
                model = BuildAndTrainModel(mlContext, trainingDataView);
                EvaluateModel(mlContext, testDataView, model);

                //the model is being loaded from the zip, so we wait
                while (this.threadSemaphore.isBeingLoaded) ;
                this.threadSemaphore.isBeingUpdated = true;
                SaveModel(mlContext, trainingDataView.Schema, model);
                this.threadSemaphore.isBeingUpdated = false;
            }
        }
    }

    //returns true if there has been added a review for a movie from a new user
    public bool VerifyUpdate()
    {
        bool hasNewUser = false;

        StreamReader streamReader = new StreamReader(".\\bin\\Debug\\net6.0\\last-update.txt");
        var lastUpdate = streamReader.ReadLine();
        streamReader.Close();

        try
        {
            connection.Open();

            var selectCmd = "SELECT user_id, movie_id, review_value, id FROM review WHERE id > " + lastUpdate + " ORDER BY id";
            var command = new MySqlCommand(selectCmd, connection);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                StreamWriter writer = new StreamWriter("recommendation-ratings-train.csv", append: true);

                while (reader.Read())
                {
                    writer.WriteLine(reader.GetFloat(2) + "," + reader.GetInt32(1) + "," + reader.GetInt32(0));
                    lastUpdate = reader.GetInt32(3).ToString();
                    hasNewUser = true;
                }
                writer.Close();

                StreamWriter streamWriter = new StreamWriter(".\\bin\\Debug\\net6.0\\last-update.txt");
                streamWriter.WriteLine(lastUpdate);
                streamWriter.Close();
            }

            reader.Close();
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return hasNewUser;
    }
}