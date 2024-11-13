using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FfmpegUnity
{
    [RequireComponent(typeof(FfmpegPlayerVideoTexture))]
    public class FfplayPlayerUI : MonoBehaviour
    {
        public FfplayCommand PlayerCommand;
        public RawImage MovieRawImage;
        public Button PlayButton;
        public Slider TimeSlider;
        public Text TimeText;
        public Toggle LoopToggle;

        bool changeTimeSliderOnScript_ = false;
        double duration_ = 0.0;
        bool isSlideTimeSlider_ = false;

        IEnumerator Start()
        {
            var videoTexture = GetComponent<FfmpegPlayerVideoTexture>().VideoTexture;
            while (videoTexture == null)
            {
                yield return null;
                videoTexture = GetComponent<FfmpegPlayerVideoTexture>().VideoTexture;
            }

            Rect rect = MovieRawImage.rectTransform.rect;

            if (rect.width / rect.height < videoTexture.width / videoTexture.height)
            {
                rect.height = rect.width * videoTexture.height / videoTexture.width;
            }
            else
            {
                rect.width = rect.height * videoTexture.width / videoTexture.height;
            }

            MovieRawImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            MovieRawImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            MovieRawImage.rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
        }

        void Update()
        {
            PlayerCommand.Loop = LoopToggle.isOn;

            MovieRawImage.texture = GetComponent<FfmpegPlayerVideoTexture>().VideoTexture;

            if (PlayerCommand.IsRunning && !PlayerCommand.Paused)
            {
                PlayButton.GetComponentInChildren<Text>().text = "Pause";
            }
            else
            {
                PlayButton.GetComponentInChildren<Text>().text = "Play";
            }

            if (!PlayerCommand.IsRunning)
            {
                return;
            }

            duration_ = PlayerCommand.Duration;
            double currentTime = PlayerCommand.CurrentTime;

            if (!isSlideTimeSlider_)
            {
                changeTimeSliderOnScript_ = true;
                if (duration_ > 0.0 && currentTime > 0.0)
                {
                    TimeSlider.value = (float)(currentTime / duration_);
                }
                changeTimeSliderOnScript_ = false;
            }

            string timeFormated(double time)
            {
                string timeText = "";

                int hours = (int)(time / (60.0 * 60.0));
                time -= hours * 60 * 60;
                timeText += string.Format("{0:D2}:", hours);

                int minutes = (int)(time / 60.0);
                time -= minutes * 60;
                timeText += string.Format("{0:D2}:", minutes);

                int seconds = (int)time;
                timeText += string.Format("{0:D2}", seconds);

                return timeText;
            }
            //if (currentTime > 0.0)
            {
                TimeText.text = timeFormated(currentTime) + "\n" + timeFormated(duration_ > 0.0 ? duration_ : 0.0);
            }
        }

        public void OnClickPlayButton()
        {
            if (!PlayerCommand.IsRunning)
            {
                PlayerCommand.Play();
            }
            else
            {
                PlayerCommand.TogglePause();
            }
        }

        public void OnSlideStartTimeSlider()
        {
            isSlideTimeSlider_ = true;
        }

        public void OnChangeValueTimeSlider(float val)
        {
            if (!changeTimeSliderOnScript_)
            {
                
            }
        }

        public void OnSlideStopTimeSlider()
        {
            isSlideTimeSlider_ = false;
            PlayerCommand.Seek(TimeSlider.value);
        }
    }
}
