using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IntListWrapper {
    public List<int> innerList = new List<int>();
}

public class Veriaition {
    public List<IntListWrapper> automata = new List<IntListWrapper>();
    public Hash128 hash = new Hash128();
    public Vector2 size;

    public Vector2 Size() {
        if (automata == null || automata.Count == 0 || automata[0] == null) {
            Debug.Log("fuck");
            return Vector2.zero;
        }
        size = new Vector2(automata.Count, automata[0].innerList.Count);
        return size;
    }
} 

[System.Serializable]
public class RuleSet {
    public int[] survivalRules;  // Neighbor counts for survival
    public int[] birthRules;        // Neighbor counts for birth
}

public class Automata {
    [SerializeField]
    public List<Veriaition> veriaitions = new List<Veriaition>();
    public List<int> offsets = new List<int>();
    public Color color = Color.yellow;

    public Automata(int mew){
        if ( mew == 0) {
            veriaitions.Add(new Veriaition());
            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[0].innerList.Add(0);
            veriaitions[0].automata[0].innerList.Add(1);
            veriaitions[0].automata[0].innerList.Add(0);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[1].innerList.Add(0);
            veriaitions[0].automata[1].innerList.Add(1);
            veriaitions[0].automata[1].innerList.Add(0);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[2].innerList.Add(0);
            veriaitions[0].automata[2].innerList.Add(1);
            veriaitions[0].automata[2].innerList.Add(0);

            veriaitions[0].Size();
            color = new Color(171 / 255f, 255 / 255f, 79 / 255f);//green
        }
        if (mew == 1) {
            veriaitions.Add(new Veriaition());
            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[0].innerList.Add(0);
            veriaitions[0].automata[0].innerList.Add(1);
            veriaitions[0].automata[0].innerList.Add(0);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[1].innerList.Add(1);
            veriaitions[0].automata[1].innerList.Add(1);
            veriaitions[0].automata[1].innerList.Add(1);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[2].innerList.Add(0);
            veriaitions[0].automata[2].innerList.Add(1);
            veriaitions[0].automata[2].innerList.Add(0);

            veriaitions[0].Size();
            color = new Color(4 / 255f, 119 / 255f, 59 / 255f);//was red now dark green
        }

        if (mew == 2) {
            veriaitions.Add(new Veriaition());
            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[0].innerList.Add(1);
            veriaitions[0].automata[0].innerList.Add(1);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[1].innerList.Add(1);
            veriaitions[0].automata[1].innerList.Add(1);

            veriaitions[0].Size();
            color = new Color(8 / 255f, 189 / 255f, 189 / 255f);// blue
        }

        if (mew == 3) {
            veriaitions.Add(new Veriaition());
            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[0].innerList.Add(1);
            veriaitions[0].automata[0].innerList.Add(1);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[1].innerList.Add(1);
            veriaitions[0].automata[1].innerList.Add(0);

            veriaitions[0].Size();
            color = Color.green;
        }

        if (mew == 4) {
            veriaitions.Add(new Veriaition());
            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[0].innerList.Add(1);
            veriaitions[0].automata[0].innerList.Add(0);
            veriaitions[0].automata[0].innerList.Add(0);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[1].innerList.Add(0);
            veriaitions[0].automata[1].innerList.Add(1);
            veriaitions[0].automata[1].innerList.Add(0);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[2].innerList.Add(0);
            veriaitions[0].automata[2].innerList.Add(0);
            veriaitions[0].automata[2].innerList.Add(1);

            veriaitions[0].Size();
            color = Color.green;
        }

        if (mew == 5) {
            veriaitions.Add(new Veriaition());
            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[0].innerList.Add(0);
            veriaitions[0].automata[0].innerList.Add(1);
            veriaitions[0].automata[0].innerList.Add(0);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[1].innerList.Add(0);
            veriaitions[0].automata[1].innerList.Add(1);
            veriaitions[0].automata[1].innerList.Add(1);

            veriaitions[0].automata.Add(new IntListWrapper());
            veriaitions[0].automata[2].innerList.Add(0);
            veriaitions[0].automata[2].innerList.Add(0);
            veriaitions[0].automata[2].innerList.Add(0);

            veriaitions[0].Size();
            color = new Color(0xF7 / 255f, 0x55 / 255f, 0x90 / 255f);
        }

        GenerateGliderVariants();
        HashVeriaitions();
    }
    public void GenerateGliderVariants() {
        // Get the original glider pattern
        Veriaition original = veriaitions[0];

        // Create rotated and mirrored patterns
        for (int i = 0; i < 4; i++) // Four rotations (0°, 90°, 180°, 270°)
        {
            Veriaition rotated = RotatePattern(original, i);
            rotated.Size();
            veriaitions.Add(rotated);

            Veriaition horizontalMirror = MirrorPattern(rotated, true);
            horizontalMirror.Size();
            Veriaition verticalMirror = MirrorPattern(rotated, false);
            verticalMirror.Size();

            veriaitions.Add(horizontalMirror);
            veriaitions.Add(verticalMirror);
        }
    }

