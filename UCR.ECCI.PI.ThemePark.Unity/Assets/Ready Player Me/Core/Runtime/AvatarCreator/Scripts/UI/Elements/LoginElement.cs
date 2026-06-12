using System;
using System.Threading;
using ReadyPlayerMe.Core;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class provides all the functionality required to create a basic Login UI element
    /// for building a Custom Avatar Creator.
    /// </summary>
    public class LoginElement : MonoBehaviour
    {
        private const string TAG = nameof(LoginElement);
        [Header("Input Fields")]
        [SerializeField, Tooltip("Input field for entering verification code")] private InputField codeField;

        [Header("Events")]
        [SerializeField] private UnityEvent OnLoginSuccess;
        [SerializeField] private UnityEvent<string> OnLoginFail;

        private bool mergeCurrentSession;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private void OnEnable()
        {
            AuthManager.OnSignInError += LoginFailed;
        }

        private void OnDisable()
        {
            AuthManager.OnSignInError -= LoginFailed;
        }

        private void OnDestroy()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        /// <summary>
        /// Sends a verification code to the email address that was used to login to the ThemePark world
        /// </summary>
        public async void SendVerificationCode()
        {
            string email = EmailExtractor.GetEmailFromPrefs();
            Debug.Log("Sending code to " + email);
            await TaskExtensions.HandleCancellation(AuthManager.SendEmailCode(email, cancellationTokenSource.Token));
        }

        public void MergeCurrentUserToRpmAccount(bool merge)
        {
            mergeCurrentSession = merge;
        }

        /// <summary>
        /// Attempts to login with the verification code that was entered into the code InputField.
        /// </summary>
        public async void LoginWithCode()
        {
            try
            {
                var userIdToMerge = mergeCurrentSession && AuthManager.IsSignedInAnonymously ? AuthManager.UserSession.Id : null;
                if (await AuthManager.LoginWithCode(codeField.text, userIdToMerge, cancellationTokenSource.Token))
                {
                    LoginSuccess();
                }
            }
            catch (Exception e)
            {
                LoginFailed(e.Message);
            }
        }

        private void LoginSuccess()
        {
            OnLoginSuccess?.Invoke();
            SDKLogger.Log(TAG, "Login with code successful");
        }

        private void LoginFailed(string error)
        {
            OnLoginFail?.Invoke(error);
            SDKLogger.Log(TAG, $"Login failed with error: {error}");
        }
    }
}
