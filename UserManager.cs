using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;

    private Dictionary<string, UserProfile> userProfiles = new Dictionary<string, UserProfile>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddUserProfile(string playerName, int playerID)
    {
        if (!userProfiles.ContainsKey(playerName))
        {
            UserProfile newUserProfile = new UserProfile(playerName, playerID);
            userProfiles.Add(playerName, newUserProfile);
            Debug.Log($"Added UserProfile: {playerName} with ID {playerID}");
        }
        else
        {
            Debug.LogWarning($"UserProfile for {playerName} already exists.");
        }
    }

    public UserProfile GetUserByName(string playerName)
    {
        if (userProfiles.TryGetValue(playerName, out UserProfile profile))
        {
            return profile;
        }
        else
        {
            Debug.LogWarning($"UserProfile not found for playerName: {playerName}");
            return null;
        }
    }

    public UserProfile GetUserByID(int playerID)
    {
        foreach (var profile in userProfiles.Values)
        {
            if (profile.playerID == playerID)
            {
                return profile;
            }
        }
        Debug.LogWarning($"UserProfile not found for playerID: {playerID}");
        return null;
    }
}
