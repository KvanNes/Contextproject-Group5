DuoDrive is a racing game where you control the car with two players, 
one for the throttle and the other for steering.
This game is made in Unity, the free version.

Limitations of Unity
There are a few limitations found in Unity. One of them is code coverage.
If we wish to have code coverage in Unity, we need to purchase a very
expensive tool. Because this is a project that lasts only 3 months and it
is for our study, and because such a tool costs a lot, we were unable to 
buy such a tool.

Functions with lots of parameters
After our previous feedback from SIG we tried to improve the parameter 
count of some of our functions. Sadly enough, there are some functions for which
we were not able to decrease the amount of parameters.
These functions are mostly the RPC functions. It is not possible to pass Objects 
(only Vectors and Quaternions) as parameters in RPC. So you must pass all of the data 
of an object to the other players with only basic datastructures. That is why these functions
have lots of parameters. 