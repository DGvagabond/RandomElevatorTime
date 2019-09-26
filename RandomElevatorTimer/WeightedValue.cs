using System;

namespace RandomElevatorTimer
{
	public abstract class WeightedValue
	{
		public abstract double GetValue();
		public abstract double GetWeight();
	}

	public class WeightedFloatValue : WeightedValue
	{
		private double value;
		private double weight;

		public WeightedFloatValue(double value, double weight = 1)
		{
			this.value = value;
			this.weight = weight;
		}

		public override double GetValue()
		{
			return value;
		}

		public override double GetWeight()
		{
			return weight;
		}
	}

	public class WeightedRangeValue : WeightedValue
	{
		private Random random;
		private double min;
		private double max;
		private double weight;

		public WeightedRangeValue(double min, double max, Random random, double weight = 1)
		{
			this.min = min;
			this.max = max;
			this.weight = weight;
			this.random = random;
		}

		public override double GetValue()
		{
			return random.NextDouble() * (max - min) + min;
		}

		public override double GetWeight()
		{
			return weight;
		}
	}

}
