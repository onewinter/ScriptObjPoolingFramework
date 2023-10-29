# ScriptObjPoolingFramework README
 A ScriptableObject-based Pooling Framework using Unity 2019+'s built-in Object Pooling feature.

## Why Use This?
Object pooling is an essential part of any serious game development.  Since 2019, Unity has included a flexible [Pooling System](https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1.html), which is simple to use but requires a lot of boilerplate code to set up.  This system standardizes that code and lets you focus on creating your Pooled Objects.  

As an added bonus, moving some of the logic and data from the Monobehaviour Instance to the ScriptableObject Setup object utilizes design patterns such as [TypeObject](https://gameprogrammingpatterns.com/type-object.html) and [Flyweight](https://gameprogrammingpatterns.com/flyweight.html), leading to cleaner, more performant code.

## Requirements
Tested in Unity 2021.3+.

## Installation
Install via the Unity Package Manager by selecting "Add package from Git url..." and entering this git repository in the dialog box.  (Current version: `https://github.com/onewinter/ScriptObjPoolingFramework.git/#v1.0.9`)

Be sure to use a version tag in your URL so that you have control over any future API changes. 

Alternatively, enter the following in your manifest.json:
```
"com.onewinter.scriptobjpoolingframework": "https://github.com/onewinter/ScriptObjPoolingFramework.git/#v1.0.9"
```
 ## Simple Usage
 Create a new ScriptableObject-based Setup object by inheriting from PooledObjectSetup<T> and calling `SpawnNewPooledObject()`.
```
[CreateAssetMenu(fileName = "Tile_", menuName = "Tile Setup", order = 51)]
public class TileSetup : PooledObjectSetup<Tile>
{
    public Tile SpawnNewTile(Vector3 position) => SpawnNewPooledObject(position);    
}
```

Create a new MonoBehaviour-based Instance object by inheriting from PooledObjectBase<T>.
```
public class Tile : PooledObjectBase<TileSetup> { }
```

- Create the TileSetup ScriptableObjects by right-clicking in the Unity project explorer, then choosing Tile Setup.
- Create a prefab; add the Tile script to it, then drag that prefab into the Tile Setup under "base prefab".
- Add a field for TileSetup(s) to a TileManager or similar type object, then add new Tiles to your scene by calling `SpawnNewTile()` from the desired TileSetup.
- When it's time to remove the object from the scene, call `ReleaseToPool()` on the Tile to remove.

## Pooled Object Lifecycle
Unity's Pooling Framework provides a number of callback opportunities during the lifecycle of the pooled object.  This framework provides a standardized way to access them:
### PooledObjectBase
- `Awake()` the standard Unity Event function.  Called once, when the object is created in the pool.
- `AssignObjectSetup()` assigns the PooledObjectSetup to a protected variable named ObjectSetup; use the TypeObject property to access it in the instance. Called once, when the object is created in the pool.
- `InitializeObjectSetup()` a blank virtual function used to initialize any instance variables from the PooledObjectSetup TypeObject. Called once, when the object is created in the pool.
- `BeforeEnable()` blank virtual function, called right before the object is enabled, every time it's removed from the pool.
- `OnEnable()` standard Unity function, called right after the object is enabled, every time it's removed from the pool.
- `FinalizeObjectSetup(vars)` add your own function to pass any instance variables every time you spawn a pooled object in your scene.
- `BeforeDisable()` blank virtual function, called right before the object is disabled, every time it's sent back to the pool.
- `OnDisable()` standard Unity function, called right after the object is disabled, every time it's sent back to the pool.
- `BeforeDestroy()` blank virtual function, called right before the object is destroyed, once when it's no longer needed by the pool.
- `OnDestroy()` standard Unity function, called right after the object is destroyed, once when it's no longer needed by the pool.

## Passing Variables At Spawn Time
Insert a Finalize()-like method in-between `SpawnNewPooledObject()` and returning your spawned object in the function you use in your ScriptableObject to spawn new pooled objects.

```
[CreateAssetMenu(fileName = "Projectile_", menuName = "Projectile Setup", order = 51)]
public class ProjectileSetup : PooledObjectSetup<Projectile>
{
    public Projectile SpawnNewProjectile(Vector3 position, Vector3 target)
    {
        var newObject = SpawnNewPooledObject(position);
        newObject.FinalizeObjectSetup(target);
        return newObject;
    }
}

public class Projectile : PooledObjectBase<ProjectileSetup>
{
    Vector3 target;

    // initialize the projectile with its target
    public void FinalizeObjectSetup(Vector3 newTarget)
    {
        target = newTarget;
    }

}
```

## Example Scene
An example scene is provided in the package that shows pooled Tiles, Enemies, and Projectiles.  It displays the flexibility of how this system can be used: 
- By attaching cosmetic models at random when the object is created in the pool
- By changing the base prefab's color
- Or by using inheritance in the ObjectSetup scripts to utilize the [TypeObject pattern](https://gameprogrammingpatterns.com/type-object.html) and change the PooledObjectBase's behavior, while still using a single base prefab (in this example, you can change the projectile's behavior by changing the AnimationCurve in the inspector; more advanced examples would provide a `virtual MoveProjectile(Projectile projectile)` function in the Setup class for inheritors to override, which would allow the Setup class to control the Monobehaviour's actual, well, behavior.)

![Screenshot of enemies rushing at a center point and being shot repeatedly by projectiles](/Examples/Screenshots/scriptobjpooling-demo.gif)

## License
This library is released under the MIT license.  Examples use Unity Primitives and basic colors.

## Author
This project was written by TJ Cioffe, whose work can be found on [his website](http://onewinter.net) and [itch.io page](http://onewinter.itch.io), as well his main [github page](https://github.com/onewinter).
