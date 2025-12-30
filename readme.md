# Unity3D Slicing & Libra Game Project

## Date

Start by **2025-12-16** (or earlier)

## Author

* zxlprogram
* GMLGY2VCrQ
* hsifeng
* yccct14
* yuuuuu66

## Contact

* Email: **[zhoudaniel02@gmail.com](mailto:zhoudaniel02@gmail.com)**
  (or contact any other groupmate)

## Environment

* Unity3D project
* Must be runnable on **all operating systems supported by Unity**

## Usage

* Move the mouse to control the slicing plane
* Slice the object
* Move the sliced pieces onto the libra (balance scale) to compare their weights

## Description

This is a **Unity3D project for the Unity course final exam**.

The project uses the **EZ-Slice** mod to perform real-time mesh slicing.

### Team Contributions

* **hsifeng**: Built the basic project architecture, imported the knife model, and cleaned up project folders
* **zxlprogram**: Researched open-source slicing implementations, studied how EZ-Slice works, and implemented the libra (balance scale)
* **yccct14**: Built the PvP game rules, imported two player models, added UI labels, and rewrote `Splitter.cs` to fit the project
* Other members participated in integration and testing

### Gameplay Goal

The player controls a character to slice objects.
The objective is to cut the object into pieces with **the closest possible weight**.

We assume that:

* All objects have **uniform density**
* Therefore, **weight comparison equals volume comparison**

## Principle of Slicing

We assume a **convex 3D object** composed of many triangles:

* Triangles form surfaces
* Surfaces form the 3D mesh

A slicing plane is defined as:

```
ax + by + cz + d = 0
```

### Triangle Classification

For each triangle:

* If the triangle is entirely on one side of the plane, it belongs to either the **upper mesh** or **lower mesh**
* If the plane intersects the triangle, the triangle must be split

### Triangle Splitting

Let the triangle vertices be **a, b, c**:

* Two vertices lie on one side of the plane (assume **a** and **b**)
* One vertex lies on the opposite side (**c**)

Let **P'** and **Q'** be the intersection points between the triangle edges and the slicing plane.

We generate:

* Two triangles:

  * `(a, b, P')`
  * `(b, P', Q')`
    → added to one mesh (upper or lower)
* One triangle:

  * `(c, P', Q')`
    → added to the opposite mesh

### Result

* The original mesh is split into **two new meshes**
* Two new GameObjects are created
* The original GameObject is removed

This is the core principle used by **EZ-Slice** to cut an object in half.

## Principle of Libra (Balance Scale)

### Components

* **Two scripts**

  * `calcWeight` (weighing pan)
  * `Libra` (balance beam)
* **Three GameObjects**

  * Two weighing pans
  * One balance beam

### Functionality

* `calcWeight`:

  * Detects objects colliding with the weighing pan
  * Calculates and stores the **weight (volume)** as a `float`
* `Libra`:

  * Stores an array of weighing pan GameObjects
  * Retrieves weight data from each pan
  * Compares the weights to determine balance results

### Weight (Volume) Calculation

Volume is calculated using the **Divergence Theorem**.

## Resources

* `.obj` files:

  * Tank (online resource)
  * Human (online resource)
  * Cube Monster (author: zxlprogram)
* Knife model (source: hsifeng)
* Player models ×2 (imported by yccct14)

## History

* **2025-12-16**

  * Project architecture finished by hsifeng

* **2025-12-19**

  * Project uploaded to GitHub by zxlprogram

* **2025-12-21**

  * Libra system added by zxlprogram

* **2025-12-30**

  * Knife model added by hsifeng
  * Player models ×2 imported
  * PvP game rules implemented
  * UI labels added by yccct14

## Known Issues / Missing Features

* Story content not implemented
* Sound effects not added
* `GameManager.cs` should reset sliced objects
