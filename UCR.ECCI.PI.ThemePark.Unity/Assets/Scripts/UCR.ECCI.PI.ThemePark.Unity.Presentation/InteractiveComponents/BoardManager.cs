using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Application;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.InteractiveComponents;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.Managers
{
    /// <summary>
    /// Manages the lifecycle of <see cref="Board"/> objects in the scene.
    /// Handles fetching, instantiation, and refreshing of all interactive board representations.
    /// </summary>
    public class BoardManager : MonoBehaviour
    {
        [Header("Prefabs & Dependencies")]
        [SerializeField] private GameObject boardPrefab;

        private IInteractiveComponentService _service;
        private readonly List<GameObject> _spawnedBoards = new();

        /// <summary>
        /// Maps each LearningSpaceId to its corresponding transform for board placement.
        /// </summary>
        private readonly Dictionary<int, Transform> _learningSpaceLookup = new();

        /// <summary>
        /// Injects dependencies through Zenject.
        /// </summary>
        /// <param name="service">Service that provides access to interactive component data.</param>
        [Inject]
        public void Construct(IInteractiveComponentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Subscribes to the event that notifies when learning spaces are ready.
        /// </summary>
        private void OnEnable()
        {
            LearningSpacesManager.OnLearningSpacesReady += HandleLearningSpacesReady;
        }

        /// <summary>
        /// Unsubscribes when this component is disabled to avoid memory leaks or duplicate listeners.
        /// </summary>
        private void OnDisable()
        {
            LearningSpacesManager.OnLearningSpacesReady -= HandleLearningSpacesReady;
        }

        /// <summary>
        /// Detects runtime input and refreshes the boards when the 'R' key is pressed.
        /// </summary>
        private async void Update()
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                await RefreshBoards();
            }
        }

        /// <summary>
        /// Triggered when learning spaces have been fully instantiated in the scene.
        /// Re-caches their transforms and loads all boards.
        /// </summary>
        private async void HandleLearningSpacesReady()
        {
            SceneReadiness.RegisterTask("Boards");

            UnityEngine.Debug.Log("[BoardManager] Received notification: Learning spaces ready. Caching and loading boards...");

            CacheLearningSpaces();

            var boards = await _service.ListAllBoardsAsync();
            DisplayBoards(boards);

            SceneReadiness.TaskDone("Boards");
        }

        /// <summary>
        /// Clears all spawned boards and reloads updated data from the service.
        /// </summary>
        private async Task RefreshBoards()
        {
            SceneReadiness.RegisterTask("BoardsRefresh");
            ClearBoards();
            var boards = await _service.RefreshBoardsAsync();
            DisplayBoards(boards);
            SceneReadiness.TaskDone("BoardsRefresh");
        }

        /// <summary>
        /// Instantiates and initializes board prefabs in the scene,
        /// parenting them under their respective learning space transforms.
        /// </summary>
        /// <param name="boards">Collection of boards to be rendered.</param>
        private void DisplayBoards(IEnumerable<Board> boards)
        {
            if (boardPrefab == null)
            {
                UnityEngine.Debug.LogError("<color=red>[BoardManager]</color> Board prefab not assigned in Inspector!");
                return;
            }

            foreach (var board in boards)
            {
                // Find the learning space parent for this board
                if (!_learningSpaceLookup.TryGetValue(board.LearningSpaceId, out Transform parentSpace))
                {
                    UnityEngine.Debug.LogWarning($"[BoardManager] No LearningSpace found for Board {board.PlateId.Value} (LearningSpaceId={board.LearningSpaceId}).");
                    continue;
                }

                // Instantiate board as a child of the corresponding learning space
                var go = Instantiate(boardPrefab, parentSpace);
                go.name = $"Board_{board.PlateId.Value}";

                var presenter = go.GetComponent<BoardPresenter>();
                if (presenter != null)
                    presenter.Initialize(board);

                _spawnedBoards.Add(go);
            }

            UnityEngine.Debug.Log($"<color=green>[BoardManager]</color> Rendered {boards.Count()} boards in the scene.");

            var sceneManager = FindFirstObjectByType<BoardInteractionSceneManager>();
            if (sceneManager != null)
                sceneManager.AssignToAllBoards();
        }

        /// <summary>
        /// Destroys all currently spawned board objects and clears the tracking list.
        /// </summary>
        private void ClearBoards()
        {
            foreach (var go in _spawnedBoards)
            {
                if (go != null)
                    Destroy(go);
            }
            _spawnedBoards.Clear();
        }

        /// <summary>
        /// Finds and caches all instantiated learning spaces currently active in the scene.
        /// </summary>
        private void CacheLearningSpaces()
        {
            var spaces = FindObjectsByType<LearningSpacePresenter>(FindObjectsSortMode.None);
            _learningSpaceLookup.Clear();

            foreach (var space in spaces)
            {
                if (space == null)
                    continue;

                int id = space.LearningSpaceId;
                if (!_learningSpaceLookup.ContainsKey(id))
                    _learningSpaceLookup.Add(id, space.transform);
            }

            UnityEngine.Debug.Log($"[BoardManager] Cached {_learningSpaceLookup.Count} learning spaces for board placement.");
        }
    }
}
