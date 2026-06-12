using Unity.Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;
using UnityEngine;
using Zenject;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces
{
    public class LearningSpacesManager : MonoBehaviour
    {
        public int laboratoryID;

        public int classroomID;

        public LearningSpacePresenter learningSpacePrefab;
        [Inject]

        private ILearningSpaceService _learningSpaceService;

        private Laboratory _laboratory;

        private Classroom _classroom;

        private List<Laboratory> _laboratories = new();

        private LearningSpacePresenter _laboratoryGameObject;

        private LearningSpacePresenter _classroomGameObject;

        private List<LearningSpacePresenter> _laboratoryGameObjects = new();

        public static event Action OnLearningSpacesReady;

        // Start is called before the first frame update
        private async void Start()
        {
            SceneReadiness.RegisterTask("LearningSpace");
            var selectedId = LearningSpaceSession.Instance != null
                ? LearningSpaceSession.Instance.SelectedLearningSpaceId
                : 0;
            var selectedType = LearningSpaceSession.Instance != null ? 
                LearningSpaceSession.Instance.SelectedLearningSpaceType 
                : string.Empty;

            if (selectedId <= 0)
            {
                UnityEngine.Debug.LogError("No SelectedLearningSpaceId set in LearningSpaceSession.");
                return;
            }

            if (selectedType == "Laboratory")
            {
                await LoadSingleLaboratory(selectedId);
            }

            else if (selectedType == "Classroom")
            {
                await LoadSingleClassroom(selectedId);
            }

            SceneReadiness.TaskDone("LearningSpace");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private async Task LoadSingleLaboratory(int labId)
        {
            // fetch
            _laboratory = await _learningSpaceService.GetLaboratoryByIdAsync(labId);

            // cleanup
            if (_laboratoryGameObject != null)
                Destroy(_laboratoryGameObject);

            // instantiate with DI so any [Inject]s on the presenter resolve
            _laboratoryGameObject = Instantiate(learningSpacePrefab, this.transform);

            // feed the presenter
            _laboratoryGameObject.SetData(
                _laboratory.Color,
                _laboratory.Texture,
                _laboratory.Dimensions,
                _laboratory.Coordinates,
                _laboratory.Id);

            OnLearningSpacesReady?.Invoke();

            // spawn/teleport player to the room center (or let a spawner do it)
            SpawnPlayerAtRoomCenter(_laboratoryGameObject);
        }

        private async Task LoadSingleClassroom(int classroomId)
        {
            // fetch
            _classroom = await _learningSpaceService.GetClassroomByIdAsync(classroomId);

            // cleanup
            if (_classroomGameObject != null)
                Destroy(_classroomGameObject);

            // instantiate with DI so any [Inject]s on the presenter resolve
            _classroomGameObject = Instantiate(learningSpacePrefab, this.transform);

            // feed the presenter
            _classroomGameObject.SetData(
                _classroom.Color,
                _classroom.Texture,
                _classroom.Dimensions,
                _classroom.Coordinates,
                _classroom.Id);

            OnLearningSpacesReady?.Invoke();

            // spawn/teleport player to the room center (or let a spawner do it)
            SpawnPlayerAtRoomCenter(_classroomGameObject);
        }

        private void SpawnPlayerAtRoomCenter(LearningSpacePresenter presenter)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return;

            var spawnPos = presenter.transform.TransformPoint(new Vector3(0f, 0.2f, 0f));
            var spawnRot = Quaternion.identity;

            var cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                player.transform.SetPositionAndRotation(spawnPos, spawnRot);
                cc.enabled = true;
            }
            else
            {
                player.transform.SetPositionAndRotation(spawnPos, spawnRot);
            }

            // Tell Cinemachine to immediately cut to the new position
            var vcam = FindFirstObjectByType<CinemachineVirtualCameraBase>();  // or keep a reference
            if (vcam != null)
                vcam.ForceCameraPosition(vcam.State.RawPosition, vcam.State.RawOrientation);
        }

        private void Awake()
        {
            Debug.Log($"[SessionCheck] LearningSpaceSession hash: {LearningSpaceSession.Instance.GetHashCode()}");
        }
    }
    
}
