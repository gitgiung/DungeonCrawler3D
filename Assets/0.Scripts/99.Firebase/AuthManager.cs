using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class AuthManager : Singleton<AuthManager>
{
    private FirebaseAuth auth;
    private bool isSigningIn;

    public FirebaseUser CurrentUser => auth?.CurrentUser;
    public bool IsLoggedIn => CurrentUser != null;

    public void Init()
    {
        Debug.Log("[Auth] Init");

        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += OnAuthStateChanged;

        if (auth.CurrentUser != null)
        {
            Debug.Log("[Auth] Existing User");
        }
        else
        {
            SignInAnonymous();
        }
    }

    private void OnAuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            Debug.Log("[Auth] Login State");
        }
        else
        {
            Debug.Log("[Auth] Logout State");
        }
    }

    private void CheckLogin()
    {
        if (auth == null)
        {
            Debug.LogError("[Auth] Not Initialized");
            return;
        }

        if (CurrentUser != null)
        {
            Debug.Log("[Auth] Existing User");
            OnLoginSuccess(CurrentUser);
            return;
        }

        SignInAnonymous();
    }

    private void SignInAnonymous()
    {
        if (isSigningIn)
            return;

        isSigningIn = true;

        auth.SignInAnonymouslyAsync()
            .ContinueWithOnMainThread(task =>
            {
                isSigningIn = false;

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

                OnLoginSuccess(task.Result.User);
            });
    }

    private void OnLoginSuccess(FirebaseUser user)
    {
        if (user == null)
        {
            Debug.LogError("[Auth] User is null");
            return;
        }

        Debug.Log("[Auth] Login Success");
        Debug.Log($"[Auth] UID: {user.UserId}");
        Debug.Log($"[Auth] Anonymous: {user.IsAnonymous}");
    }

    // 버튼 클릭용 로그인
    public void SignIn()
    {
        CheckLogin();
    }

    // 로그아웃
    public void SignOut()
    {
        if (auth == null)
        {
            Debug.LogError("[Auth] Not Initialized");
            return;
        }

        if (isSigningIn)
        {
            Debug.LogWarning("[Auth] Login In Progress");
            return;
        }

        if (CurrentUser == null)
        {
            Debug.Log("[Auth] Already Signed Out");
            return;
        }

        Debug.Log($"[Auth] Sign Out: {CurrentUser.UserId}");

        auth.SignOut();

        Debug.Log("[Auth] Sign Out Complete");
    }

    private void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= OnAuthStateChanged;
        }
    }
}