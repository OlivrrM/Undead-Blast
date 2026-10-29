using UnityEngine;
using System.Collections;
using System;

public class AchievementExample : MonoBehaviour
{
/*
	enum ExampleState
	{
		ES_Startup = 0,
		ES_SigningIn,
		ES_PrepareForUnlock,
		ES_WaitForUnlock,
		ES_Done,
	};

    private ExampleState            m_state = ExampleState.ES_Startup;
    private string                  m_info = "";
	
	// Achievement icons (debug display)
	private Texture2D[]             m_achievements = null;

	void Start()
	{
		X360Achievements.OnAward += OnAwardedAchievement;
		X360Achievements.OnAchievementsEnumerated += OnAchievementsEnumerated;
	}
	
	void OnAwardedAchievement(uint userIndex, uint achievementId, X360AchievementStatus status)
	{
		BuildInfoString(string.Format("OnAwardedAchievement({0}, {1}, {2})", userIndex, achievementId, status));
	}
	
	void OnAchievementsEnumerated()
	{
		uint count = X360Achievements.GetCount();
		if (count == 0)
			return;
		
		BuildInfoString("Loading achievement icons from Xbox Live");
		m_achievements = new Texture2D[count];
		for (uint i = 0; i < count; ++i)
		{
			m_achievements[i] = X360Achievements.GetAchievementDesc(i).Picture;
		}
	}
	
    void Update()
    {
        switch (m_state)
        {
            case ExampleState.ES_Startup:
                {
                    // Request user to sign-in
                    if (!X360Core.IsUserSignedIn(0, false))
                    {
                        X360Core.RequestSignIn(1, 1, false);
                        BuildInfoString("User sign-in requested");
						m_state = ExampleState.ES_SigningIn;
                    }
                    else
					{
                        BuildInfoString("User is already signed in");
						m_state = ExampleState.ES_PrepareForUnlock;
					}
                }
                break;
			case ExampleState.ES_SigningIn:
				{
					if (X360Core.IsUserSignedIn(0, false) && !X360Core.IsSystemUIVisible())
					{
						BuildInfoString("Sign-in window closed");
						m_state = ExampleState.ES_PrepareForUnlock;
					}
				}
				break;
			case ExampleState.ES_PrepareForUnlock:
				{
					ChangeState("Press 'A' to unlock achievement");
				}
				break;
			case ExampleState.ES_WaitForUnlock:
				{
					if (X360Core.IsUserSignedIn(0, false) && Input.GetButtonDown("Fire1"))
					{
						X360Achievements.AwardUser(0, (uint)SPAConfig.ACHIEVEMENT_PLAYED1GAME);
						ChangeState("Achievement unlock requested");
					}
				}
				break;
			case ExampleState.ES_Done:
				{
				}
				break;
        } // switch

    } // Update

    void OnGUI()
    {
        GUI.Label(new Rect(50, 50, 420, Screen.height - 100), m_info, GUI.skin.box);
		
		if (m_achievements != null)
		{
			float offset = 512.0f;
			foreach (Texture2D picture in m_achievements)
			{
				GUI.DrawTexture(new Rect(offset, 50, 64, 64), picture);
				offset += 64 + 2;
			}
		}
    }

	
	
    #region Callbacks

    void ChangeState(string message)
    {
        m_state++;
        BuildInfoString("State change: " + (m_state - 1).ToString() + " --> " + m_state.ToString() + "\n" + "Action > : " + message);
    }

    void BuildInfoString(string msg)
    {
        Debug.Log(msg);
        m_info = m_info + msg + "\n";
    }

    #endregion
*/
}
