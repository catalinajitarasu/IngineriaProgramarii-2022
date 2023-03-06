using Microsoft.ML;
using MLNet;
using MSPrediction;
using MySql.Data.MySqlClient;
using System.Data;

public class Prediction
{
    private ThreadSemaphore threadSemaphore;
    private MySqlConnection connection;

    public Prediction(ThreadSemaphore threadSemaphore, MySqlConnection connection)
    {
        this.threadSemaphore = threadSemaphore;
        this.connection = connection;
    }

    //the function used in MLController
    //predicts which n movies could get the highest rating from the user_id
    public List<int> GetTopPredictions(int user_id, int n)
    {
        MLContext mlContext = new MLContext();
        ITransformer model;

        //if the model was retrained, we wait for it to be saved in the zip
        this.threadSemaphore.isBeingLoaded = true;
        while (this.threadSemaphore.isBeingUpdated) ;
        model = mlContext.Model.Load("MovieRecommenderModel.zip", out _);
        this.threadSemaphore.isBeingLoaded = false;

        var sortedPred = UseModelForTopPrediction(mlContext, model, user_id);

        //extract the ids of the first n predictions from the sortedPred
        List<int> topPred = new List<int>();
        topPred = sortedPred.Select(i => i.id).Take(n).ToList();

        return topPred;
    }

    //get all the movies the user didn't rate and get a prediction for them
    //sort the predictions out and give them all
    private List<MovieScore> UseModelForTopPrediction(MLContext mlContext, ITransformer model, int user_id)
    {
        List<MovieScore> scores = new List<MovieScore>();

        try
        {
            connection.Open();

            //var selectCmd = "SELECT DISTINCT m.id FROM movie m JOIN review r ON m.id = r.movie_id WHERE r.user_id != " + user_id;
            //var command = new MySqlCommand(selectCmd, connection);
            //var reader = command.ExecuteReader();

            //while (reader.Read())
            //{
            //    scores.Add(UseModelForSinglePrediction(mlContext, model, user_id, reader.GetInt32(0)));
            //}
            //reader.Close();

            //updating last-movie.txt
            var selectCmd = "SELECT MAX(id) FROM movie";
            var command2 = new MySqlCommand(selectCmd, connection);
            var reader2 = command2.ExecuteReader();
            
            reader2.Read();
            if (reader2.HasRows && !reader2.IsDBNull(0))
            {
                StreamWriter streamWriter = new StreamWriter(".\\bin\\Debug\\net6.0\\last-movie.txt");
                streamWriter.WriteLine(reader2.GetInt32(0).ToString());
                streamWriter.Close();
            }

            reader2.Close();
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        if(scores.Count <= 0)
        {
            StreamReader streamReader = new StreamReader(".\\bin\\Debug\\net6.0\\last-movie.txt");
            var lastMovie = streamReader.ReadLine();
            streamReader.Close();

            Console.WriteLine("--"+lastMovie+"--");
            for(int i = 1; i <= Int32.Parse(lastMovie); i++)
            {
                scores.Add(UseModelForSinglePrediction(mlContext, model, user_id, i));
            }

        }

        scores.Sort((b, a) => a.score.CompareTo(b.score));

        return scores;
    }

    //predict the score the user would give to the movie
    private MovieScore UseModelForSinglePrediction(MLContext mlContext, ITransformer model, int user_id, int movie_id)
    {
        var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
        var testInput = new ModelInput { user_id = user_id, movie_id = movie_id };
        var movieRatingPrediction = predictionEngine.Predict(testInput);
        return new MovieScore { id = movie_id, score = movieRatingPrediction.Score };
    }
}