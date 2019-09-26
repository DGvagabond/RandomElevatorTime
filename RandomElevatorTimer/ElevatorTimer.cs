using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.EventHandlers;
using Smod2.Events;

namespace RandomElevatorTimer
{
	[PluginDetails(
		author = "stepper",
		name = "Random Elevator Time",
		description = "Allows you to set a random time for the elevator",
		id = "stepper.randomElevator",
		version = "1.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
		)]
	public class ElevatorTimer : Plugin
	{
		public WeightedRandomizer WeightedRandomizer { get; private set; }

		private Random random;

		public uint BroadCastTime { get; private set; }
		public double BroadcastValue { get; private set; }
		public string TimerValues { get; private set; }
		public string BroadcastMessage { get; private set; }

		public override void Register()
		{
			// Register single event with priority (need to specify the handler type)
			this.AddEventHandler(typeof(IEventHandlerElevatorUse), new ElevatorUseEvent(this), Priority.High);

			this.AddConfig(new ConfigSetting("ret_timer_values", "(4-6:60)(6-10:5)(10-15:1)", false, true,
				"The values that the elevator time can be set at."));
			this.AddConfig(new ConfigSetting("ret_min_broadcast_value", 7, false, true,
				"Minimum amount of seconds before broadcasting the time to all players near the elevator."));
			this.AddConfig(new ConfigSetting("ret_broadcast_message", "This elevator will take $seconds$ seconds to arrive.", false, true,
				"The message that will be broadcasted to all users. Replacement string for seconds is \"$seconds$\""));
			this.AddConfig(new ConfigSetting("ret_max_broadcast_time", 2, false, true,
				"The time the message will be broadcasted for."));
		}

		public override void OnEnable()
		{
			random = new Random();

			BroadCastTime = (uint)GetConfigInt("ret_max_broadcast_time");
			BroadcastValue = GetConfigFloat("ret_min_broadcast_value");
			TimerValues = GetConfigString("ret_timer_values");
			BroadcastMessage = GetConfigString("ret_broadcast_message");
			WeightedRandomizer = new WeightedRandomizer(ParseWeightedValues(), random);
		}

		public override void OnDisable()
		{

		}

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
			MatchCollection matches = Regex.Matches(TimerValues, @"\((.*?)\)");
			List<string> groups = (from Match match in matches select match.Groups[1].Value).ToList();

			List<WeightedValue> weightedValues = new List<WeightedValue>();

			foreach (string group in groups)
			{
				bool isRange = group.Contains("-"); // if false, min-value will be used as value.
				bool hasWeight = group.Contains(":"); // if false, default weight of 1 will be use

				// if the string does not contain a - or : it means it's a single number with a weight of 1
				if (!isRange && !hasWeight)
				{
					weightedValues.Add(new WeightedFloatValue(double.Parse(group)));
					continue;
				}

				double weight = 1.0;

				// find the weight first
				if (hasWeight)
				{
					int weightCharIndex = group.IndexOf(':');
					string rawWeightValue = group.Substring(weightCharIndex + 1, group.Length - weightCharIndex - 1);
					weight = double.Parse(rawWeightValue);
				}

				int valueSeparator = isRange ? group.IndexOf('-') : group.IndexOf(':');
				string rawMinValue = group.Substring(0, valueSeparator);
				double minValue = double.Parse(rawMinValue);

				if (isRange)
				{
					int endLength = hasWeight ? group.IndexOf(':') : group.Length;
					string rawMaxValue = group.Substring(valueSeparator + 1, endLength - valueSeparator - 1);
					double maxValue = double.Parse(rawMaxValue);

					if (maxValue <= minValue)
					{
						throw new Exception("Max value can not be less than min value. Please fix in config settings.");
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
