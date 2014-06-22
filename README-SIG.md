DuoDrive is a racing game where you control the car with two players, one
for the throttle and the other for steering.
This game is made in Unity, the free version.

Limitations of Unity
--------------------
There are a few limitations found in Unity. One of them is code coverage.
If we wish to have code coverage in Unity, we need to purchase a rather
expensive tool. Because this is a project that lasts only three months and
it is for our study, we did not buy such a tool.

Functions with lots of parameters
---------------------------------
After our previous feedback from SIG we tried to improve the parameter 
count of some of our functions. Sadly enough, there are some functions for
which we were not able to decrease the amount of parameters.
These functions are mostly the RPC functions. It is not possible to pass
Objects as parameters with RPC, only some primitive values, Vectors and
Quaternions. So with RPC we have to pass all data of an object to the other
players with only these basic data types. That is why these functions
typically have quite a lot of parameters.
