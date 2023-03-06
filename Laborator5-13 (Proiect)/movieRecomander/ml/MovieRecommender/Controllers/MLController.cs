using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace MovieRecommender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MLController : ControllerBase
    {
        readonly ILogger<MLController> _logger;

        //static string connectionToken = "Data Source=localhost,3306;Initial Catalog=movie_recommender;User ID=tudor;Password=tudor;";
        static string connectionToken = "Data Source=157.230.114.95,3306;Initial Catalog=movie_recommender;User ID=admin;Password=password;";
        static MySqlConnection connection1 = new MySqlConnection(connectionToken);
        static MySqlConnection connection2 = new MySqlConnection(connectionToken);

        static ModelCreator modelCreator = new ModelCreator();
        static ThreadSemaphore threadSemaphore = new ThreadSemaphore();
        static Prediction prediction = new Prediction(threadSemaphore, connection1);
        static bool started = false;

        public MLController(ILogger<MLController> logger)
        {
            _logger = logger;

            modelCreator.createModel();

            //create the model retrainer thread which will retrain the model when new data appears
            if (!started)
            {
                ModelRetrainer modelRetrainer = new ModelRetrainer(threadSemaphore, connection2);
                Thread modelRetrainerThread = new Thread(modelRetrainer.RetrainModel);
                modelRetrainerThread.Start();
                started = true;
            }
        }

        [HttpGet("{user_id}/{n}")]
        public ActionResult<List<int>> Get(int user_id, int n)
        {
            return Ok(prediction.GetTopPredictions(user_id, n));
        }
    }
}