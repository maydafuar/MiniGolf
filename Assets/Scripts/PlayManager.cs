using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    [SerializeField] BallController ballController;
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject finishWindow;
    [SerializeField] TMP_Text finishText;
    [SerializeField] TMP_Text shootCountText;

    bool isBallOutside;
    bool isBallTeleporting;
    bool isGoal;
    Vector3 lastBallPosition;


    private void OnEnable() 
    {
        ballController.onBallShooted.AddListener(UpdateShootCount);    
    }

    private void OnDisable() 
    {
        ballController.onBallShooted.RemoveListener(UpdateShootCount); 
    }

    private void Update()
    {
        if (ballController.ShootingMode)
        {
            lastBallPosition = ballController.transform.position;
        }
        var inputActive = Input.GetMouseButton(0)
            && ballController.IsMove() == false
            && ballController.ShootingMode == false
            && isBallOutside == false;

        cameraController.SetInputActive(inputActive);
    }

    public void OnBallGoalEnter()
    {
        isGoal = true;
        ballController.enabled = false;

        Debug.Log(finishWindow);
        finishWindow.gameObject.SetActive(true);
        finishText.text = "Level Complete!\n " + " Shoot Count: " + ballController.ShootCount;
        //  TODO player win window popup
    }

    public void OnBallOutside()
    {
        if (isGoal)
        {
            return;
        }

        if (isBallTeleporting == false)
        {
            Invoke("TeleportBallLastPosition", 3);
        }

        ballController.enabled = false;
        isBallOutside = true;
        isBallTeleporting = true;
    }

    public void TeleportBallLastPosition()
    {
        TeleportBall(lastBallPosition);
    }

    public void TeleportBall(Vector3 targetPosition)
    {
        var rb = ballController.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        ballController.transform.position = targetPosition;
        rb.isKinematic = false;

        ballController.enabled = true;
        isBallOutside = false;
        isBallTeleporting = false;
    }

    public void UpdateShootCount(int shootCount)
    {
        shootCountText.text = shootCount.ToString();
    }


    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