    private Veriaition RotatePattern(Veriaition input, int rotation) {
        Veriaition result = new Veriaition();
        int rows = input.automata.Count;
        int cols = input.automata[0].innerList.Count;

        for (int i = 0; i < cols; i++) {
            IntListWrapper newRow = new IntListWrapper();

            for (int j = 0; j < rows; j++) {
                if (rotation == 1) // Rotate 90° clockwise
                    newRow.innerList.Add(input.automata[rows - j - 1].innerList[i]);
                else if (rotation == 2) // Rotate 180°
                    newRow.innerList.Add(input.automata[rows - i - 1].innerList[cols - j - 1]);
                else if (rotation == 3) // Rotate 270° clockwise
                    newRow.innerList.Add(input.automata[j].innerList[cols - i - 1]);
                else // 0° (original)
                    newRow.innerList.Add(input.automata[i].innerList[j]);
            }

            result.automata.Add(newRow);
        }

        return result;
    }

    private Veriaition MirrorPattern(Veriaition input, bool horizontal) {
        Veriaition result = new Veriaition();

        foreach (var row in input.automata) {
            IntListWrapper newRow = new IntListWrapper();

            if (horizontal) {
                for (int i = row.innerList.Count - 1; i >= 0; i--) {
                    newRow.innerList.Add(row.innerList[i]);
                }
            } else {
                newRow.innerList.AddRange(row.innerList);
            }

            result.automata.Add(newRow);
        }

        if (!horizontal) // For vertical mirror, reverse the entire pattern's rows
        {
            result.automata.Reverse();
        }

        return result;
    }

    private void HashVeriaitions() {
        for (int v = 0; v < veriaitions.Count; v++) {
            for (int i = 0; i < veriaitions[v].automata.Count; i++) {
                for (int j = 0; j < veriaitions[v].automata[i].innerList.Count; j++) {
                    veriaitions[v].hash.Append(veriaitions[v].automata[i].innerList[j]);
                }
            }
        }
    }
}

public enum Tool {
    pen, selector
}

public class GridManager : MonoBehaviour {
    [Header("Grid Settings")]
    public int width = 50;
    public int height = 50;
    public int game = 0;
    public int z = 1;
    public float cellSize = 1f;
    public bool wrapEdges = true;
    public bool bazingaL = true;
    public bool bazingaR = true;
    public bool mirrorX = true;
    public bool mirrorY = true;
    public bool color = true;
    private bool layer2Active = false;
    private bool secondClick = false;
    private Vector2Int firstClick = new Vector2Int();
    private bool backGround = true;
    private bool flush = true;
    public bool thirdDemontion = false;
    public Color aliveColor = Color.black;
    public Color deadColor = Color.white;
    public Color chesswhite;
    public Color chessBlack;
    public Color mirrorColor;
    public Color temp;
    public Color pen;
    private Vector2Int penPos;
    public GameObject cube;
    public GameObject parentFor3d;
    public GameObject rotationParent;

