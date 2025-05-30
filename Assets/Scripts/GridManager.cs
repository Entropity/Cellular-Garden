using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System;

public class Slice {
    public List<int> slice = new List<int>();
}

public class Evolution {
    public List<Slice> slices = new List<Slice>();
    public Hash128 hash = new Hash128();
    public Vector2 size;

    public Vector2 Size() {
        if (slices == null || slices.Count == 0 || slices[0] == null) {
            Debug.Log("fuck");
            return Vector2.zero;
        }
        size = new Vector2(slices.Count, slices[0].slice.Count);
        return size;
    }
    public Sprite GenerateSprite(Color coloring) {
        // Validate automata data
        if (slices == null || slices.Count == 0 || slices[0].slice.Count == 0) {
            Debug.LogWarning("Invalid automata data.");
            return null;
        }

        // Get the size of the automata
        int width = slices.Count;
        int height = slices[0].slice.Count;

        // Create a new texture
        Texture2D texture = new Texture2D(width, height);

        // Fill the texture with data from the automata
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                // Get the value from the inner list (assumes values are 0 or 1)
                int value = slices[x].slice[y];

                // Set the base pixel color
                Color color = value == 1 ? coloring : Color.white;

                // Example condition: Modify the color for "even" cells.
                // Here "even" is defined as cells where (x+y) is even.
                if ((x + y) % 2 == 0) {
                    // To darken the color: multiply by a factor less than 1 (e.g., 0.9).
                    float darkenFactor = 0.97f;
                    color.r *= darkenFactor;
                    color.g *= darkenFactor;
                    color.b *= darkenFactor;

                    // Alternatively, if you want the color to be lighter on even cells, use a factor > 1.
                    // float lightenFactor = 1.1f;
                    // color.r = Mathf.Min(color.r * lightenFactor, 1f);
                    // color.g = Mathf.Min(color.g * lightenFactor, 1f);
                    // color.b = Mathf.Min(color.b * lightenFactor, 1f);
                }

                // Apply the pixel color
                texture.SetPixel(x, y, color);
            }
        }

        // Apply the texture changes
        texture.Apply();

        // Create and return a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        return sprite;

    }
}
[System.Serializable]
public class RuleSet {
    public int[] survivalRules;  // Neighbor counts for survival
    public int[] birthRules;        // Neighbor counts for birth
}

public class Automata {
    [SerializeField]
    public List<Evolution> evolutions = new List<Evolution>();
    public List<int> offsets = new List<int>();
    public Color color = Color.yellow;
    public bool active = true;
    public int gold = 0;
    public Automata() {

    }
    public Automata(int hardCodedAutomataIndex){
        if ( hardCodedAutomataIndex == 0) {
            evolutions.Add(new Evolution());
            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[0].slice.Add(0);
            evolutions[0].slices[0].slice.Add(1);
            evolutions[0].slices[0].slice.Add(0);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[1].slice.Add(0);
            evolutions[0].slices[1].slice.Add(1);
            evolutions[0].slices[1].slice.Add(0);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[2].slice.Add(0);
            evolutions[0].slices[2].slice.Add(1);
            evolutions[0].slices[2].slice.Add(0);

            evolutions[0].Size();
            color = new Color(171 / 255f, 255 / 255f, 79 / 255f);//green
            gold = 1;
            HashAutomata(evolutions[0]);
        }
        if (hardCodedAutomataIndex == 1) {
            evolutions.Add(new Evolution());
            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[0].slice.Add(0);
            evolutions[0].slices[0].slice.Add(1);
            evolutions[0].slices[0].slice.Add(0);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[1].slice.Add(1);
            evolutions[0].slices[1].slice.Add(1);
            evolutions[0].slices[1].slice.Add(1);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[2].slice.Add(0);
            evolutions[0].slices[2].slice.Add(1);
            evolutions[0].slices[2].slice.Add(0);

            evolutions[0].Size();
            color = new Color(4 / 255f, 119 / 255f, 59 / 255f);//was red now dark green
            HashAutomata(evolutions[0]);
        }

        if (hardCodedAutomataIndex == 2) {
            evolutions.Add(new Evolution());
            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[0].slice.Add(1);
            evolutions[0].slices[0].slice.Add(1);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[1].slice.Add(1);
            evolutions[0].slices[1].slice.Add(1);

            evolutions[0].Size();
            color = new Color(8 / 255f, 189 / 255f, 189 / 255f);// blue
            HashAutomata(evolutions[0]);
        }

        if (hardCodedAutomataIndex == 3) {
            evolutions.Add(new Evolution());
            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[0].slice.Add(1);
            evolutions[0].slices[0].slice.Add(1);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[1].slice.Add(1);
            evolutions[0].slices[1].slice.Add(0);

            evolutions[0].Size();
            color = Color.green;
            HashAutomata(evolutions[0]);
        }

        if (hardCodedAutomataIndex == 4) {
            evolutions.Add(new Evolution());
            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[0].slice.Add(1);
            evolutions[0].slices[0].slice.Add(0);
            evolutions[0].slices[0].slice.Add(0);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[1].slice.Add(0);
            evolutions[0].slices[1].slice.Add(1);
            evolutions[0].slices[1].slice.Add(0);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[2].slice.Add(0);
            evolutions[0].slices[2].slice.Add(0);
            evolutions[0].slices[2].slice.Add(1);

            evolutions[0].Size();
            color = Color.green;
            HashAutomata(evolutions[0]);
        }

        if (hardCodedAutomataIndex == 5) {
            evolutions.Add(new Evolution());
            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[0].slice.Add(0);
            evolutions[0].slices[0].slice.Add(1);
            evolutions[0].slices[0].slice.Add(0);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[1].slice.Add(0);
            evolutions[0].slices[1].slice.Add(1);
            evolutions[0].slices[1].slice.Add(1);

            evolutions[0].slices.Add(new Slice());
            evolutions[0].slices[2].slice.Add(0);
            evolutions[0].slices[2].slice.Add(0);
            evolutions[0].slices[2].slice.Add(0);

            evolutions[0].Size();
            color = new Color(0xF7 / 255f, 0x55 / 255f, 0x90 / 255f);
            HashAutomata(evolutions[0]);
        }

        GenerateVariants();
    }
    public void GenerateVariants() {
        Evolution original = evolutions[0];

        for (int i = 0; i < 4; i++) // Four rotations (0°, 90°, 180°, 270°)
        {
            Evolution rotated = RotateAutomata(original, i);
            rotated.Size();
            Evolution horizontalMirror = MirrorAutomata(rotated, true);
            horizontalMirror.Size();
            Evolution verticalMirror = MirrorAutomata(rotated, false);
            verticalMirror.Size();

            HashAutomata(rotated);
            HashAutomata(horizontalMirror);
            HashAutomata(verticalMirror);

            if (!CheckHash(rotated)) {
                evolutions.Add(rotated);
            }
            if (!CheckHash(horizontalMirror)) {
                evolutions.Add(horizontalMirror);
            }
            if (!CheckHash(verticalMirror)) {
                evolutions.Add(verticalMirror);
            }
        }
    }

