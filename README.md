# MiRIAD - Mixed Reality Interface Authoring Tool for Digital Twins
A repo to hold this project while it's too big to have no version control, but too unofficial to be anywhere more official. 

This is an XR app (we're using passthrough!) for making visualizations of MTConnect data.
This repository does not contain the entire project-- it's too big. 
Final version at the end of the summer: https://drive.google.com/drive/folders/1Til8H48EQt2lEdHqATdGbED6QaJ4ctNs?usp=sharing

This project uses Unity verison 2020.3.34f1, and the Oculus integration. It's designed to work with the Meta Quest 2, 
and should be easy to modify to work with the Quest Pro. 
Modifying it to work with other HMDs (in particular, the XR Elite would be a good choice) is excellent future work. 

Dev is the relevant scene. 

In it's current state, this project can read pre-recorded MTConnect formatted data from a specified file, and for floats 
(and only floats) allows users to place a few visualizations of those floats in 3D space. Some logical next steps for this project are: 

- Displaying real-time data. This is sort of big-- check Server Talker to get some ideas.
- Displaying interactions between multiple variables (graphs, etc)
- Displaying non-float variables
- bi-directional interactivity-- make actions taken in VR have an effect in physical reality
-   ROS# might be useful for this
- Adaptation to other HMDs
- Addition of more formats than MTConnect
- Representing data from other DTs, rather than just machine data
