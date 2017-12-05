namespace GeneticAlgorithm.Operators.Mutation
{
    public abstract class Mutation<T> : IMutation<T>
    {
        protected readonly double _mutationProbability;

        public Mutation(double mutationProbability)
        {
            _mutationProbability = mutationProbability;
        }
        
        public abstract T Mutate(T chromosome);
    }
}