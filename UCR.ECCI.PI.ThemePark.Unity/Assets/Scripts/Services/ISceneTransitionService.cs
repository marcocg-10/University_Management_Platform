using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UCR.ECCI.PI.ThemePark.Unity.Services
{
    public interface ISceneTransitionService
    {
        void TransitionTo(string sceneName);
        void TransitionTo(int sceneIndex);
    }
}