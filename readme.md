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
this is a Unity3D project for the unity lesson's final exam, used the EZ-slice mod and hsifeng builded the basic architecture, zxlprogram find the open-source program to research how does the mod working and build the libra.

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
.obj file: tank(online resource), human(online resource), cube monster(author: zxlprogram)

# history
2025-12-16
the project architecture is finished by shifeng

2025-12-19
the project put on github repo by zxlprogram

2025-12-21
added libra by zxlprogram


# HOW TO JOIN REPO?

**第一步：下載 Git**
前往官網下載並安裝 Git
👉 https://git-scm.com/
**第二步：建立 GitHub 帳號**
用瀏覽器搜尋 GitHub
使用 Google 帳號或 Email 註冊 GitHub
記下你的 GitHub 使用者名稱（username）
**第三步：把你的 GitHub 使用者名稱或gmail給我**
**第四步：接受 GitHub 邀請**
收到邀請後，登入 GitHub
到右上角通知（或 Email）
Accept invitation
**第五步：Clone 專案到電腦**
在桌面開啟 cmd
輸入：
git clone https://github.com/zxlprogram/UnityLessonsProject.git
**第六步：用 Unity Hub 開啟專案**
開啟 Unity Hub
點選 Add → Add project from disk
選擇 UnityLessonsProject 資料夾
**第七步：開始編輯專案**
正常在 Unity 裡寫程式、做場景、改素材
**第八步：準備上傳修改**
編輯完成後
開啟 UnityLessonsProject 資料夾
在該資料夾中開啟 cmd
**第九步：第一次使用 Git（只需做一次）**
git config --global user.name "你的GitHub使用者名稱"
git config --global user.email "你註冊GitHub的Email"
**第十步：上傳專案到 GitHub**
git add .
git commit -m "簡述你做了哪些修改"
git push
之後每次修改只需要重複 第十步
