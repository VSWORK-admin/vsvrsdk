using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FfmpegUnity.Sample
{
    public class CheckFinished : MonoBehaviour
    {
        public Text TextUI;
        public FfmpegCommand TargetCommand;

        bool checkError()
        {
            int returnCode = 0;
            try
            {
                returnCode = TargetCommand.ReturnCode;
            }
            catch (NotImplementedException)
            {
            }
            return returnCode != 0 && returnCode != 255;
        }

        IEnumerator Start()
        {
            TextUI.text = "Status: Is Starting...";

            while (!TargetCommand.IsRunning && !TargetCommand.IsFinished)
            {
                if (checkError())
                {
                    TextUI.text = "Status: Is Error";
                    yield break;
                }
                yield return null;
            }

            TextUI.text = "Status: Is Running...";

            while (TargetCommand.IsRunning && !TargetCommand.IsFinished)
            {
                yield return null;
            }

            if (!checkError())
            {
                TextUI.text = "Status: Is Finished";
            }
            else
            {
                TextUI.text = "Status: Is Error";
            }
        }
    }
}
