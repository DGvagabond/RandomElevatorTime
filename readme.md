# Random Elevator Timer
but why.. (by stepper)

Have you always wanted the ability to change the elevator times? Well, that's possible! Natively. 
Just use the config entry that SMod provides you. (`lift_move_duration: 5`)

Oh, but what if you wanted to completely randomise the elevator time? 
At one moment you could be in there for a normal duration, but on the rare occassion it's suddenly 15 seconds?! 
That could be a life changing situation. Well it's here. And it's probably nothing more than annoying.

## Config

This plugin exposes 4 config entries:

`ret_elevator_values: (4-6:60)(6-10:5)(10-15:1)`  
_These are the values that can be chosen by the randomiser once the elevator is activated._   
The format is pretty simple:  
`(minTime-maxTime:weight)` 
Don't want a range, but instead just a single random number? use `(5:1)` `(time:weight)`  
Don't want a weight? Just leave out `:weight`. Like so: `(2-10)` This will use a default weight of 1. Saves some time, or something.  
To "turn off" the plugin Just set this to `(5)`. And it will always be 5, like default.

`ret_broadcast_message: This elevator will take $seconds$`  
_The message that will be broadcast when the elevator is activated. This will be broadcast to everyone near the elevator_  
There is a `$seconds$` variable that will be replaced in the string with the seconds the elevator will take.

`ret_max_broadcast_time: 2`  
_The time in seconds the broadcast message will stay_  
The broadcast time is always clamped to the elevator time, so say the elevator arrives in 3, but this is defined as 5, the broadcast message will be 3.

`ret_min_broadcast_value: 7`  
_The minimum value that is needed before the broadcast is made._  
Set to 0 to turn broadcasting off completely.

Have fun.
