using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu = null;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (pauseMenu == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool _newState = !pauseMenu.activeSelf;

            pauseMenu.SetActive(_newState);

            Cursor.lockState = _newState == true ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _newState;
        }
    }
}
