// -----------------------------------------------------------------------
// <copyright file="ElevatorTimer.cs">
// Copyright (c) DGvagabond. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RandomElevatorTimer
{
	using Exiled.API.Features;
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Linq;
	
	using PlayerEvents = Exiled.Events.Handlers.Player;
	public class ElevatorTimer : Plugin<Config>
	{
		internal static ElevatorTimer Instance { get; } = new ElevatorTimer();

		public override string Author => "DGvagabond";
		public override string Name => "ElevatorTimer";
		public override Version Version { get; } = new Version(1,0,0);
		public override Version RequiredExiledVersion { get; } = new Version(6,0,0);

		private EventHandlers _events;

		public override void OnEnabled()
		{
			random = new Random();
			_events = new EventHandlers();
			
			WeightedRandomizer = new WeightedRandomizer(ParseWeightedValues(), random);
			
			PlayerEvents.InteractingElevator += _events.OnElevatorUse;
			base.OnEnabled();
		}

		public override void OnDisabled()
		{
			PlayerEvents.InteractingElevator += _events.OnElevatorUse;
			_events = null;
			base.OnDisabled();
		}
		public WeightedRandomizer WeightedRandomizer { get; private set; }

		private Random random;

		/// <summary>
		/// This parser is not a pretty boy too look at, but it gets the job done
		/// and only parses 1 time, so it's aight ok?
		///
		/// <para>
		/// Parses the elevator values defined in <see cref="TimerValues"/>
		/// </para>
		/// </summary>
		/// <returns>A collection of all values that can be randomized</returns>
		private IEnumerable<WeightedValue> ParseWeightedValues()
		{
			var matches = Regex.Matches(Instance.Config.TimerValues, @"\((.*?)\)");
			var groups = (from Match match in matches select match.Groups[1].Value).ToList();

			var weightedValues = new List<WeightedValue>();

			foreach (var group in groups)
			{
				var isRange = group.Contains("-");
				var hasWeight = group.Contains(":");

				if (!isRange && !hasWeight)
				{
					weightedValues.Add(new WeightedFloatValue(double.Parse(group)));
					continue;
				}

				var weight = 1.0;

				if (hasWeight)
				{
					var weightCharIndex = group.IndexOf(':');
					var rawWeightValue = group.Substring(weightCharIndex + 1, group.Length - weightCharIndex - 1);
					weight = double.Parse(rawWeightValue);
				}

				var valueSeparator = isRange ? group.IndexOf('-') : group.IndexOf(':');
				var rawMinValue = group.Substring(0, valueSeparator);
				var minValue = double.Parse(rawMinValue);

				if (isRange)
				{
					var endLength = hasWeight ? group.IndexOf(':') : group.Length;
					var rawMaxValue = group.Substring(valueSeparator + 1, endLength - valueSeparator - 1);
					var maxValue = double.Parse(rawMaxValue);

					if (maxValue <= minValue)
					{
						throw new Exception("Maximum value can not be less than minimum value. Please repair your config.");
					}

					weightedValues.Add(new WeightedRangeValue(minValue, maxValue, random, weight));
				}
				else
				{
					weightedValues.Add(new WeightedFloatValue(minValue, weight));
				}
			}

			return weightedValues;
		}
	}
}
