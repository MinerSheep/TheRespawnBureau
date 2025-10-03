using UnityEngine;
using Firebase.Auth;
using System;

public class FirebaseAuthManager
{
    private static FirebaseAuthManager instance = null;

    public static FirebaseAuthManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseAuthManager();
            }
            return instance;
        }
    }

    private FirebaseAuth auth; // Login / Register
    private FirebaseUser user; // Authenticated user

    public string UserId => user.UserId;

    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser != null)
        {
            Logout();
        }

        auth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if(auth.CurrentUser != user)
        {
            bool signed = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signed && user != null)
            {
                Debug.Log("Signed out");
                LoginState?.Invoke(false);
            }
            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("Signed in");
                LoginState?.Invoke(true);
            }
        }
    }

    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => 
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Register Canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Register Failiure");
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            user = newUser;
            Debug.LogError("Registser Successfully");
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Login Canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Login Failiure");
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            user = newUser;
            Debug.LogError("Login Successfully");
        });
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("User logged out.");
    }
}
