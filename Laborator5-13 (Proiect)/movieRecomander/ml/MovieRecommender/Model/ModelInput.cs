using Microsoft.ML.Data;

namespace MLNet
{
    public class ModelInput
    {
        [LoadColumn(0)]
        public int user_id { set; get; }

        [LoadColumn(1)]
        public int movie_id { set; get; }

        [LoadColumn(2), ColumnName("Label")]
        public float review_value { set; get; }
    }
}