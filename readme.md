# date
start by: 2025-12-16 or earlier

# author
zxlprogram, GMLGY2VCrQ, hsifeng, yccct14, yuuuuu66

# connect us
zhoudaniel02@gmail.com
(or connect other groupmate)

# enviornment
it's Unity project, it must runnable on every type of operating system which Unity supported

# Usage
move your mouse to control the slicing plane
cut the object
move the piece and use the libra to compare it

# description
this is a Unity3D project for the unity lesson's final exam, used the EZ-slice mod and hsifeng builded the basic architecture, zxlprogram find the open-source program to research how does the mod working and build the libra. hsifeng import the knife model, cleaned up folders and yccct14 builded the game rule(pvp mode) and import the players model,also rewrite the splitter.cs to fit the project.

this project are try to make a game, play a character to cut some object, the goal is trying to cut those object in closest weight(we suppose that all object have fixed density, so it have same result on comparing volume)

# principle of slicing
suppose there have a convex 3D object, it is made by a lot of triangle(the triangle builded the surface, and the surface builded the 3D shape),and there's a plane ax+by+cz+d=0,if the plane have an intersection with a triangle means that triangle should cut in half, otherwise it means that part is belong to lower pieces or upper pieces(if the triangle is higher then plane means it's belong to upper pieces and same logic on lower pieces)
suppose the triangle which have line of intersection with plane, and the point of intersection of plane and line is P' and Q', and the vertex of triangle is a,b,c
there must have two vertex lower or upper than line, we make that two point to be a and b, we add triangle(a,b,P') and (b,P',Q') on the upper piece or lower piece and triangle(c,P',Q') for opposite pieces
suddenly we have two piece of mesh, upper and lower mesh, we add two gameObject to add the mesh and remove the original 3D object
that is how did ezslice cut an object to half.

# principle of libra
we have two script and three gameObject:
the script for weighing pan(calcWeight) and balance beam(Libra)
the calcWeight must do the calculate of weight(volume) which collided weighing the pan and save the float weight information
and the Libra have an array to save the weighing pan gameObject, and if you want to compare the weight, you can catch the array's information and compare it.
## how to calculate weight(volume)
Divergence theorem

# resources
.obj file: tank(online resource), human(online resource), cube monster(author: zxlprogram), knife(source: by hsifeng), player*2(imported by yccct14)

# history
2025-12-16
-the project architecture is finished by hsifeng

2025-12-19
-the project put on github repo by zxlprogram

2025-12-21
-added libra by zxlprogram

2025-12-30
-added the knife model by hsifeng
-added the players model*2 and builded the gamerule of pvp mode,also add the UI label by yccct14

# leak
the story,
sound effects,
GameManager.cs should reset the sliced object