    private Evolution RotateAutomata(Evolution input, int rotation) {
        Evolution result = new Evolution();
        int rows = input.slices.Count;
        int cols = input.slices[0].slice.Count;

        for (int i = 0; i < cols; i++) {
            Slice newRow = new Slice();

            for (int j = 0; j < rows; j++) {
                if (rotation == 1) // Rotate 90° clockwise
                    newRow.slice.Add(input.slices[rows - j - 1].slice[i]);
                else if (rotation == 2) // Rotate 180°
                    newRow.slice.Add(input.slices[rows - i - 1].slice[cols - j - 1]);
                else if (rotation == 3) // Rotate 270° clockwise
                    newRow.slice.Add(input.slices[j].slice[cols - i - 1]);
                else // 0° (original)
                    newRow.slice.Add(input.slices[i].slice[j]);
            }

            result.slices.Add(newRow);
        }

        return result;
    }

    private Evolution MirrorAutomata(Evolution input, bool horizontal) {
        Evolution result = new Evolution();

        foreach (var row in input.slices) {
            Slice newRow = new Slice();

            if (horizontal) {
                for (int i = row.slice.Count - 1; i >= 0; i--) {
                    newRow.slice.Add(row.slice[i]);
                }
            } else {
                newRow.slice.AddRange(row.slice);
            }

            result.slices.Add(newRow);
        }

        if (!horizontal) // For vertical mirror, reverse the entire pattern's rows
        {
            result.slices.Reverse();
        }

        return result;
    }

    public void HashAutomata(Evolution eve) {
        for (int i = 0; i < eve.slices.Count; i++) {
            for (int j = 0; j < eve.slices[i].slice.Count; j++) {
                eve.hash.Append(eve.slices[i].slice[j]);
            }
        }
    }
    public bool CheckHash(Evolution eve) {
        foreach (Evolution eves in evolutions) {
            if(eve.hash == eves.hash) {
                return true;
            }
        }
        return false;
    }
}

public enum Tool {
    pen, selector, shapes
}
public enum Shape {
    circle, line//, triangle
}

public class GridManager : MonoBehaviour {
    #region Grid Settings
    [Header("Grid Settings")]
    public int width = 50;
    public int height = 50;
    public int rule = 0;
    public int z = 1;
    public float cellSize = 1f;
    public bool wrapEdges = true;
    public bool bazingaL = true;
    public bool bazingaR = true;
    public bool mirrorX = true;
    public bool mirrorY = true;
    public bool coloring = true;

    // Internal grid state
    private bool layer2Active = false;
    public bool secondClick = false;
    private bool backGround = true;
    private bool flush = true;
    public bool voxelRendering = false;
    public bool highPriority = true;
    private Vector2Int firstClick = new Vector2Int();
    private Vector2Int penPos;
    #endregion

    #region Color Settings
    [Header("Colors")]
    public Color aliveColor = Color.black;
    public Color deadColor = Color.white;
    public Color chesswhite;
    public Color chessBlack;
    public Color mirrorColor;
    public Color temp;
    public Color penShadow;
    #endregion

    #region Voxel & Visual Settings
    [Header("Voxel & Visual Settings")]
    public GameObject voxel;
    public GameObject voxels;
    public GameObject voxelsRotationAxis;

    private Coroutine highlightCoroutine = null;
    public float colorUpdateInterval = 1f; // Interval for low-priority updates

    public Renderer textureRenderer;
    public Material defaultVoxelMaterial;

    public float rotationSpeed = 100f;
    private Vector3 lastMousePosition;
    #endregion

    #region Tool & Input Settings
    [Header("Tool & Input Settings")]
    private Tool activeTool = Tool.pen;
    public Slider slider;
    public Slider gameSlider;
    #endregion

    #region Shapes
    [Header("Shapes")]
    private Shape activeShape = Shape.circle;
    private bool shapeSymmetry = true;
    #endregion

    #region Game Settings
    [Header("Game")]
    public int gold = 0;
    public TMP_Text goldText;
    #endregion

    #region Interaction Settings
    [Header("Interaction")]
    public bool drawEnabled = true;
    public int brushSize = 1;
    public bool eraseMode = false;
    #endregion

    #region Simulation Settings
    [Header("Simulation Settings")]
    [Range(0.001f, 1f)]
    public float updateInterval = 0.01f;
    public bool simulate = false;
    #endregion

