using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class OBJExporter : MonoBehaviour
{
    public static OBJExporter Instance { get; private set; }

    public string exportPath = "ExportedMesh.obj";
    public bool useVertexColors = true;

    public bool combineSimilarCubes = true;
    public float combineDistanceThreshold = 0.1f; // How close cubes need to be to combine
    public bool includeMaterials = true; // Toggle to include/exclude materials

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    public void ExportGameObjects(GameObject[] objectsToExport) {
        StringBuilder sb = new StringBuilder();
        StringBuilder mtlBuilder = new StringBuilder();

        int vertexOffset = 1; // OBJ files are 1-indexed
        int materialIndex = 0;
        string mtlFileName = Path.GetFileNameWithoutExtension(exportPath) + ".mtl";

        // Start OBJ file
        sb.AppendLine("# Exported from Unity");
        if (includeMaterials) {
            sb.AppendLine("mtllib " + mtlFileName);
        }

        // Group objects by material and color if combining is enabled
        Dictionary<Material, List<GameObject>> materialGroups = new Dictionary<Material, List<GameObject>>();
        Dictionary<Material, List<GameObject>> combinedGroups = new Dictionary<Material, List<GameObject>>();

        // First group by material
        foreach (GameObject go in objectsToExport) {
            MeshFilter meshFilter = go.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null) continue;

            Material mat = go.GetComponent<Renderer>()?.sharedMaterial;
            if (mat == null) mat = new Material(Shader.Find("Standard")); // Default material

            if (!materialGroups.ContainsKey(mat)) {
                materialGroups[mat] = new List<GameObject>();
            }
            materialGroups[mat].Add(go);
        }

        // Then combine nearby objects with the same material if enabled
        if (combineSimilarCubes) {
            foreach (var group in materialGroups) {
                List<GameObject> combinedList = new List<GameObject>();
                List<GameObject> remainingObjects = new List<GameObject>(group.Value);

                while (remainingObjects.Count > 0) {
                    GameObject current = remainingObjects[0];
                    remainingObjects.RemoveAt(0);
                    List<GameObject> cluster = new List<GameObject> { current };

                    // Find all nearby objects with the same material
                    for (int i = remainingObjects.Count - 1; i >= 0; i--) {
                        if (Vector3.Distance(current.transform.position, remainingObjects[i].transform.position) <= combineDistanceThreshold) {
                            cluster.Add(remainingObjects[i]);
                            remainingObjects.RemoveAt(i);
                        }
                    }

                    // Create a combined game object for this cluster
                    if (cluster.Count > 1) {
                        GameObject combinedGO = new GameObject("Combined_" + group.Key.name);
                        combinedGO.transform.position = GetCenter(cluster);
                        Mesh combinedMesh = CombineMeshes(cluster);
                        combinedGO.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
                        combinedGO.AddComponent<MeshRenderer>().sharedMaterial = group.Key;
                        combinedList.Add(combinedGO);
                    } else {
                        combinedList.AddRange(cluster);
                    }
                }

                combinedGroups[group.Key] = combinedList;
            }
        } else {
            combinedGroups = materialGroups;
        }

        // Process each object or combined object
        foreach (var group in combinedGroups) {
            Material mat = group.Key;
            Color color = mat != null ? mat.color : Color.white;

            // Write material definition if materials are included
            if (includeMaterials) {
                string materialName = "mat_" + materialIndex++;
                mtlBuilder.AppendLine("newmtl " + materialName);
                mtlBuilder.AppendLine("Kd " + color.r + " " + color.g + " " + color.b);
                mtlBuilder.AppendLine("d 1.0"); // Alpha (1 = opaque)
                mtlBuilder.AppendLine("illum 1"); // Basic illumination

                sb.AppendLine("usemtl " + materialName);
            }

            foreach (GameObject go in group.Value) {
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshFilter == null || meshFilter.sharedMesh == null) continue;

                Mesh mesh = meshFilter.sharedMesh;

                // Write vertices
                for (int vertexIndex = 0; vertexIndex < mesh.vertices.Length; vertexIndex++) {
                    Vector3 vertex = mesh.vertices[vertexIndex];
                    Vector3 worldVertex = go.transform.TransformPoint(vertex);
                    sb.AppendLine($"v {worldVertex.x} {worldVertex.y} {worldVertex.z}");

                    if (useVertexColors && mesh.colors.Length > 0 && vertexIndex < mesh.colors.Length) {
                        Color vertexColor = mesh.colors[vertexIndex];
                        sb.AppendLine($"vc {vertexColor.r} {vertexColor.g} {vertexColor.b}");
                    }
                }

                // Write normals
                foreach (Vector3 normal in mesh.normals) {
                    Vector3 worldNormal = go.transform.TransformDirection(normal);
                    sb.AppendLine($"vn {worldNormal.x} {worldNormal.y} {worldNormal.z}");
                }

                // Write UVs
                foreach (Vector2 uv in mesh.uv) {
                    sb.AppendLine($"vt {uv.x} {uv.y}");
                }

                // Write faces
                for (int i = 0; i < mesh.triangles.Length; i += 3) {
                    int idx1 = mesh.triangles[i] + vertexOffset;
                    int idx2 = mesh.triangles[i + 1] + vertexOffset;
                    int idx3 = mesh.triangles[i + 2] + vertexOffset;

                    if (mesh.normals.Length > 0 && mesh.uv.Length > 0) {
                        sb.AppendLine($"f {idx1}/{idx1}/{idx1} {idx2}/{idx2}/{idx2} {idx3}/{idx3}/{idx3}");
                    } else if (mesh.normals.Length > 0) {
                        sb.AppendLine($"f {idx1}//{idx1} {idx2}//{idx2} {idx3}//{idx3}");
                    } else {
                        sb.AppendLine($"f {idx1} {idx2} {idx3}");
                    }
                }

                vertexOffset += mesh.vertices.Length;
            }
        }

        // Save files
        string directory = Path.GetDirectoryName(exportPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(exportPath, sb.ToString());

        if (includeMaterials) {
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(exportPath), mtlFileName), mtlBuilder.ToString());
        }

        Debug.Log("Exported OBJ to: " + exportPath);
    }

    // Helper method to get center of a group of objects
    private Vector3 GetCenter(List<GameObject> objects) {
        Vector3 center = Vector3.zero;
        foreach (GameObject go in objects) {
            center += go.transform.position;
        }
        return center / objects.Count;
    }

    // Helper method to combine meshes
    private Mesh CombineMeshes(List<GameObject> objects) {
        List<CombineInstance> combines = new List<CombineInstance>();

        foreach (GameObject go in objects) {
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null) {
                CombineInstance ci = new CombineInstance();
                ci.mesh = mf.sharedMesh;
                ci.transform = go.transform.localToWorldMatrix;
                combines.Add(ci);
            }
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combines.ToArray(), true, true);
        return combinedMesh;
    }
    public static GameObject[] GetAllChildren(GameObject parent) {
        int childCount = parent.transform.childCount;
        GameObject[] children = new GameObject[childCount];

        for (int i = 0; i < childCount; i++) {
            children[i] = parent.transform.GetChild(i).gameObject;
        }

        return children;
    }
}
