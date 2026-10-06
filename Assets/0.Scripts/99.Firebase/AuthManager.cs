using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class AuthManager : Singleton<AuthManager>
{
    private FirebaseAuth auth;

    public FirebaseUser CurrentUser => auth?.CurrentUser;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;

        SignInAnonymous();
    }

    private void SignInAnonymous()
    {
        auth.SignInAnonymouslyAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogWarning("[Auth] Login Cancel");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("[Auth] Login Failed");
                    Debug.LogException(task.Exception);
                    return;
                }

                FirebaseUser user = task.Result.User;

                Debug.Log("[Auth] Login Success");
                Debug.Log("[Auth] UID: " + user.UserId);
                Debug.Log("[Auth] Anonymous: " + user.IsAnonymous);
            });
    }
}