    #region Rules
    [Header("Rules")]
    public List<RuleSet> ruleSet = new List<RuleSet>();
    #endregion

    #region Grid Data & Textures
    private int[,] grid;
    private List<int[,]> timeLine = new List<int[,]>();
    public List<Automata> coloredAutomatas = new List<Automata>();
    public List<Vector2> sizes = new List<Vector2>();
    private int[,] newGrid;
    private Texture2D gridTexture;
    private Texture2D layer2Texture;
    public GameObject layer2;
    private float timer = 0f;
    #endregion

    #region Audio Settings
    public AudioSource audioSource;
    public AudioClip shutterSound;
    #endregion

    #region UI Settings
    // UI Elements
    public GameObject CardFab;
    public RectTransform content; // Reference to ScrollView's Content
    public List<GameObject> cards; // List of card objects
    #endregion
    void Start() {
        InitializeGrid();
        InitializeTexture();
        InitializeSecondLayerTexture();
        //wanted.Add(new Automata(3));
        
        coloredAutomatas.Add(new Automata(0));
        coloredAutomatas.Add(new Automata(1));
        coloredAutomatas.Add(new Automata(2));
        coloredAutomatas.Add(new Automata(4));
        coloredAutomatas.Add(new Automata(5));
        UpdateSizes();
        voxels.transform.SetParent(voxelsRotationAxis.transform);
        gameSlider.maxValue = ruleSet.Count - 1;
        Render();
        MakeCards();
    }

    void Update() {
        if (simulate && !layer2Active) {
            timer += Time.deltaTime;
            if (timer >= updateInterval) {
                timer = 0f;
                SimulateStep();
                Render();
                Voxelate();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            SpaceB();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            simulate = !simulate;
            SetHighPriority(simulate);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ClearGrid();
            GoBroke();
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            Voxelate();
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            ExportObj();
        }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
            if( Input.GetKeyDown(KeyCode.S)){
                Debug.Log("Ctrl + S Pressed!");
                SaveGrid();
            }
            else if (Input.GetKeyDown(KeyCode.Z)) {
                Debug.Log("Ctrl + Z Pressed!");
                LoadGrid();
            }
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            InvertGrid();
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            Discover();
        }
        //RotateObject();
        RotateObjectWithMouse();
        HandleInput();
        SecondLayer();
    }