    private Tool activeTool = Tool.pen;

    public Slider slider;
    public Slider gameSlider;

    [Header("Interaction")]
    public bool drawEnabled = true;
    public int brushSize = 1;
    public bool eraseMode = false;

    [Header("Simulation Settings")]
    [Range(0.001f, 0.01f)]
    public float updateInterval = 0.01f;
    public bool simulate = true;

    [Header("Rules")]
    public List<RuleSet> ruleSet = new List<RuleSet>();

    private int[,] grid;
    public List<Automata> wanted = new List<Automata>();
    public List<Vector2> sizes = new List<Vector2>();
    private int[,] newGrid;
    private Texture2D gridTexture;
    private Texture2D layer2Texture;
    public GameObject layer2;
    private float timer = 0f;

    public Renderer textureRenderer;

    public AudioSource audioSource;
    public AudioClip shutterSound;

    void Start() {
        InitializeGrid();
        InitializeTexture();
        InitializeSecondLayerTexture();
        //wanted.Add(new Automata(3));
        wanted.Add(new Automata(0));
        wanted.Add(new Automata(1));
        wanted.Add(new Automata(2));
        //wanted.Add(new Automata(4));
        wanted.Add(new Automata(5));
        UpdateSizes();
        parentFor3d.transform.SetParent(rotationParent.transform);
        gameSlider.maxValue = ruleSet.Count - 1;
        Render();
    }

