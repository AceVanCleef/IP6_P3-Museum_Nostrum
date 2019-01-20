# Dimension of Gameworld and Gameobjects

## Unity unit

** 1 Unity unit equals 1 meter.**

## Player avatar specification

** Body height:** 1.75m
This is simulated by childing the main camera to the "Player" GameObject and then setting the camera's transform.position.y to 1.75 units. To make the player taller/shorter, change this value.
Make sure you do not change the transform.position.y value of the "Player" GameObject.