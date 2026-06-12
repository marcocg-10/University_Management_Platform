using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;
using UCR.ECCI.PI.ThemePark.Unity.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Unity.Services;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces
{
    public class LaboratorySelectionUI : MonoBehaviour
    {
        [Inject]
        private ISceneTransitionService _sceneTransitionService;

        [Header("UI")]
        [Tooltip("ScrollView/Viewport/Content where buttons are instantiated")]
        public Transform LaboratoryContent;

        [Header("UI")]
        [Tooltip("ScrollView/Viewport/Content where buttons are instantiated")]
        public Transform ClassroomContent;

        [Tooltip("Button prefab (using Text/TMP_Text child)")]
        public Button buttonPrefab;

        [Tooltip("Title")]
        public Text title;

        [Header("Navigation")]
        [Tooltip("Index of the interior escence.")]
        public int interiorSceneIndex = 2;

        [Inject] private ILearningSpaceService _learningSpaceService;
        [Inject] private IOAuth2Service _oauth;
        [Inject] private IAuthReady _authReady;

        private void Awake()
        {
            // If no title assigned in Inspector, try to auto-locate it in scene
            if (title == null)
            {
                var found = GameObject.Find("TitleText");
                if (found != null)
                {
                    title = found.GetComponent<Text>();
                    Debug.Log("[LabSelectUI] Auto-bound 'TitleText' from scene.");
                }
                else
                {
                    Debug.LogWarning("[LabSelectUI] No GameObject named 'TitleText' found. Title UI will not be updated.");
                }
            }

            // Configure rect transform and alignment
            if (title != null)
            {
                var rt = title.GetComponent<RectTransform>();

                rt.anchorMin = new Vector2(0.5f, 1f);
                rt.anchorMax = new Vector2(0.5f, 1f);
                rt.pivot = new Vector2(0.5f, 1f);

                // Adjust Y position
                rt.anchoredPosition = new Vector2(0f, -20f);

                // Size
                rt.sizeDelta = new Vector2(600f, 45f);

                // Centered Text
                title.alignment = TextAnchor.UpperCenter;

                // Font Size
                title.fontSize = 28;

                // Remove best fit
                title.resizeTextForBestFit = false;

                // Optional Style
                title.fontStyle = FontStyle.Bold;
            }
        }

        private async void Start()
        {

            // BuildingId from session
            if (BuildingSession.Instance == null)
            {
                Debug.LogError("BuildingSession.Instance is null. Did you add it to the bootstrap scene?");
                if (title) title.text = "No building selected";
                return;
            }

            // Get BuildingId
            int buildingId = BuildingSession.Instance.BuildingId;
            if (buildingId <= 0)
            {
                Debug.LogError($"Invalid BuildingId: {buildingId}");
                if (title) title.text = "Invalid building";
                return;
            }

            if (title) title.text = $"Learning Spaces in building {buildingId}";

            // Load laboratories by building
            IEnumerable<Laboratory> labs;

            // Load classrooms by building
            IEnumerable<Classroom> classrooms;

            try
            {
                labs = await _learningSpaceService.ListLaboratoriesByBuildingAsync(buildingId);
                classrooms = await _learningSpaceService.ListClassroomsByBuildingAsync(buildingId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading learningSpace by building: {ex.Message}");
                if (title) title.text = "Failed to load learningSpace";
                return;
            }

            SceneReadiness.RegisterTask("LearningSpaceSelection");

            // Fill UI with buttons
            PopulateLaboratoryButtons(labs?.ToList() ?? new List<Laboratory>());
            PopulateClassroomButtons(classrooms?.ToList() ?? new List<Classroom>());

            //StartCoroutine(SceneReadiness.DelayedAction(3.0f));

            SceneReadiness.TaskDone("LearningSpaceSelection");
        }

        private void PopulateLaboratoryButtons(IReadOnlyList<Laboratory> labs)
        {
            if (!LaboratoryContent)
            {
                Debug.LogError("[LabSelect] Content not assigned in Inspector.");
                return;
            }
            if (!buttonPrefab)
            {
                Debug.LogError("[LabSelect] Button Prefab not assigned.");
                return;
            }

            foreach (Transform child in LaboratoryContent) Destroy(child.gameObject);

            if (labs == null || labs.Count == 0)
            {
                if (title) title.text += " (0 found)";
                return;
            }

            var ordered = labs
                .OrderBy(l => l.FloorLevel ?? int.MaxValue)
                .ThenBy(l => l.RoomId ?? string.Empty);

            foreach (var lab in ordered)
            {
                var btn = Instantiate(buttonPrefab, LaboratoryContent, false);

                var rt = btn.GetComponent<RectTransform>();
                if (rt) { rt.localScale = Vector3.one; rt.anchoredPosition3D = Vector3.zero; }

                var labelText =
                    $"{(lab.FloorLevel.HasValue ? $"Floor {lab.FloorLevel.Value} • " : "")}" +
                    $"{(!string.IsNullOrWhiteSpace(lab.RoomId) ? lab.RoomId : $"Lab #{lab.Id}")}";

                var stdText = btn.GetComponentInChildren<Text>(true);
                if (stdText) stdText.text = labelText;

                Debug.Log($"[Button] LabelText: {labelText}");
                Debug.Log($"[Button] StdText: {stdText.text}");

                int capturedId = lab.Id;
                btn.onClick.AddListener(() =>
                {
                    LearningSpaceSession.Instance.SelectedLearningSpaceId = capturedId;
                    LearningSpaceSession.Instance.SelectedLearningSpaceType = "Laboratory";
                    Debug.Log($"[LabSelect] SelectedLearningSpaceId = {capturedId}");
                    if (_sceneTransitionService == null)
                    {
                        var sceneContext = FindObjectOfType<SceneContext>();
                        if (sceneContext != null)
                        {
                            Debug.Log("[LearningSpaceSelection] Resolving ISceneTransitionService manually from SceneContext.");
                            _sceneTransitionService = sceneContext.Container.Resolve<ISceneTransitionService>();
                        }
                        else
                        {
                            Debug.LogError("[LearningSpaceSelection] No SceneContext found to manually resolve ISceneTransitionService.");
                        }
                    }
                    //SceneManager.LoadScene(interiorSceneIndex);
                    _sceneTransitionService.TransitionTo(interiorSceneIndex);
                });
            }
        }

        private void PopulateClassroomButtons(IReadOnlyList<Classroom> classrooms)
        {
            if (!ClassroomContent)
            {
                Debug.LogError("[ClassroomSelect] Content not assigned in Inspector.");
                return;
            }
            if (!buttonPrefab)
            {
                Debug.LogError("[ClassroomSelect] Button Prefab not assigned.");
                return;
            }

            foreach (Transform child in ClassroomContent) Destroy(child.gameObject);

            if (classrooms == null || classrooms.Count == 0)
            {
                if (title) title.text += " (0 found)";
                return;
            }

            var ordered = classrooms
                .OrderBy(l => l.FloorLevel ?? int.MaxValue)
                .ThenBy(l => l.RoomId ?? string.Empty);

            foreach (var classroom in ordered)
            {
                var btn = Instantiate(buttonPrefab, ClassroomContent, false);

                var rt = btn.GetComponent<RectTransform>();
                if (rt) { rt.localScale = Vector3.one; rt.anchoredPosition3D = Vector3.zero; }

                var labelText =
                    $"{(classroom.FloorLevel.HasValue ? $"Floor {classroom.FloorLevel.Value} • " : "")}" +
                    $"{(!string.IsNullOrWhiteSpace(classroom.RoomId) ? classroom.RoomId : $"classroom #{classroom.Id}")}";

                var stdText = btn.GetComponentInChildren<Text>(true);
                if (stdText) stdText.text = labelText;

                Debug.Log($"[Button] LabelText: {labelText}");
                Debug.Log($"[Button] StdText: {stdText.text}");

                int capturedId = classroom.Id;
                btn.onClick.AddListener(() =>
                {
                    LearningSpaceSession.Instance.SelectedLearningSpaceId = capturedId;
                    LearningSpaceSession.Instance.SelectedLearningSpaceType = "Classroom";
                    Debug.Log($"[ClassroomSelect] SelectedLearningSpaceId = {capturedId}");
                    if (_sceneTransitionService == null)
                    {
                        var sceneContext = FindObjectOfType<SceneContext>();
                        if (sceneContext != null)
                        {
                            Debug.Log("[LearningSpaceSelection] Resolving ISceneTransitionService manually from SceneContext.");
                            _sceneTransitionService = sceneContext.Container.Resolve<ISceneTransitionService>();
                        }
                        else
                        {
                            Debug.LogError("[LearningSpaceSelection] No SceneContext found to manually resolve ISceneTransitionService.");
                        }
                    }
                    //SceneManager.LoadScene(interiorSceneIndex);
                    _sceneTransitionService.TransitionTo(interiorSceneIndex);
                });
            }
        }

    }
}
