using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.InteractiveComponents
{
    /// <summary>
    /// Enables interactive drawing on a board surface.
    /// Creates a unique texture for each board instance at runtime
    /// and allows the user to paint or erase using brush and eraser modes.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(Collider))]
    public class BoardDrawer : MonoBehaviour
    {
        /// <summary>
        /// Width of the drawing texture in pixels.
        /// </summary>
        [Header("Drawing Settings")]
        [Tooltip("Width of the drawing texture in pixels.")]
        public int textureWidth = 1024;

        /// <summary>
        /// Height of the drawing texture in pixels.
        /// </summary>
        [Tooltip("Height of the drawing texture in pixels.")]
        public int textureHeight = 1024;

        /// <summary>
        /// Brush radius in pixels.
        /// </summary>
        [Tooltip("Brush radius in pixels.")]
        [Range(1, 100)]
        public int brushSize = 5;

        /// <summary>
        /// The current active brush color used for drawing on the board.
        /// </summary>
        [Tooltip("Current color used for drawing strokes.")]
        public Color brushColor;

        /// <summary>
        /// The board's default brush color. 
        /// </summary>
        [Tooltip("Default brush color stored for reset operations.")]
        public Color brushDefaultColor;

        /// <summary>
        /// The brush color before the color picker was opened. 
        /// </summary>
        [Tooltip("Original brush color saved before opening the color picker.")]
        public Color originalBrushColor;

        /// <summary>
        /// Temporary preview color shown while the color picker is open.
        /// </summary>
        [Tooltip("Live preview brush color while selecting inside the color picker.")]
        public Color tempBrushColor;

        /// <summary>
        /// Color used for the board background.
        /// </summary>
        [Tooltip("Base color of the board background.")]
        public Color boardColor;

        private Texture2D _drawingTexture;     // The texture that will store drawing data.
        private Texture2D _baseTexture;        // Stores the background texture to preserve it.
        private Renderer _renderer;            // Renderer component to display the texture.
        private bool _isDrawing;               // True while the user holds down the left mouse button.
        private bool _isErasing;               // True if the user is erasing.
        private Vector2 _prevPixel;            // Previous pixel coordinate for interpolation.
        private bool _hasPrevPixel;            // Tracks if a previous drawing point exists.
        private bool _initialized;             // Tracks whether initialization has been completed.
        private BoardCursorManager _cursorManager; // Reference to the cursor manager.
        private BoardMode _mode = BoardMode.Draw;  // Current mode of the board drawer.

        public bool IsErasing => _isErasing;

        /// <summary>
        /// Enum to represent the current mode of the board drawer.
        /// </summary>
        public enum BoardMode
        {
            Idle,
            Draw,
            Erase
        }

        /// <summary>
        /// Initializes the board drawer by creating a unique texture
        /// and applying it to the board's material.
        /// </summary>
        private void Start()
        {
            EnsureInitialized();
            _cursorManager = GetComponent<BoardCursorManager>();
        }

        /// <summary>
        /// Ensures that the texture and material are initialized before being used.
        /// </summary>
        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            _renderer = GetComponent<Renderer>();
            if (_renderer == null)
            {
                Debug.LogError("[BoardDrawer] Missing Renderer component.");
                enabled = false;
                return;
            }

            // Create a new unlit material to prevent lighting or tinting from affecting colors.
            Shader unlitShader = Shader.Find("Unlit/Texture");
            if (unlitShader == null)
            {
                Debug.LogError("[BoardDrawer] Could not find shader 'Unlit/Texture'. Make sure it is included in the build.");
                enabled = false;
                return;
            }
            Material material = new Material(unlitShader)
            {
                color = Color.white // Ensures the texture is displayed exactly as drawn.
            };
            _renderer.material = material;

            // Initialize the drawing texture and apply it to the material.
            _drawingTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };

            // Initialize base texture for background preservation
            _baseTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

            ClearTexture(boardColor);
            _renderer.material.mainTexture = _drawingTexture;

            _initialized = true;
        }

        /// <summary>
        /// Handles mouse input and triggers drawing when appropriate.
        /// </summary>
        private void Update()
        {
            if (!_initialized || Mouse.current == null)
                return;

            if (BoardColorPickerManager.Instance != null
                && BoardColorPickerManager.Instance.IsVisible)
                return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (_mode == BoardMode.Draw || _mode == BoardMode.Erase)
                {
                    _isDrawing = true;
                    _hasPrevPixel = false;
                }
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                _isDrawing = false;
                _hasPrevPixel = false;
            }

            if (_mode == BoardMode.Idle)
                return;

            if (_isDrawing)
                DrawAtMousePosition();
        }

        /// <summary>
        /// Disposes of the created texture and material to free resources.
        /// </summary>
        private void OnDestroy()
        {
            if (_renderer != null && _renderer.material != null)
                Destroy(_renderer.material);

            if (_drawingTexture != null)
                Destroy(_drawingTexture);

            if (_baseTexture != null)
                Destroy(_baseTexture);
        }

        /// <summary>
        /// Clears the entire drawing texture by filling it with the specified color.
        /// </summary>
        /// <param name="backgroundColor">Color to use when filling the texture.</param>
        private void ClearTexture(Color backgroundColor)
        {
            if (_drawingTexture == null)
                return;

            Color[] pixels = new Color[textureWidth * textureHeight];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = backgroundColor;

            _drawingTexture.SetPixels(pixels);
            _drawingTexture.Apply();

            // Also update base texture
            if (_baseTexture != null)
            {
                _baseTexture.SetPixels(pixels);
                _baseTexture.Apply();
            }
        }

        /// <summary>
        /// Calculates the drawing position based on the mouse raycast
        /// and applies brush or eraser strokes at the corresponding texture coordinates.
        /// </summary>
        private void DrawAtMousePosition()
        {
            Camera camera = Camera.main;
            if (camera == null || Mouse.current == null)
                return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            if (!Physics.Raycast(camera.ScreenPointToRay(mousePos), out RaycastHit hit))
                return;

            if (hit.collider.gameObject != gameObject)
                return;

            Vector3 localHit = transform.InverseTransformPoint(hit.point);
            Bounds bounds = _renderer.localBounds;

            // Normalize the hit point to texture-space coordinates (0 to 1 range).
            float normalizedX = Mathf.InverseLerp(bounds.min.x, bounds.max.x, localHit.x);
            float normalizedY = Mathf.InverseLerp(bounds.min.y, bounds.max.y, localHit.y);

            // Flip horizontally to match the board's local X orientation with the
            // texture's UV coordinate system (texture's U axis runs opposite to local X)
            normalizedX = 1f - normalizedX;

            int x = Mathf.Clamp(Mathf.RoundToInt(normalizedX * textureWidth), 0, textureWidth - 1);
            int y = Mathf.Clamp(Mathf.RoundToInt(normalizedY * textureHeight), 0, textureHeight - 1);

            if (_hasPrevPixel)
            {
                if (_isErasing)
                    DrawLine(_prevPixel, new Vector2(x, y), Color.clear, brushSize);
                else
                    DrawLine(_prevPixel, new Vector2(x, y), brushColor, brushSize);
            }
            else
            {
                if (_isErasing)
                    DrawCircle(x, y, brushSize, Color.clear);
                else
                    DrawCircle(x, y, brushSize, brushColor);
                _hasPrevPixel = true;
            }

            _drawingTexture.Apply();
            _prevPixel = new Vector2(x, y);
        }

        /// <summary>
        /// Draws a filled circle of the specified radius and color on the texture.
        /// Blends with existing pixels to preserve background texture.
        /// </summary>
        /// <param name="centerXTexture">Center X coordinate on the texture.</param>
        /// <param name="centerYTexture">Center Y coordinate on the texture.</param>
        /// <param name="radius">Radius of the circle in pixels.</param>
        /// <param name="color">Color of the brush stroke.</param>
        private void DrawCircle(int centerXTexture, int centerYTexture, int radius, Color color)
        {
            if (_drawingTexture == null || _baseTexture == null)
                return;

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (i * i + j * j > radius * radius)
                        continue;

                    int drawPixelX = centerXTexture + i;
                    int drawPixelY = centerYTexture + j;

                    if (drawPixelX >= 0 && drawPixelX < textureWidth && drawPixelY >= 0 && drawPixelY < textureHeight)
                    {
                        Color baseColor = _baseTexture.GetPixel(drawPixelX, drawPixelY);
                        Color currentColor = _drawingTexture.GetPixel(drawPixelX, drawPixelY);

                        // Blend the new color with existing color using alpha blending
                        Color blendedColor = BlendColors(baseColor, currentColor, color);
                        _drawingTexture.SetPixel(drawPixelX, drawPixelY, blendedColor);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a continuous line between two texture points using Bresenham's algorithm.
        /// </summary>
        /// <param name="start">Start point in pixel coordinates.</param>
        /// <param name="end">End point in pixel coordinates.</param>
        /// <param name="color">Color of the line.</param>
        /// <param name="radius">Brush or eraser radius.</param>
        private void DrawLine(Vector2 start, Vector2 end, Color color, int radius)
        {
            int x0 = (int)start.x;
            int y0 = (int)start.y;
            int x1 = (int)end.x;
            int y1 = (int)end.y;

            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                DrawCircle(x0, y0, radius, color);
                if (x0 == x1 && y0 == y1)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        /// <summary>
        /// Blends colors using alpha compositing to preserve background texture.
        /// </summary>
        /// <param name="baseColor">The original background color from the texture.</param>
        /// <param name="currentColor">The current color at this pixel.</param>
        /// <param name="newColor">The new color being drawn (brush or eraser).</param>
        /// <returns>The blended result color.</returns>
        private Color BlendColors(Color baseColor, Color currentColor, Color newColor)
        {
            // If erasing (alpha = 0), restore base texture
            if (newColor.a == 0f)
            {
                return baseColor;
            }

            // Alpha blending: blend new color over current color
            float alpha = newColor.a;
            Color result = new Color(
                currentColor.r * (1f - alpha) + newColor.r * alpha,
                currentColor.g * (1f - alpha) + newColor.g * alpha,
                currentColor.b * (1f - alpha) + newColor.b * alpha,
                Mathf.Max(currentColor.a, newColor.a)
            );

            return result;
        }

        /// <summary>
        /// Immediately sets the active brush color used for drawing.
        /// This is the "direct" setter used by systems that apply color
        /// without going through the color picker workflow.
        /// </summary>
        /// <param name="newColor">The color to assign to the brush.</param>
        /// <param name="isDefault">
        /// If true, this color is also stored as the board's default brush color
        /// and will be restored when using the Reset button.
        /// </param>
        public void SetBrushColor(Color newColor, bool isDefault = false)
        {
            brushColor = newColor;

            if (isDefault)
                brushDefaultColor = newColor;
        }

        /// <summary>
        /// Assigns a temporary preview color while the color picker is open.
        /// This updates the brush color visually in real time, but the change
        /// is NOT permanent unless <see cref="ApplyBrushColor"/> is called.
        /// Used for UI preview behavior.
        /// </summary>
        /// <param name="newColor">The preview color selected in the picker.</param>
        public void SetBrushPreviewColor(Color newColor)
        {
            tempBrushColor = newColor;

            // Drawing uses the preview color immediately.
            brushColor = newColor;
        }

        /// <summary>
        /// Applies the final brush color after the user presses the Accept button.
        /// This makes the selected color permanent until it is changed again.
        /// </summary>
        /// <param name="newColor">The color chosen by the user.</param>
        public void ApplyBrushColor(Color newColor)
        {
            brushColor = newColor;
        }

        /// <summary>
        /// Updates the background color of the board and repaints it.
        /// </summary>
        /// <param name="newColor">The new background color.</param>
        public void SetBoardColor(Color newColor)
        {
            boardColor = newColor;
            EnsureInitialized();
            ClearTexture(boardColor);
        }

        /// <summary>
        /// Switches the board to drawing mode.
        /// </summary>
        public void SetDrawMode()
        {
            _mode = BoardMode.Draw;
            _isErasing = false;
            if (_cursorManager != null)
                _cursorManager.UpdateCursor();
        }

        /// <summary>
        /// Switches the board to erasing mode.
        /// </summary>
        public void SetEraseMode()
        {
            _mode = BoardMode.Erase;
            _isErasing = true;
            if (_cursorManager != null)
                _cursorManager.UpdateCursor();
        }

        /// <summary>
        /// Disables drawing and erasing while keeping the board functional.
        /// </summary>
        public void SetIdleMode()
        {
            _mode = BoardMode.Idle;
            _isDrawing = false;
            _isErasing = false;

            var cursor = GetComponent<BoardCursorManager>();
            if (cursor != null)
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        /// <summary>
        /// Assign the brush size value.
        /// </summary>
        public void SetBrushSize(int newSize)
        {
            brushSize = Mathf.Max(1, newSize);
        }

        /// <summary>
        /// Applies a texture to the board from a file, with an optional tint color.
        /// </summary>
        /// <param name="textureFileName">The name of the texture file (e.g., "myTexture.png").</param>
        /// <param name="tintColor">An optional tint color to apply to the texture.</param>
        public void ApplyBoardTexture(string textureFileName, Color? tintColor = null)
        {
            EnsureInitialized();

            // Permite rutas relativas dentro de Textures/Boards (ej: "maderas/oscura.png" o "maderas/oscura")
            string relative = textureFileName.Replace("\\", "/");
            string noExt = Path.GetFileNameWithoutExtension(relative);
            string dir = Path.GetDirectoryName(relative)?.Replace("\\", "/");

            string resourcePath = string.IsNullOrEmpty(dir)
                ? $"Textures/Boards/{noExt}"
                : $"Textures/Boards/{dir}/{noExt}";

            Texture2D loaded = Resources.Load<Texture2D>(resourcePath);

            if (loaded == null)
            {
                // Fallback Editor: busca con la ruta relativa completa
                string fileNameWithExt = Path.HasExtension(relative) ? relative : relative + ".png";

                string texturePath = Path.GetFullPath(Path.Combine(
                    UnityEngine.Application.dataPath, "Textures", "Boards", fileNameWithExt
                ));

                if (!File.Exists(texturePath))
                {
                    return;
                }
                byte[] fileData = File.ReadAllBytes(texturePath);

                loaded = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (!loaded.LoadImage(fileData))
                {
                    Debug.LogError($"[BoardDrawer] Failed to load texture bytes from: {texturePath}");
                    return;
                }
            }
            Texture2D source = loaded;
            if (source.width != textureWidth || source.height != textureHeight)
            {
                Texture2D resized = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
                for (int y = 0; y < textureHeight; y++)
                {
                    float v = (float)y / (textureHeight - 1);
                    for (int x = 0; x < textureWidth; x++)
                    {
                        float u = (float)x / (textureWidth - 1);
                        Color c = source.GetPixelBilinear(u, v);
                        resized.SetPixel(x, y, c);
                    }
                }
                resized.Apply();
                source = resized;
            }

            Color[] pixels = source.GetPixels();
            if (tintColor.HasValue)
            {
                var tint = tintColor.Value;
                for (int i = 0; i < pixels.Length; i++)
                    pixels[i] *= tint;
            }

            // Store in both drawing and base textures
            _drawingTexture.SetPixels(pixels);
            _drawingTexture.Apply();

            _baseTexture.SetPixels(pixels);
            _baseTexture.Apply();
        }
    }
}
