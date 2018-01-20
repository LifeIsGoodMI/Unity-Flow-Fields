# Unity-Flow-Fields
The implementation is pretty basic, it needs to be a little adjusted to be used in conjunction with groups having different goals.
That's a trivial task so I've left it out to keep the implementation lighter. However, you can spawn multiple actors with a common goal by simply using the provided 'Group' class.

See the implementation in action (including some visuals for debugging which I've left out in this implementation): 
https://www.youtube.com/watch?v=jLjaN6ps3BA


**Instructions**
1) Set up a prefab for an Actor, throw the 'Actor' component on top of it.
2) Add the 'FlowFieldsMaster' component to a Game Object in your scene. Assign the actor prefab.
	*You're good to go!*
	
Note: No visuals included, you might want to visualize the grid for example. (e.g. using Unity's low level Graphics library)
