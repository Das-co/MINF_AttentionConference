using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing {
    Forwards,
    Backwards,
    Up,
    Down,
    Inwards,
    Outwards,
    Unknown
}

public class Helper {
    public static Facing GetFacing (Vector3 direction) {
        return GetFacing (direction, false);
    }

    public static Facing GetFacing (Vector3 direction, bool invertInwardOutward) {
        int closestDirection = ClosestDirection (direction, new Vector3 [6] {
            Vector3.forward,
            Vector3.back,
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right
        });

        switch (closestDirection) {
            case 0:
                return Facing.Forwards;
            case 1:
                return Facing.Backwards;
            case 2:
                return Facing.Up;
            case 3:
                return Facing.Down;
            case 4:
                if (invertInwardOutward) {
                    return Facing.Outwards;
                } else {
                    return Facing.Inwards;
                }
            case 5:
                if (invertInwardOutward) {
                    return Facing.Inwards;
                } else {
                    return Facing.Outwards;
                }
        }

        return Facing.Unknown;
    }

    public static int ClosestDirection (Vector3 direction, Vector3 [] directions) {
        float [] distances = new float [directions.Length];

        for (int i = 0; i < distances.Length; i++) {
            distances [i] = Mathf.Acos (Vector3.Dot (direction, directions [i]));
        }

        // @TODO: refactor, sort isn't needed here
        int [] indexes = Helper.BubbleSort (ref distances);

        return indexes [0];
    }

    /**
     * Sorts values and returns array of indexes (new index => old index)
     */
    public static int [] BubbleSort<T> (ref T [] values) {
        return BubbleSort<T> (ref values, Comparer<T>.Default);
    }

    private static int [] BubbleSort<T> (ref T [] values, IComparer<T> comparer) {
        bool sorted = false;

        int [] indexes = new int [values.Length];
        for (int i = 0; i < indexes.Length; i++) {
            indexes [i] = i;
        }

        while (sorted == false) {
            bool changedPositions = false;

            for (int i = 0; i < values.Length - 1; i++) {
                if (comparer.Compare (values [i], values [i + 1]) > 0) {
                    T tmpValue = values [i + 1];
                    values [i + 1] = values [i];
                    values [i] = tmpValue;

                    int tmpIndex = indexes [i + 1];
                    indexes [i + 1] = indexes [i];
                    indexes [i] = tmpIndex;

                    changedPositions = true;
                }
            }

            if (changedPositions == false) {
                sorted = true;
            }
        }

        return indexes;
    }

    public static string Vector3DebugString (Vector3 vector) {
        return "(" + vector.x + ", " + vector.y + ", " + vector.z + ")";
    }
}

public static class MeshExtension {
    private class Vertices {
        List<Vector3> verts = null;
        List<Vector2> uv1 = null;
        List<Vector2> uv2 = null;
        List<Vector2> uv3 = null;
        List<Vector2> uv4 = null;
        List<Vector3> normals = null;
        List<Vector4> tangents = null;
        List<Color32> colors = null;
        List<BoneWeight> boneWeights = null;

        public Vertices () {
            verts = new List<Vector3> ();
        }
        public Vertices (Mesh aMesh) {
            verts = CreateList (aMesh.vertices);
            uv1 = CreateList (aMesh.uv);
            uv2 = CreateList (aMesh.uv2);
            uv3 = CreateList (aMesh.uv3);
            uv4 = CreateList (aMesh.uv4);
            normals = CreateList (aMesh.normals);
            tangents = CreateList (aMesh.tangents);
            colors = CreateList (aMesh.colors32);
            boneWeights = CreateList (aMesh.boneWeights);
        }

        private List<T> CreateList<T> (T [] aSource) {
            if (aSource == null || aSource.Length == 0)
                return null;
            return new List<T> (aSource);
        }
        private void Copy<T> (ref List<T> aDest, List<T> aSource, int aIndex) {
            if (aSource == null)
                return;
            if (aDest == null)
                aDest = new List<T> ();
            aDest.Add (aSource [aIndex]);
        }
        public int Add (Vertices aOther, int aIndex) {
            int i = verts.Count;
            Copy (ref verts, aOther.verts, aIndex);
            Copy (ref uv1, aOther.uv1, aIndex);
            Copy (ref uv2, aOther.uv2, aIndex);
            Copy (ref uv3, aOther.uv3, aIndex);
            Copy (ref uv4, aOther.uv4, aIndex);
            Copy (ref normals, aOther.normals, aIndex);
            Copy (ref tangents, aOther.tangents, aIndex);
            Copy (ref colors, aOther.colors, aIndex);
            Copy (ref boneWeights, aOther.boneWeights, aIndex);
            return i;
        }
        public void AssignTo (Mesh aTarget) {
            aTarget.SetVertices (verts);
            if (uv1 != null)
                aTarget.SetUVs (0, uv1);
            if (uv2 != null)
                aTarget.SetUVs (1, uv2);
            if (uv3 != null)
                aTarget.SetUVs (2, uv3);
            if (uv4 != null)
                aTarget.SetUVs (3, uv4);
            if (normals != null)
                aTarget.SetNormals (normals);
            if (tangents != null)
                aTarget.SetTangents (tangents);
            if (colors != null)
                aTarget.SetColors (colors);
            if (boneWeights != null)
                aTarget.boneWeights = boneWeights.ToArray ();
        }
    }

    public static Mesh GetSubmesh (this Mesh aMesh, int aSubMeshIndex) {
        if (aSubMeshIndex < 0 || aSubMeshIndex >= aMesh.subMeshCount)
            return null;
        int [] indices = aMesh.GetTriangles (aSubMeshIndex);
        Vertices source = new Vertices (aMesh);
        Vertices dest = new Vertices ();
        Dictionary<int, int> map = new Dictionary<int, int> ();
        int [] newIndices = new int [indices.Length];
        for (int i = 0; i < indices.Length; i++) {
            int o = indices [i];
            int n;
            if (!map.TryGetValue (o, out n)) {
                n = dest.Add (source, o);
                map.Add (o, n);
            }
            newIndices [i] = n;
        }
        Mesh m = new Mesh ();
        dest.AssignTo (m);
        m.triangles = newIndices;
        return m;
    }
}