    void Update() {
        if (simulate && !layer2Active) {
            timer += Time.deltaTime;
            if (timer >= updateInterval) {
                timer = 0f;
                SimulateStep();
                Render();
                ThirdDomention();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            SpaceB();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            simulate = !simulate;
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ClearGrid();
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            ThirdDomention();
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            ExportObj();
        }

        RotateObject();
        //RotateObjectWithMouse();
        HandleInput();
        SecondLayer();
    }

    void InitializeGrid() {
        grid = new int[width, height];
        newGrid = new int[width, height];
    }
    void InitializeTexture() {
        gridTexture = new Texture2D(width, height);
        gridTexture.filterMode = FilterMode.Point;
        textureRenderer.material.SetTexture("_Texture2D", gridTexture);
    }
    void InitializeSecondLayerTexture() {
        layer2Texture = new Texture2D(width, height);
        layer2Texture.filterMode = FilterMode.Point;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                layer2Texture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        layer2.GetComponent<Renderer>().material.SetTexture("_Texture2D", layer2Texture);
        layer2Texture.Apply();
    }
    void SimulateStep() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighbors = CountNeighbors(x, y);
                newGrid[x, y] = ApplyRules(grid[x, y], neighbors);
            }
        }

        // Swap grids
        (grid, newGrid) = (newGrid, grid);
    }
    int ApplyRules(int currentState, int neighbors) {
        if (currentState == 1) {
            return System.Array.Exists(ruleSet[game].survivalRules, n => n == neighbors) ? 1 : 0;
        } else {
            return System.Array.Exists(ruleSet[game].birthRules, n => n == neighbors) ? 1 : 0;
        }
    }
    int CountNeighbors(int x, int y) {
        int count = 0;

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) continue;

                int neighborX = x + i;
                int neighborY = y + j;

                if (wrapEdges) {
                    neighborX = (neighborX + width) % width;
                    neighborY = (neighborY + height) % height;
                } else if (neighborX < 0 || neighborX >= width ||
                         neighborY < 0 || neighborY >= height) {
                    continue;
                }

                count += grid[neighborX, neighborY];
            }
        }

        return count;
    }
    void UpdateTexture() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                //Color aliveRandom = Random.ColorHSV();.
                if (grid[x, y] == 1) {
                    gridTexture.SetPixel(x, y, aliveColor);
                }
            }
        }
    }
    void HandleInput() {
        if(activeTool != Tool.pen) {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (Input.GetMouseButton(0)) {
            HandleMouseClick(1);
        } else if (Input.GetMouseButton(1)) {
            HandleMouseClick(0);
        }
    }
    void HandleMouseClick(int value) {
        if (layer2Active) {
            return;
        }

        Vector3 gridOrigin = transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane gridPlane = new Plane(Vector3.forward, gridOrigin);

        if (gridPlane.Raycast(ray, out float enter)) {
            Vector3 worldPosition = ray.GetPoint(enter);
            worldPosition = new Vector3(worldPosition.x * width, worldPosition.y * height, worldPosition.z);

            int x = Mathf.FloorToInt((worldPosition.x - gridOrigin.x) / cellSize + width / 2);
            int y = Mathf.FloorToInt((worldPosition.y - gridOrigin.y) / cellSize + height / 2);

            if (x >= 0 && x < width && y >= 0 && y < height) {
                Draw(value, x, y);
            }
        }
    }
    private void SpaceB() {
        if (simulate) {
            simulate = !simulate;
        }
        SimulateStep();
        Render();
    }
    private void SecondLayer() {
        if(activeTool == Tool.pen) {
            ShowPenPos();
        }
        if(activeTool != Tool.selector) {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (Input.GetMouseButtonDown(0)) {
            HandleLayer2MouseClick(1);
            layer2Texture.Apply();
        }
        if (secondClick) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    layer2Texture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }

            Vector3 gridOrigin = transform.position;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane gridPlane = new Plane(Vector3.forward, gridOrigin);

            if (gridPlane.Raycast(ray, out float enter)) {
                Vector3 worldPosition = ray.GetPoint(enter);
                worldPosition = new Vector3(worldPosition.x * width, worldPosition.y * height, worldPosition.z);

                int x = Mathf.FloorToInt((worldPosition.x - gridOrigin.x) / cellSize + width / 2);
                int y = Mathf.FloorToInt((worldPosition.y - gridOrigin.y) / cellSize + height / 2);

                if (x >= 0 && x < width && y >= 0 && y < height) {
                    int minX = Mathf.Min(firstClick.x, x);
                    int maxX = Mathf.Max(firstClick.x, x);
                    int minY = Mathf.Min(firstClick.y, y);
                    int maxY = Mathf.Max(firstClick.y, y);

                    // Create a color array to hold pixel data
                    Color[] colors = new Color[(maxX - minX + 1) * (maxY - minY + 1)];

                    for (int j = minY, index = 0; j <= maxY; j++) {
                        for (int i = minX; i <= maxX; i++, index++) {
                            if (i == minX || i == maxX || j == minY || j == maxY) {
                                colors[index] = new Color(0, 0, 0, 0.9f); // Border color
                            } else {
                                colors[index] = new Color(0, 1, 0, 0.5f); // Inner color
                            }
                        }
                    }

                    // Apply the color array to the texture
                    layer2Texture.SetPixels(minX, minY, maxX - minX + 1, maxY - minY + 1, colors);
                    layer2Texture.Apply();
                }
            }
        }
    }
    private void ShowPenPos() {
        layer2Texture.SetPixel(penPos.x, penPos.y, new Color(0,0,0,0));

        Vector3 gridOrigin = transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane gridPlane = new Plane(Vector3.forward, gridOrigin);

        if (gridPlane.Raycast(ray, out float enter)) {
            Vector3 worldPosition = ray.GetPoint(enter);
            worldPosition = new Vector3(worldPosition.x * width, worldPosition.y * height, worldPosition.z);

            int x = Mathf.FloorToInt((worldPosition.x - gridOrigin.x) / cellSize + width / 2);
            int y = Mathf.FloorToInt((worldPosition.y - gridOrigin.y) / cellSize + height / 2);


            if (x >= 0 && x < width && y >= 0 && y < height) {
                if(penPos.x != x || penPos.y != y) {
                    layer2Texture.SetPixel(x, y, pen);
                    penPos = new Vector2Int(x, y);
                    layer2Texture.Apply();
                }
            }
        }
    }
    void HandleLayer2MouseClick(int value) {
        Vector3 gridOrigin = transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane gridPlane = new Plane(Vector3.forward, gridOrigin);

        if (gridPlane.Raycast(ray, out float enter)) {
            Vector3 worldPosition = ray.GetPoint(enter);
            worldPosition = new Vector3(worldPosition.x * width, worldPosition.y * height, worldPosition.z);

            int x = Mathf.FloorToInt((worldPosition.x - gridOrigin.x) / cellSize + width / 2);
            int y = Mathf.FloorToInt((worldPosition.y - gridOrigin.y) / cellSize + height / 2);

            if (x >= 0 && x < width && y >= 0 && y < height) {
                if (!secondClick) {
                    layer2Texture.SetPixel(x, y, aliveColor);
                    firstClick = new Vector2Int(x, y);
                    secondClick = true;
                } else {
                    int minX = Mathf.Min(firstClick.x, x);
                    int maxX = Mathf.Max(firstClick.x, x);
                    int minY = Mathf.Min(firstClick.y, y);
                    int maxY = Mathf.Max(firstClick.y, y);

                    for (int i = minX + 1; i <= maxX - 1; i++) {
                        for (int j = minY + 1; j <= maxY - 1; j++) {
                            WGrid(i, j, 1);
                        }
                    }
                    secondClick = false;
                    FlushLayerTwo();
                    Render();
                }
            }
        }
    }
    private void FlushLayerTwo() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                layer2Texture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
    }
    public void HighlightAutomata() {
        HashSet<Hash128>[,] hashGrid = new HashSet<Hash128>[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                hashGrid[x, y] = new HashSet<Hash128>();
            }
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                foreach (Hash128 hash in HashAllSizes(y, x)) {
                    hashGrid[x, y].Add(hash);
                }
            }
        }

        foreach (Automata au in wanted) {
            foreach (Veriaition ve in au.veriaitions) {
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        if (hashGrid[x, y].Contains(ve.hash)) { 
                            ColorAutomata(y, x, ve, au); 
                        }
                    }
                }
            }
        }
    }

    private List<Hash128> HashAllSizes(int x, int y) {
        List<Hash128> hashes = new List<Hash128>();

        foreach(Vector2 size in sizes) {
            hashes.Add(HashAGrid(x, y, size));
        }

        return hashes;
    }
    private Hash128 HashAGrid(int x, int y, Vector2 size) {
        Hash128 hash = new Hash128();
        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                hash.Append(RGrid(i + x, j + y));
            }
        }
        return hash;
    }
    private void Draw(int value, int x, int y) {
        int changeCheck = 0;
        if(value == 0) {
            changeCheck = 1;
        }else if (value == 1) {
            changeCheck = 0;
        }

        if (RGrid(x, y) == changeCheck || !flush) {
            int midX = width / 2;
            int midY = height / 2;
            int disX = midX - x;
            int disY = midY - y;

            if (mirrorX) {
                WGrid(midX - disX, midY + disY, value);
                WGrid(midX - disX, midY - disY, value);

                if ((bazingaL && bazingaR)) {
                    WGrid(midX + disX, midY - disY, value);
                }
            }
            if (mirrorY) {
                WGrid(midX + disX, midY - disY, value);
                WGrid(midX - disX, midY - disY, value);

                if ((bazingaL && bazingaR)) {
                    WGrid(midX - disX, midY + disY, value);
                }
            }
            if (bazingaR) {
                WGrid(midY - disY, midX - disX, value);
                WGrid(x, y, value);

                if ((mirrorX && mirrorY)) {
                    WGrid(midY + disY, midX + disX, value);
                }
            }
            if (bazingaL) {
                WGrid(midY + disY, midX + disX, value);
                WGrid(x, y, value);

                if ((mirrorX && mirrorY)) {
                    WGrid(midY - disY, midX - disX, value);
                }
            }
            if ((bazingaR && bazingaL) && (mirrorX && mirrorY)) {
                WGrid(midY + disY, midX - disX, value);
                WGrid(midY - disY, midX + disX, value);
            }
            if ((bazingaR && bazingaL) || (mirrorX && mirrorY)) {
                WGrid(midX + disX, midY + disY, value);
            } else if (!mirrorX && !mirrorY && !bazingaL && !bazingaR) {
                WGrid(x, y, value);
            }
            Render();
        }
    }
    private bool CheckAutomata(int x, int y, int veriaition, int a) {
        for (int i = 0; i < wanted[a].veriaitions[veriaition].automata.Count; i++) {
            for (int j = 0; j < wanted[a].veriaitions[veriaition].automata[i].innerList.Count; j++) {
                // Swap i and j if the glider appears rotated
                if (RGrid(x + j, y + i) != wanted[a].veriaitions[veriaition].automata[i].innerList[j]) {
                    return false;
                }
            }
        }
        return true;
    }
    private void ColorAutomata(int x, int y, Veriaition veriaition, Automata automata) {
        for (int i = 0; i < veriaition.size.x; i++) {
            for (int j = 0; j < veriaition.size.y; j++) {
                if (veriaition.automata[i].innerList[j] == 1) {
                    gridTexture.SetPixel(x + i, y + j, automata.color);
                }
            }
        }
    }
    private void MirrorAxis() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (mirrorY) {
                    if(x == width / 2) {
                        gridTexture.SetPixel(x, y, mirrorColor);
                    }
                }
                if (mirrorX) {
                    if(y == height / 2) {
                        gridTexture.SetPixel(x, y, mirrorColor);
                    }
                }
                if (bazingaR) {
                    if(x == y) {
                        gridTexture.SetPixel(x, y, mirrorColor);
                    }
                }
                if (bazingaL) {
                    if(x == height - y) {
                        gridTexture.SetPixel(x, y, mirrorColor);
                    }
                }
            }
        }
    }
    private void ChessBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if ((x + y) % 2 == 0) {
                    gridTexture.SetPixel(x, y, chesswhite);
                } else {
                    gridTexture.SetPixel(x, y, chessBlack);
                }
            }
        }
    }
    public void ClearGrid()
    {
        InitializeGrid();
        Flush();
        if (backGround) {
            ChessBoard();
            MirrorAxis();
        }
        gridTexture.Apply();
    }
    private void Render() {
        if (flush) {
            Flush();
        }
        if (backGround && flush) {
            ChessBoard();
            MirrorAxis();
        }
        UpdateTexture();
        if (color) {
            HighlightAutomata();
        }
        gridTexture.Apply();
    }
    private void Flush() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                gridTexture.SetPixel(x, y, deadColor);
            }
        }
    }
    private bool ThirdDomention() {
        if (!thirdDemontion) {
            return false;
        }
        bool change = false;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Color color = gridTexture.GetPixel(x, y);
                if (color == chessBlack) {
                    continue;
                } else if (color == chesswhite) {
                    continue;
                } else if (color == deadColor) {
                    continue;
                } else if (AreColorsEqual(color, mirrorColor)) {
                    continue;
                } else {
                    GameObject instance = Instantiate(cube);
                    float scale = 1f / width;
                    instance.transform.SetParent(parentFor3d.transform);
                    instance.transform.localScale = new Vector3(scale, scale, scale);
                    instance.transform.position = new Vector3((x - width / 2) * scale + 2, (y - height / 2) * scale, z * scale);
                    instance.GetComponent<MeshRenderer>().material.color = color;
                    change = true;
                }
            }
        }
        if (change) {
            z += 1;
        }
        return true;
    }
    private void RotateObject() {
        float rotationSpeed = 200f;

        if (Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKey(KeyCode.J)) {
                rotationParent.transform.rotation = Quaternion.Euler(0, 90, 0);
            } else if (Input.GetKey(KeyCode.K)) {
                rotationParent.transform.rotation = Quaternion.Euler(0, 180, 0);
            } else if (Input.GetKey(KeyCode.L)) {
                rotationParent.transform.rotation = Quaternion.Euler(0, -90, 0);
            } else if (Input.GetKey(KeyCode.I)) {
                rotationParent.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        } else {
            if (Input.GetKey(KeyCode.J)) {
                rotationParent.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); //Z-axis (counterclockwise)
            }
            if (Input.GetKey(KeyCode.K)) {
                rotationParent.transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime); //Z-axis (clockwise)
            }
            if (Input.GetKey(KeyCode.L)) {
                rotationParent.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); //Y-axis
            }
            if (Input.GetKey(KeyCode.I)) {
                rotationParent.transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime); //Y-axis (opposite direction)
            }
        }
    }
    private void RotateObjectWithMouse() {
        float rotationSpeed = 200f; // Adjust to control the sensitivity of the rotation

        if (Input.GetMouseButton(0)) { // Detect left mouse button hold
            float mouseX = Input.GetAxis("Mouse X"); // Horizontal mouse movement
            float mouseY = Input.GetAxis("Mouse Y"); // Vertical mouse movement

            // Rotate around the Y-axis based on horizontal mouse movement
            rotationParent.transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime, Space.World);

            // Rotate around the X-axis based on vertical mouse movement
            rotationParent.transform.Rotate(Vector3.right * -mouseY * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
    bool AreColorsEqual(Color a, Color b, float tolerance = 0.01f) {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance &&
               Mathf.Abs(a.a - b.a) < tolerance;
    }
    public void XmirrorToggle() {
        mirrorX = !mirrorX;
        Render();
    }
    public void YmirrorToggle() { 
        mirrorY = !mirrorY;
        Render();
    }
    public void BazingaLToggle() {
        bazingaL = !bazingaL;
        Render();
    }
    public void BazingaRToggle() {
        bazingaR = !bazingaR;
        Render();
    }
    public void ColorToggle() {
        color = !color;
        Render();
    }
    public void BackGroundToggle() {
        backGround = !backGround;
        Render();
    }
    public void FlushToggle() {
        flush = !flush;
    }
    public void ThirdDemontionToggle() {
        thirdDemontion = !thirdDemontion;
    }
    public void GridSizeChange() {
        width = (int)slider.value;
        height = (int)slider.value;
        InitializeGrid();
        InitializeTexture();
        Render();
    }
    public void GameChange() {
        game = (int)gameSlider.value;
    }
    public void UpdateSizes() {
        foreach(Automata au in wanted) {
            foreach(Veriaition ve in au.veriaitions) {
                if (!sizes.Contains(ve.size)) {
                    sizes.Add(ve.size);
                }
            }
        }
    }
    private int RGrid(int x, int y) {
        if(x < width && y < height) {
            return grid[x, y];
        }else {
            return -1;
        }
    }
    private void WGrid(int x, int y, int value) {
        if (x < width && y < height) {
            grid[x, y] = value;
            if (!flush && value == 0) {
                gridTexture.SetPixel(x, y, deadColor);
            }
        }
    }
    private void ExportObj() {
        GameObject[] children = OBJExporter.GetAllChildren(parentFor3d);
        OBJExporter.Instance.ExportGameObjects(children);
    }

    public void ShuterSound() {
        audioSource.pitch = Random.Range(0.95f, 1f);
        audioSource.PlayOneShot(shutterSound);
        SpaceB();
    }

    public void ChangeTool(int index) {
        activeTool = (Tool)index;
    }
}