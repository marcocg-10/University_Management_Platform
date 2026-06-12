using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces;

[RequireComponent(typeof(CinemachineConfiner3D))]
public class LearningSpaceConfiner : MonoBehaviour
{
    private IEnumerator Start()
    {
        var confiner = GetComponent<CinemachineConfiner3D>();
        if (confiner == null)
            yield break;

        // Wait until a LearningSpacePresenter exists in the scene
        LearningSpacePresenter presenter = null;
        while (presenter == null)
        {
            presenter = FindObjectOfType<LearningSpacePresenter>();
            if (presenter == null)
                yield return null; // wait a frame and try again
        }

        // Make sure the collider exists
        if (presenter.LearningSpaceBounds == null)
        {
            Debug.LogWarning("LearningSpaceConfiner: LearningSpaceBounds collider is missing.");
            yield break;
        }

        confiner.BoundingVolume = presenter.LearningSpaceBounds;
        Debug.Log("Cinemachine Confiner 3D bound to LearningSpace bounds.");
    }
}
