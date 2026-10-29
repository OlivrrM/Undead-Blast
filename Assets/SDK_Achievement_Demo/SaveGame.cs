using System;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    /*
    public MeshRenderer MyPlane;
    public enum StoreState
    {
        Idle,
        Reenumerate,
        GetSaveCount,
        StoringData,
        ForceStoreThenDeleteData,
        RetriveData,
        CloseFile
    }

    public enum StoreCommand
    {
        Idle,
        Busy,
        PerformSave,
        PerformSaveForced,
        PerformLoad,
        PerformDelete,
        CheckForOverwrite
    }

    private StoreState _state;
    private StoreCommand _command;
    private X360UserStorageStatus _deviceStatus;
    private string _info;
    private float _theTime;

    // Use this for initialization

    public void Start () 
    {
        X360Storage.OnUserSaveGameContentUpdated += OnReenumerate;
        X360Storage.SelectSaveGameStorageDeviceForUser(0, true, 100);
        _state = StoreState.Reenumerate;
        _command = StoreCommand.Idle;
	    _info = "Welcome...";
    }

    // Update is called once per frame

    public void Update ()
	{
	    UpdateFractal();
	    UpdateStorageDevice();
        
        if (!IsStorageDeviceReady())
            return;

	    UpdateStorageState();

        if (IsStorageDeviceBusy())
            return;

        UpdateCommand();
	}

    public void OnGUI()
    {
        GUI.Label(new Rect(30.0f, 15.0f, 300.0f, 32.0f), "Y: Select Storage (forced)");
        if (_deviceStatus == X360UserStorageStatus.Ready)
        {
            GUI.Label(new Rect(30.0f, 35.0f, 300.0f, 32.0f), "A: Save Game");
            GUI.Label(new Rect(30.0f, 55.0f, 300.0f, 32.0f), "B: Load Game");
            GUI.Label(new Rect(30.0f, 75.0f, 300.0f, 32.0f), "X: Delete Game");
        }

        GUI.Label(new Rect(30f, 115f, 300f, 32f), string.Format("Storage Device State: {0}", _deviceStatus.ToString()));
        GUI.Label(new Rect(30.0f, Screen.height - 100.0f, 300.0f, 64.0f), _info);
    }

    private void UpdateFractal()
    {
        _theTime += Time.deltaTime/16.0f;
        MyPlane.sharedMaterial.SetFloat("_MyFloat", _theTime);
    }

    private void UpdateStorageDevice()
    {
        _deviceStatus = X360Storage.GetUserStorageStatus(0);

        if (_deviceStatus == X360UserStorageStatus.DeviceNotSelected)
            UpdateGamePadActions();
    }

    private void UpdateStorageState()
    {
        switch (_state)
        {
            case StoreState.Reenumerate:
                X360Storage.ReenumerateSaveGames(0);
                break;
            case StoreState.GetSaveCount:
                GetSaveCount();
                break;
            case StoreState.StoringData:
                StoringData();
                break;
            case StoreState.ForceStoreThenDeleteData:
                ForceStoreThenDeleteData();
                break;
            case StoreState.RetriveData:
                RetriveData();
                break;
            case StoreState.CloseFile:
                CloseFile();
                break;
        }
    }

    private void UpdateCommand()
    {
        switch (_command)
        {
            case StoreCommand.Idle:
                UpdateGamePadActions();
                break;
            case StoreCommand.PerformSave:
                PerformSave();
                break;
            case StoreCommand.PerformSaveForced:
                PerformSaveForced();
                break;
            case StoreCommand.CheckForOverwrite:
                CheckForOverwrite();
                break;
            case StoreCommand.PerformDelete:
                PerformDelete();
                break;
            case StoreCommand.PerformLoad:
                PerformLoad();
                break;
        }
    }

    private bool IsStorageDeviceBusy()
    {
        return _state != StoreState.Idle;
    }

    private bool IsStorageDeviceReady()
    {
        return _deviceStatus == X360UserStorageStatus.Ready;
    }

    private void UpdateGamePadActions()
    {
        if (Input.GetKey(KeyCode.JoystickButton3))
        {
            X360Storage.SelectSaveGameStorageDeviceForUser(0, true, 100);
            return;
        }

        if (_deviceStatus != X360UserStorageStatus.Ready)
            return;

        if (Input.GetButtonDown("Fire1"))
            _command = StoreCommand.CheckForOverwrite;
        else if (Input.GetButtonDown("Fire2"))
            _command = StoreCommand.PerformLoad;
        else if (Input.GetButtonDown("Fire3"))
            _command = StoreCommand.PerformDelete;
    }

    private void PerformLoad()
    {
        _command = StoreCommand.Busy;
        X360SaveGame sg = X360Storage.GetSaveGameDesc(0, 0);
        if (sg != null)
        {
            if (!sg.Open("xxx", OnOpen))
            {
                _info = "Failed to open game.";
                _command = StoreCommand.Idle;
                _state = StoreState.Reenumerate;
            }
        }
        else
        {
            _info = "No save games stored.";
            _command = StoreCommand.Idle;
            _state = StoreState.Reenumerate;
        }
    }

    private void PerformDelete()
    {
        _command = StoreCommand.Busy;
        var sg = X360Storage.GetSaveGameDesc(0, 0);
        if (sg != null)
            sg.Delete(OnDelete);
        else if (X360Storage.IsCurrentDeviceCorrupt(0))
        {
            _info = "Corrupt";
            _command = StoreCommand.PerformSaveForced;
            _state = StoreState.Reenumerate;
        }
        else
        {
            _info = "No save games to delete.";
            _command = StoreCommand.Idle;
        }
    }

    private void CheckForOverwrite()
    {
        _command = StoreCommand.Busy;
        var sg = X360Storage.GetSaveGameDesc(0, 0);
        if (sg != null)
            sg.Delete(OnDeleteThenSave);
        else
            _command = StoreCommand.PerformSave;
    }

    private void PerformSave()
    {
        _command = StoreCommand.Busy;
        _info = "Saving...";
        if (!X360Storage.CreateSaveGame(0, "Test_Save", "save.file", "xxx", OnSaveGame))
        {
            _info = "Error in X360Storage.CreateSaveGame()";
            _command = StoreCommand.Idle;
        }
    }

    private void PerformSaveForced()
    {
        _command = StoreCommand.Busy;
        _info = "Saving...";
        if (!X360Storage.CreateSaveGameEx(0, "Test_Save", "save.file", "xxx", 1, OnSaveGameForced))
        {
            _info = "Error in X360Storage.CreateSaveGame()";
            _command = StoreCommand.Idle;
        }
    }

    private void CloseFile()
    {
        if (!X360Storage.Unmount("xxx"))
            _info = "Error in X360Storage.Unmount";
        _state = StoreState.Reenumerate;
    }

    private void RetriveData()
    {
        if (!X360Storage.ReadBuffer("xxx", OnRead))
        {
            _info = "Error in X360Storage.ReadBuffer";
            _state = StoreState.CloseFile;
            _command = StoreCommand.Idle;
        }
    }

    private void StoringData()
    {
        Debug.Log("_theTime is: " + _theTime);
        var buf = BitConverter.GetBytes(_theTime);
        _info = !X360Storage.WriteBuffer("xxx", buf, OnWriteBuffer)
                    ? "Error in X360Storage.WriteBuffer"
                    : "Done.";
        _state = StoreState.CloseFile;
        _command = StoreCommand.Idle;
    }

    private void ForceStoreThenDeleteData()
    {
        var buf = BitConverter.GetBytes(_theTime);
        _info = !X360Storage.WriteBuffer("xxx", buf, OnWriteBuffer)
                    ? "Error in X360Storage.WriteBuffer"
                    : "Done.";
        _state = StoreState.CloseFile;
        _command = StoreCommand.PerformDelete;
    }

    private void GetSaveCount()
    {
        _info += "\nSave game count: " + X360Storage.GetSaveGameCount(0);
        _state = StoreState.Idle;
    }

    private void OnReenumerate( uint index )
    {
        _state++;
    }

    private void OnSaveGameForced( X360StorageOpResult result, uint errorCode, byte[] buffer )
    {
        if (result == X360StorageOpResult.Success)
        {
            _state = StoreState.ForceStoreThenDeleteData;
        }
        else
        {
            _info = "OnSaveGame: " + result;
            _state = StoreState.Idle;
            _command = StoreCommand.Idle;
        }
    }

    private void OnSaveGame( X360StorageOpResult result, uint errorCode, byte[] buffer )
    {
        if (result == X360StorageOpResult.Success)
        {
            _state = StoreState.StoringData;
        }
        else
        {
            _info = "OnSaveGame: " + result;
            _state = StoreState.Idle;
            _command = StoreCommand.Idle;
        }
    }

    private void OnWriteBuffer(X360StorageOpResult result, uint errorCode, byte[] buffer)
    {
        if (result == X360StorageOpResult.Success)
            _info = "Buffer written.";
        else
            _info = "WriteBuffer: " + result;
        _state = StoreState.CloseFile; 
    }

    private void OnDelete(X360StorageOpResult result, uint errorCode, byte[] buffer)
    {
        if (result == X360StorageOpResult.Success)
            _info = "Save game deleted";
        else
            _info = "OnDelete: " + result;

        _command = StoreCommand.Idle;
        _state = StoreState.Reenumerate;
    }

    private void OnDeleteThenSave(X360StorageOpResult result, uint errorCode, byte[] buffer)
    {
        if (result == X360StorageOpResult.Success)
            _info = "Old Save game deleted";
        else
            _info = "OnDeleteThenSave: " + result;

        _command = StoreCommand.PerformSave;
        _state = StoreState.Reenumerate;
    }

    private void OnOpen(X360StorageOpResult result, uint errorCode, byte[] buffer)
    {
        if (result == X360StorageOpResult.Success)
        {
            _info = "Save game opened";
            _state = StoreState.RetriveData;
        }
        else
        {
            _info = "Open: " + result;
            _command = StoreCommand.Idle;
            _state = StoreState.Idle;
        }
    }

    private void OnRead(X360StorageOpResult result, uint errorCode, byte[] buffer)
    {
        if (result == X360StorageOpResult.Success)
        {
            _info = "Buffer read.";
            _theTime = BitConverter.ToSingle(buffer, 0);
        }
        else
            _info = "OnRead: " + result;

        _command = StoreCommand.Idle;
        _state = StoreState.CloseFile;
    }
     * */
}
