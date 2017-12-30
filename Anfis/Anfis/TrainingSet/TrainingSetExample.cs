namespace Anfis.TrainingSet
{
    public class TrainingSetExample
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double F { get; set; }

        public override string ToString()
        {
            return X + "\t" + Y + "\t" + F;
        }
    }
}