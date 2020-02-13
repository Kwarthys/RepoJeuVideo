using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector
{
    public class CollisionSphereData
    {
        public Vector3 origin = new Vector3(0, 0, 0);
        public float radius = 2;
        public Vector3 direction = new Vector3(0, 0, 1);
        public float maxDistance = 1;
    }

    public static CollisionSphereData[] getSpheresFor(string resourceName, Transform usedExitPoint)
    {
        List<CollisionSphereData> data = new List<CollisionSphereData>();

        CollisionSphereData d;

        switch (resourceName)
        {
            case "QuadRoom":
                d = new CollisionSphereData();
                d.origin = usedExitPoint.position + usedExitPoint.forward * 4.5f;
                d.origin.y = 2;
                data.Add(d);
                break;

            case "GreatRoom":
            case "Hangar":
                d = new CollisionSphereData();
                d.origin = usedExitPoint.position + usedExitPoint.forward * 4.5f * 3;
                d.origin.y = 4;
                d.radius = 4;
                d.direction = -usedExitPoint.right;
                d.maxDistance = 18;
                data.Add(d);

                d = new CollisionSphereData();
                d.origin = usedExitPoint.position + usedExitPoint.forward * 4.5f;
                d.origin.y = 4;
                d.radius = 4;
                d.direction = -usedExitPoint.right;
                d.maxDistance = 18;
                data.Add(d);
                break;
        }


        return data.ToArray();
    }
}
