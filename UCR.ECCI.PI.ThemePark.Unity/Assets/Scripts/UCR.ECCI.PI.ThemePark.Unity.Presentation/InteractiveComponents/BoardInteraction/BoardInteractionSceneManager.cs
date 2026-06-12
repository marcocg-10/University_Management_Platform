using UnityEngine;
using System;

/// <summary>
/// Scene manager that injects camera controller components (<see cref="Behaviour"/>[]) into all <see cref="BoardInteraction"/> instances.
/// </summary>
public class BoardInteractionSceneManager : MonoBehaviour
{
    [Header("Componentes de cámara a deshabilitar en foco")]
    [Tooltip("Arrastra aquí los componentes (Behaviour) que viven en la escena y deben deshabilitarse mientras el board está en foco.")]
    public Behaviour[] cameraComponentsToDisableOnFocus;

    [Tooltip("Reasignar automáticamente al finalizar Start.")]
    public bool assignOnStart = true;

    [Tooltip("Reasignar periódicamente (0 = desactivado). Útil si se instancian boards sin evento.")]
    public float periodicReassignSeconds = 0f;

    private void Start()
    {
        if (assignOnStart)
            AssignToAllBoards();

        if (periodicReassignSeconds > 0f)
            InvokeRepeating(nameof(AssignToAllBoards), periodicReassignSeconds, periodicReassignSeconds);
    }

    /// <summary>
    /// Assigns the configured camera controller components to all <see cref="BoardInteraction"/> instances found in the scene.
    /// This can be invoked explicitly (for example, by <see cref="BoardManager"/> after instantiating new board prefabs).
    /// Null-safe: if <see cref="cameraComponentsToDisableOnFocus"/> is null, an empty array is applied.
    /// </summary>
    public void AssignToAllBoards()
    {
        var boards = FindObjectsByType<BoardInteraction>(FindObjectsSortMode.None);
        var controllers = cameraComponentsToDisableOnFocus ?? Array.Empty<Behaviour>();
        for (int i = 0; i < boards.Length; i++)
        {
            var b = boards[i];
            if (b == null) continue;
            b.InjectCameraControllers(controllers);
        }
    }
}