    void MakeCards() {
        float newWidth = content.parent.parent.GetComponent<RectTransform>().sizeDelta.x;
        float newHeight = newWidth - 180 + 60;

        foreach (Transform child in content) {
            child.gameObject.SetActive(false); // Disable the child for reuse
        }
        cards = new List<GameObject>();
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, (coloredAutomatas.Count+1) * newHeight);
        for (int i = 0; i < coloredAutomatas.Count; i++) {
            int index = i; // Create a local copy of `i`

            cards.Add(Instantiate(CardFab));
            cards[i].transform.SetParent(content);
            RectTransform rectTransform = cards[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, (coloredAutomatas.Count -1 - i) * -newHeight + content.GetComponent<RectTransform>().sizeDelta.y / 2 - (newHeight/2));
            rectTransform.sizeDelta = new Vector2(newWidth, newHeight);

            Sprite sprite = coloredAutomatas[i].evolutions[0].GenerateSprite(coloredAutomatas[i].color);
            cards[i].transform.GetChild(1).GetComponent<Image>().sprite = sprite;
            cards[i].transform.GetChild(1).GetComponent<Image>().sprite.texture.filterMode = FilterMode.Point;
            cards[i].transform.GetChild(2).GetComponent<TMP_Text>().text = "1/" + coloredAutomatas[i].evolutions.Count.ToString();

            cards[i].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => MoveColoredAutmataLower(index));
            cards[i].transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => MoveColoredAutmataHigher(index));
            cards[i].transform.GetChild(5).GetComponent<Toggle>().isOn = coloredAutomatas[index].active;
            cards[i].transform.GetChild(5).GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) =>
            {
                ToggleAutomata(index);
            });
        }
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
    public void MoveColoredAutmataHigher(int index) {
        // Ensure the object is not already at the beginning
        if (index > 0) {
            // Swap with the previous element
            print(index);
            var temp = coloredAutomatas[index];
            coloredAutomatas[index] = coloredAutomatas[index - 1];
            coloredAutomatas[index - 1] = temp;
        }
        MakeCards();
        Render(false);
    }
    public void ToggleAutomata(int index) {
        coloredAutomatas[index].active = !coloredAutomatas[index].active;
        Render(false);
    }
    public void MoveColoredAutmataLower(int index) {
        // Ensure the object is not already at the end
        if (index < coloredAutomatas.Count - 1) {
            // Swap with the next element
            print(index);
            var temp = coloredAutomatas[index];
            coloredAutomatas[index] = coloredAutomatas[index + 1];
            coloredAutomatas[index + 1] = temp;
        }
        MakeCards();
        Render(false);
    }
    public static Color CuteColor() {
        // Generate a random hue between 0 and 1.
        float hue = UnityEngine.Random.value;

        // Keep saturation and value high (0.8 to 1 for vivid colors).
        float saturation = UnityEngine.Random.Range(0.8f, 1f);
        float value = UnityEngine.Random.Range(0.8f, 1f);

        // Convert HSV to RGB and return it.
        return Color.HSVToRGB(hue, saturation, value);
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
            return System.Array.Exists(ruleSet[rule].survivalRules, n => n == neighbors) ? 1 : 0;
        } else {
            return System.Array.Exists(ruleSet[rule].birthRules, n => n == neighbors) ? 1 : 0;
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
    void DrawAliveCells() {
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
                DrawSymmetry(value, x, y, true);
            }
        }
    }
    private void SpaceB() {
        if (simulate) {
            simulate = false;
        }
        SimulateStep();
        Render(false);
        SetHighPriority(false);
    }
    private void SecondLayer() {
        ShowPenPos();
        if (activeTool == Tool.pen) {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (Input.GetMouseButtonDown(0)) {
            HandleLayer2MouseClick(1);
            layer2Texture.Apply();
        }
        if (Input.GetMouseButtonDown(1)) {
            HandleLayer2MouseClick(0);
            layer2Texture.Apply();
        }
        if (secondClick) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    layer2Texture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            int shapeCount = Enum.GetValues(typeof(Shape)).Length;

            if (scroll > 0) {
                activeShape++;
            } else if (scroll < 0) {
                activeShape--;
            }

            activeShape = (Shape)(((int)activeShape + shapeCount) % shapeCount);

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

                    int width = maxX - minX;
                    int height = maxY - minY;

                    if(activeTool == Tool.selector) {
                        // Create a color array to hold pixel data
                        Color[] colors = new Color[(maxX - minX + 1) * (maxY - minY + 1)];

                        for (int j = minY, index = 0; j <= maxY; j++) {
                            for (int i = minX; i <= maxX; i++, index++) {
                                if (i == minX || i == maxX || j == minY || j == maxY) {
                                    colors[index] = new Color(0, 0, 0, 0.9f); // Border color
                                } else {
                                    if (width == height) {
                                        colors[index] = new Color(0, 1, 0, 0.5f); // Inner color
                                    } else {
                                        colors[index] = new Color(1, 0, 0, 0.5f); // Inner color
                                    }

                                }
                            }
                        }
                        layer2Texture.SetPixels(minX, minY, maxX - minX + 1, maxY - minY + 1, colors);
                    }
                   
                    if(activeTool == Tool.shapes) {
                        if (activeShape == Shape.circle) {
                            //circle
                            int centerX = (minX + maxX) / 2;
                            int centerY = (minY + maxY) / 2;
                            int radius = Mathf.Max(Mathf.Abs(x - centerX), Mathf.Abs(y - centerY));

                            for (int i = centerX - radius; i <= centerX + radius; i++) {
                                for (int j = centerY - radius; j <= centerY + radius; j++) {
                                    float distanceSquared = Mathf.Pow(i - centerX, 2) + Mathf.Pow(j - centerY, 2);

                                    if (distanceSquared <= Mathf.Pow(radius, 2)) {
                                        layer2Texture.SetPixel(i, j, new Color(1, 0, 0, 0.4f));
                                    }
                                }
                            }

                            // Calculate horizontal and vertical radii based on the second point.
                            int radiusX = Mathf.Abs(x - centerX);
                            int radiusY = Mathf.Abs(y - centerY);

                            // It's wise to check that neither radius is zero.
                            if (radiusX == 0 || radiusY == 0) {
                                return;
                            }

                            for (int i = centerX - radiusX; i <= centerX + radiusX; i++) {
                                for (int j = centerY - radiusY; j <= centerY + radiusY; j++) {
                                    // Normalize the pixel's distance from the center in both dimensions.
                                    float normalizedX = Mathf.Pow((i - centerX) / (float)radiusX, 2);
                                    float normalizedY = Mathf.Pow((j - centerY) / (float)radiusY, 2);

                                    // Check if the point is inside the ellipse.
                                    if (normalizedX + normalizedY <= 1) {
                                        if (shapeSymmetry) {
                                            DrawLayer2Symmetry(i, j, new Color(0, 1, 0, 0.8f));
                                        } else {
                                            layer2Texture.SetPixel(i, j, new Color(0, 1, 0, 0.8f));
                                        }
                                        
                                    }
                                }
                            }
                        }
                        // Apply the color array to the texture

                        if (activeShape == Shape.line) {
                            DrawLine(firstClick.x, firstClick.y, x, y, layer2Texture, Color.black);
                        }

                        //if (activeShape == Shape.triangle) {
                            //DrawTriangle(firstClick.x, firstClick.y, x, y, layer2Texture, Color.blue);
                        //}
                    }

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
                if (penPos.x != x || penPos.y != y) {
                    layer2Texture.SetPixel(x, y, penShadow);
                    penPos = new Vector2Int(x, y);
                    layer2Texture.Apply();
                }
            }
        }
    }
    void HandleLayer2MouseClick(int value) {
        if(value == 0) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    layer2Texture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
            secondClick = false;
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
                if (!secondClick) {
                    layer2Texture.SetPixel(x, y, aliveColor);
                    firstClick = new Vector2Int(x, y);
                    secondClick = true;
                } else {
                    int minX = Mathf.Min(firstClick.x, x);
                    int maxX = Mathf.Max(firstClick.x, x);
                    int minY = Mathf.Min(firstClick.y, y);
                    int maxY = Mathf.Max(firstClick.y, y);

                    if(activeTool == Tool.selector) {
                        if (maxX - minX == maxY - minY) {
                            Automata auto = new Automata();
                            auto.evolutions.Add(new Evolution());
                            int shit = 0;
                            for (int i = minX + 1; i <= maxX - 1; i++) {
                                auto.evolutions[0].slices.Add(new Slice());
                                for (int j = minY + 1; j <= maxY - 1; j++) {
                                    int cellValue = RGrid(i, j);
                                    auto.evolutions[0].slices[shit].slice.Add(cellValue);
                                    print(value);
                                }
                                shit++;
                            }
                            auto.evolutions[0].Size();
                            auto.HashAutomata(auto.evolutions[0]);
                            auto.color = CuteColor();
                            auto.GenerateVariants();
                            coloredAutomatas.Add(auto);
                            UpdateSizes();
                            MakeCards();
                        }
                    }

                    if(activeTool == Tool.shapes) {
                        if (activeShape == Shape.circle) {
                            //circle
                            int centerX = (minX + maxX) / 2;
                            int centerY = (minY + maxY) / 2;
                            int radius = Mathf.Max(Mathf.Abs(x - centerX), Mathf.Abs(y - centerY));

                            for (int i = centerX - radius; i <= centerX + radius; i++) {
                                for (int j = centerY - radius; j <= centerY + radius; j++) {
                                    float distanceSquared = Mathf.Pow(i - centerX, 2) + Mathf.Pow(j - centerY, 2);

                                    if (distanceSquared <= Mathf.Pow(radius, 2)) {
                                        //layer2Texture.SetPixel(i, j, new Color(1, 0, 0, 0.6f));
                                    }
                                }
                            }

                            // Calculate horizontal and vertical radii based on the second point.
                            int radiusX = Mathf.Abs(x - centerX);
                            int radiusY = Mathf.Abs(y - centerY);

                            // It's wise to check that neither radius is zero.
                            if (radiusX == 0 || radiusY == 0) {
                                return;
                            }

                            for (int i = centerX - radiusX; i <= centerX + radiusX; i++) {
                                for (int j = centerY - radiusY; j <= centerY + radiusY; j++) {
                                    // Normalize the pixel's distance from the center in both dimensions.
                                    float normalizedX = Mathf.Pow((i - centerX) / (float)radiusX, 2);
                                    float normalizedY = Mathf.Pow((j - centerY) / (float)radiusY, 2);

                                    // Check if the point is inside the ellipse.
                                    if (normalizedX + normalizedY <= 1) {
                                        if (shapeSymmetry) {
                                            DrawSymmetry(1, i, j, false);
                                        } else {
                                            WGrid(i, j, 1);
                                        }                                        
                                    }
                                }
                            }
                        }

                        if (activeShape == Shape.line) {
                            DrawLine(firstClick.x, firstClick.y, x, y);
                        }

                        //if(activeShape == Shape.triangle) {
                            //DrawTriangle(firstClick.x, firstClick.y, x, y);
                        //}
                    }
                    
                    secondClick = false;
                    FlushLayerTwo();
                    Render(false);
                }
            }
        }
    }
    void DrawLine(int x0, int y0, int x1, int y1, Texture2D texture, Color color) {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);

        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;

        int err = dx - dy;

        while (true) {
            if (shapeSymmetry) {
                DrawLayer2Symmetry(x0, y0, color);
            } else {
                texture.SetPixel(x0, y0, color);
            }

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;

            if (e2 > -dy) {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx) {
                err += dx;
                y0 += sy;
            }
        }
    }
    void DrawLine(int x0, int y0, int x1, int y1) {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);

        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;

        int err = dx - dy;

        while (true) {
            if (shapeSymmetry) {
                DrawSymmetry(1, x0, y0, false);
            } else {
                WGrid(x0, y0, 1);
            }

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;

            if (e2 > -dy) {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx) {
                err += dx;
                y0 += sy;
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
                foreach (Hash128 hash in HashAllSizes(y, x)) { // ALSO HASHES THE NONE ACTIVE AUTOMATAS SO ITS SLOW AS FUCK MAKE IT RIGHT
                    hashGrid[x, y].Add(hash);
                }
            }
        }

        foreach (Automata au in coloredAutomatas) {
            foreach (Evolution ve in au.evolutions) {
                if(au.active == false) {
                    break;
                }
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        if (hashGrid[x, y].Contains(ve.hash)) {
                            GainGold(au);
                            ColorAutomata(y, x, ve, au); 
                        }
                    }
                }
            }
        }
    }
    public void HighlightAutomataFastButNotWorking() {
        // Allocate and initialize your grid in parallel.
        var hashGrid = new HashSet<Hash128>[width, height];

        // Parallel initialization of each cell.
        Parallel.For(0, width, x =>
        {
            for (int y = 0; y < height; y++) {
                hashGrid[x, y] = new HashSet<Hash128>();
            }
        });

        // Populate the grid. Since each (x,y) cell is independent, using Parallel.For helps speed this up.
        Parallel.For(0, width, x =>
        {
            for (int y = 0; y < height; y++) {
                foreach (Hash128 hash in HashAllSizes(y, x)) {
                    // No concurrency issue here since each thread writes to its own hashGrid[x, y]
                    hashGrid[x, y].Add(hash);
                }
            }
        });

        // Process automata concurrently.
        // Note: if GainGold and ColorAutomata change shared state,
        // ensure that they are thread safe. For instance, you might use locks.
        Parallel.ForEach(coloredAutomatas, automata =>
        {
            foreach (Evolution evolution in automata.evolutions) {
                // Parallelize grid cell processing for each evolution.
                Parallel.For(0, width, x =>
                {
                    for (int y = 0; y < height; y++) {
                        if (hashGrid[x, y].Contains(evolution.hash)) {
                            // If GainGold (or ColorAutomata) modify automata's internal data,
                            // consider locking. Replace 'automataLock' with an appropriate lock object.
                            lock (automata) {
                                GainGold(automata);
                            }
                            ColorAutomata(y, x, evolution, automata);
                        }
                    }
                });
            }
        });
    }
    private void GainGold(Automata auto) {
        gold += auto.gold;
        goldText.text = "Gold: " + gold.ToString();
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
    private void DrawSymmetry(int value, int x, int y, bool render) {
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
            if (render) {
                Render(false);
            }
        }
    }
    private void DrawLayer2Symmetry(int x, int y, Color color) {
        int midX = width / 2;
        int midY = height / 2;
        int disX = midX - x;
        int disY = midY - y;

        if (mirrorX) {
            layer2Texture.SetPixel(midX - disX, midY + disY, color);
            layer2Texture.SetPixel(midX - disX, midY - disY, color);

            if ((bazingaL && bazingaR)) {
                layer2Texture.SetPixel(midX + disX, midY - disY, color);
            }
        }
        if (mirrorY) {
            layer2Texture.SetPixel(midX + disX, midY - disY, color);
            layer2Texture.SetPixel(midX - disX, midY - disY, color);

            if ((bazingaL && bazingaR)) {
                layer2Texture.SetPixel(midX - disX, midY + disY, color);
            }
        }
        if (bazingaR) {
            layer2Texture.SetPixel(midY - disY, midX - disX, color);
            layer2Texture.SetPixel(x, y, color);

            if ((mirrorX && mirrorY)) {
                layer2Texture.SetPixel(midY + disY, midX + disX, color);
            }
        }
        if (bazingaL) {
            layer2Texture.SetPixel(midY + disY, midX + disX, color);
            layer2Texture.SetPixel(x, y, color);

            if ((mirrorX && mirrorY)) {
                layer2Texture.SetPixel(midY - disY, midX - disX, color);
            }
        }
        if ((bazingaR && bazingaL) && (mirrorX && mirrorY)) {
            layer2Texture.SetPixel(midY + disY, midX - disX, color);
            layer2Texture.SetPixel(midY - disY, midX + disX, color);
        }
        if ((bazingaR && bazingaL) || (mirrorX && mirrorY)) {
            layer2Texture.SetPixel(midX + disX, midY + disY, color);
        } else if (!mirrorX && !mirrorY && !bazingaL && !bazingaR) {
            layer2Texture.SetPixel(x, y, color);
        }
    }
    private bool CheckAutomata(int x, int y, int veriaition, int a) {
        for (int i = 0; i < coloredAutomatas[a].evolutions[veriaition].slices.Count; i++) {
            for (int j = 0; j < coloredAutomatas[a].evolutions[veriaition].slices[i].slice.Count; j++) {
                // Swap i and j if the glider appears rotated
                if (RGrid(x + j, y + i) != coloredAutomatas[a].evolutions[veriaition].slices[i].slice[j]) {
                    return false;
                }
            }
        }
        return true;
    }
    private void ColorAutomata(int x, int y, Evolution veriaition, Automata automata) {
        for (int i = 0; i < veriaition.size.x; i++) {
            for (int j = 0; j < veriaition.size.y; j++) {
                if (veriaition.slices[i].slice[j] == 1) {
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
        DrawAliveCells();
        if (coloring) {
            if (highPriority) {
                // If a low-priority coroutine is pending, cancel it
                if (highlightCoroutine != null) {
                    StopCoroutine(highlightCoroutine);
                    highlightCoroutine = null;
                }
                HighlightAutomata();
            } else {
                // Only start one coroutine if it's not already running
                if (highlightCoroutine == null) {
                    print("new core");
                    highlightCoroutine = StartCoroutine(DelayedHighlight());
                }
            }
        }

        gridTexture.Apply();
    }
    private void Render(bool pen) {
        if (flush && !pen) {
            Flush();
        }
        if (backGround && flush && !pen) {
            ChessBoard();
            MirrorAxis();
        }
        if (!pen) {
            DrawAliveCells();
        }
        if (coloring) {
            if (highPriority || !pen) {
                // If a low-priority coroutine is pending, cancel it
                if (highlightCoroutine != null) {
                    StopCoroutine(highlightCoroutine);
                    highlightCoroutine = null;
                }
                HighlightAutomata();
            } else {
                // Only start one coroutine if it's not already running
                if (highlightCoroutine == null) {
                    highlightCoroutine = StartCoroutine(DelayedHighlight());
                }
            }
        }

        gridTexture.Apply();
    }
    private IEnumerator DelayedHighlight() {
        yield return new WaitForSeconds(colorUpdateInterval);
        HighlightAutomata();
        highlightCoroutine = null; // Reset reference so it can be re-started next time if needed
        gridTexture.Apply();
    }
    public void SetHighPriority(bool isHighPriority) {
        highPriority = isHighPriority;
    }
    private void Flush() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                gridTexture.SetPixel(x, y, deadColor);
            }
        }
    }
    private bool Voxelate() {
        if (!voxelRendering) {
            return false;
        }

        // First, build an array of valid voxels and store their color.
        bool[,] validVoxels = new bool[width, height];
        Color[,] voxelColors = new Color[width, height];
        bool change = false;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Color color = gridTexture.GetPixel(x, y);
                // Skip if this pixel is one of the chosen “do not render” colors.
                if (color == chessBlack || color == chesswhite || color == deadColor || AreColorsEqual(color, mirrorColor)) {
                    validVoxels[x, y] = false;
                } else {
                    validVoxels[x, y] = true;
                    voxelColors[x, y] = color;
                    change = true;
                }
            }
        }

        // Determine the scale for this layer – we assume cubes should fill the width.
        float scale = 1f / width;

        // Prepare lists to build the mesh.
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

        // Loop through the layer and add cube (or partial cube) geometry for active voxels.
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (!validVoxels[x, y])
                    continue;

                Color col = voxelColors[x, y];
                // Calculate the “center” position for this voxel.
                Vector3 voxelPosition = new Vector3(
                    (x - width / 2f) * scale,
                    (y - height / 2f) * scale,
                    z * scale);

                // Add geometry for this cube into our lists.
                AddCubeMesh(voxelPosition, scale, col, validVoxels, x, y, vertices, triangles, colors);
            }
        }

        // Only create a mesh if at least one voxel was added.
        if (vertices.Count > 0) {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.colors = colors.ToArray();
            mesh.RecalculateNormals();

            // Create one GameObject for this layer
            GameObject layerObj = new GameObject("VoxelLayer_" + z);
            layerObj.transform.SetParent(voxels.transform, false);
            // Attach mesh components
            MeshFilter filter = layerObj.AddComponent<MeshFilter>();
            MeshRenderer renderer = layerObj.AddComponent<MeshRenderer>();
            filter.mesh = mesh;
            renderer.material = defaultVoxelMaterial;
        }

        // Increment z if any voxel has been rendered.
        if (change) {
            z += 1;
        }

        return true;
    }
    private void AddCubeMesh(Vector3 pos, float scale, Color col, bool[,] validVoxels, int x, int y,
                             List<Vector3> vertices, List<int> triangles, List<Color> colors) {
        // The cube size is 'scale', so half of that:
        float half = scale / 2f;

        // Define the 8 corners of the cube.
        // p0: left-bottom-front; p1: right-bottom-front; p2: right-top-front; p3: left-top-front;
        // p4: left-bottom-back;  p5: right-bottom-back;  p6: right-top-back;  p7: left-top-back.
        Vector3 p0 = pos + new Vector3(-half, -half, half);
        Vector3 p1 = pos + new Vector3(half, -half, half);
        Vector3 p2 = pos + new Vector3(half, half, half);
        Vector3 p3 = pos + new Vector3(-half, half, half);
        Vector3 p4 = pos + new Vector3(-half, -half, -half);
        Vector3 p5 = pos + new Vector3(half, -half, -half);
        Vector3 p6 = pos + new Vector3(half, half, -half);
        Vector3 p7 = pos + new Vector3(-half, half, -half);

        // Check adjacent voxels horizontally and vertically (in this 2D layer).
        // If a neighbor exists then that face of the cube (shared with that neighbor) is hidden.
        bool addLeft = (x == 0 || !validVoxels[x - 1, y]);
        bool addRight = (x == validVoxels.GetLength(0) - 1 || !validVoxels[x + 1, y]);
        bool addBottom = (y == 0 || !validVoxels[x, y - 1]);
        bool addTop = (y == validVoxels.GetLength(1) - 1 || !validVoxels[x, y + 1]);

        // For the depth (z), we assume that this method only builds one slice, so both front and back are drawn.
        bool addFront = true;
        bool addBack = true;

        int startIndex = vertices.Count;

        // Add Front face (p0, p1, p2, p3)
        if (addFront) {
            vertices.Add(p0); vertices.Add(p1); vertices.Add(p2); vertices.Add(p3);
            colors.Add(col); colors.Add(col); colors.Add(col); colors.Add(col);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 3);
            startIndex += 4;
        }

        // Add Back face (p5, p4, p7, p6)
        if (addBack) {
            vertices.Add(p5); vertices.Add(p4); vertices.Add(p7); vertices.Add(p6);
            colors.Add(col); colors.Add(col); colors.Add(col); colors.Add(col);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 3);
            startIndex += 4;
        }

        // Add Left face (p4, p0, p3, p7)
        if (addLeft) {
            vertices.Add(p4); vertices.Add(p0); vertices.Add(p3); vertices.Add(p7);
            colors.Add(col); colors.Add(col); colors.Add(col); colors.Add(col);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 3);
            startIndex += 4;
        }

        // Add Right face (p1, p5, p6, p2)
        if (addRight) {
            vertices.Add(p1); vertices.Add(p5); vertices.Add(p6); vertices.Add(p2);
            colors.Add(col); colors.Add(col); colors.Add(col); colors.Add(col);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 3);
            startIndex += 4;
        }

        // Add Top face (p3, p2, p6, p7)
        if (addTop) {
            vertices.Add(p3); vertices.Add(p2); vertices.Add(p6); vertices.Add(p7);
            colors.Add(col); colors.Add(col); colors.Add(col); colors.Add(col);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 3);
            startIndex += 4;
        }

        // Add Bottom face (p4, p5, p1, p0)
        if (addBottom) {
            vertices.Add(p4); vertices.Add(p5); vertices.Add(p1); vertices.Add(p0);
            colors.Add(col); colors.Add(col); colors.Add(col); colors.Add(col);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 3);
        }
    }
    private void RotateObject() {
        float rotationSpeed = 200f;

        if (Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKey(KeyCode.J)) {
                voxelsRotationAxis.transform.rotation = Quaternion.Euler(0, 90, 0);
            } else if (Input.GetKey(KeyCode.K)) {
                voxelsRotationAxis.transform.rotation = Quaternion.Euler(0, 180, 0);
            } else if (Input.GetKey(KeyCode.L)) {
                voxelsRotationAxis.transform.rotation = Quaternion.Euler(0, -90, 0);
            } else if (Input.GetKey(KeyCode.I)) {
                voxelsRotationAxis.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        } else {
            if (Input.GetKey(KeyCode.J)) {
                voxelsRotationAxis.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); //Z-axis (counterclockwise)
            }
            if (Input.GetKey(KeyCode.K)) {
                voxelsRotationAxis.transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime); //Z-axis (clockwise)
            }
            if (Input.GetKey(KeyCode.L)) {
                voxelsRotationAxis.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); //Y-axis
            }
            if (Input.GetKey(KeyCode.I)) {
                voxelsRotationAxis.transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime); //Y-axis (opposite direction)
            }
        }
    }

    private void RotateObjectWithMouse() {
        if (!voxelRendering)
            return;

        // When the drag starts, record the initial mouse position.
        if (Input.GetMouseButtonDown(0)) {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            // Find the mouse delta since the last frame.
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Calculate rotation amounts.
            float rotationY = -delta.x * rotationSpeed * Time.deltaTime;
            float rotationX = delta.y * rotationSpeed * Time.deltaTime;

            // Create quaternion rotations
            Quaternion rotationDeltaY = Quaternion.AngleAxis(rotationY, Vector3.up);
            Quaternion rotationDeltaX = Quaternion.AngleAxis(rotationX, Vector3.right);

            // Apply rotations using quaternion multiplication
            voxelsRotationAxis.transform.rotation = rotationDeltaY * voxelsRotationAxis.transform.rotation;
            voxelsRotationAxis.transform.rotation = voxelsRotationAxis.transform.rotation * rotationDeltaX;

            // Update the last mouse position.
            lastMousePosition = Input.mousePosition;
        }
    }
    bool AreColorsEqual(Color a, Color b, float tolerance = 0.01f) {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance &&
               Mathf.Abs(a.a - b.a) < tolerance;
    }
    public void ShapeSymmetryToggle() {
        shapeSymmetry = !shapeSymmetry;
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
        coloring = !coloring;
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
        voxelRendering = !voxelRendering;
    }
    public void GridSizeChange() {
        width = NearestEven((int)slider.value);
        height = NearestEven((int)slider.value);
        InitializeGrid();
        InitializeTexture();
        InitializeSecondLayerTexture();
        Render();
    }
    private int NearestEven(int num) {
        if(num % 2 == 0) {
            return num;
        }
        return num + 1;
    }
    public void GameChange() {
        rule = (int)gameSlider.value;
    }
    public void UpdateSizes() {
        foreach(Automata au in coloredAutomatas) {
            foreach(Evolution ve in au.evolutions) {
                if (!sizes.Contains(ve.size)) {
                    sizes.Add(ve.size);
                }
            }
        }
    }
    private int RGrid(int x, int y) {
        if(0 < x && x < width && 0 < y && y < height) {
            return grid[x, y];
        }else {
            return -1;
        }
    }
    private void GoBroke() {
        gold = 0;
        goldText.text = "Gold: 0";
    }
    private void WGrid(int x, int y, int value) {
        if (0 < x && x < width && 0 < y && y < height) {
            grid[x, y] = value;
            if (value == 0) {
                gridTexture.SetPixel(x, y, deadColor);
            } else {
                gridTexture.SetPixel(x, y, aliveColor);
            }
        }
        //gridTexture.Apply();
    }
    private void ExportObj() {
        GameObject[] children = OBJExporter.GetAllChildren(voxels);
        OBJExporter.Instance.ExportGameObjects(children);
    }
    public void ShuterSound() {
        audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
        audioSource.PlayOneShot(shutterSound);
        SpaceB();
    }
    public void ChangeTool(int index) {
        activeTool = (Tool)index;
    }
    public void RandomizeHighLightColors() {
        foreach(Automata automata in coloredAutomatas) {
            automata.color = CuteColor();
            MakeCards();
            Render();
        }
    }
    public void ScreenShot() {
        if (backGround) {
            backGround = false;
            Render(false);
            SaveTextureToPNG(gridTexture);
            backGround = true;
            Render(false);
        } else {
            SaveTextureToPNG(gridTexture);
        }
    }
    public static void SaveTextureToPNG(Texture2D texture) {
        if (texture == null) {
            Debug.LogError("Texture is null!");
            return;
        }

        // Convert texture to PNG
        Texture2D upscaledTexture = UpscaleTexture(texture, 4);
        byte[] pngData = upscaledTexture.EncodeToPNG();
        if (pngData == null) {
            Debug.LogError("Failed to encode texture to PNG!");
            return;
        }

        // Generate filename with current time and date to seconds
        string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string directoryPath = Directory.GetParent(Application.dataPath).FullName + "/ScreenShots";

        string filePath = Path.Combine(directoryPath, fileName);

        // Ensure directory exists
        if (!Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
        }

        // Write PNG file
        File.WriteAllBytes(filePath, pngData);
        Debug.Log($"Texture saved to: {filePath}");
    }
    private static Texture2D UpscaleTexture(Texture2D original, int scaleFactor) {
        int newWidth = original.width * scaleFactor;
        int newHeight = original.height * scaleFactor;
        Texture2D newTexture = new Texture2D(newWidth, newHeight, original.format, false);

        for (int x = 0; x < newWidth; x++) {
            for (int y = 0; y < newHeight; y++) {
                // Find the nearest pixel in the original texture
                Color pixelColor = original.GetPixel(x / scaleFactor, y / scaleFactor);
                newTexture.SetPixel(x, y, pixelColor);
            }
        }

        newTexture.Apply();
        return newTexture;
    }
    private void SaveGrid() {
        int[,] gridCopy = new int[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                gridCopy[i, j] = grid[i, j];
            }
        }

        timeLine.Add(gridCopy);
    }
    private void LoadGrid() {
        int[,] sourceGrid = timeLine[timeLine.Count - 1];

        grid = new int[sourceGrid.GetLength(0), sourceGrid.GetLength(1)];

        for (int i = 0; i < sourceGrid.GetLength(0); i++) {
            for (int j = 0; j < sourceGrid.GetLength(1); j++) {
                grid[i, j] = sourceGrid[i, j];
            }
        }

        //timeLine.RemoveAt(timeLine.Count - 1);
        Render(false);
    }
    private void InvertGrid() {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                if (grid[i, j] == 1) {
                    grid[i, j] = 0;
                } else {
                    grid[i, j] = 1;
                }
            }
        }
        Render(false);
    }
    private void RandomizeGrid() {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                int value = UnityEngine.Random.Range(0, 2);
                grid[i, j] = value;
            }
        }
        Render(false);
    }
    private void Discover() {
        RandomRule();
        RandomizeGrid();
    }
    private void RandomRule() {
        ruleSet[0].birthRules = new int[1];
        ruleSet[0].birthRules[0] = UnityEngine.Random.Range(0, 10);

        ruleSet[0].survivalRules = new int[2];
        ruleSet[0].survivalRules[0] = UnityEngine.Random.Range(0, 10);
        ruleSet[0].survivalRules[1] = UnityEngine.Random.Range(0, 10);
    }
}