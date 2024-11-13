using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FfmpegUnity
{
    [RequireComponent(typeof(FfmpegPlayerVideoTexture))]
    public class FfmpegPlayerUI : MonoBehaviour
    {
        public FfmpegPlayerCommand PlayerCommand;
        public RawImage MovieRawImage;
        public Button PlayButton;
        public Slider TimeSlider;
        public Text TimeText;
        public Toggle LoopToggle;

        bool ignoreLoop_ = false;
        bool changeTimeSliderOnScript_ = false;
        float duration_ = 0f;
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
            if (!PlayerCommand.IsPlaying && LoopToggle.isOn && !ignoreLoop_)
            {
                PlayerCommand.StartFfmpeg();
            }
            if (!LoopToggle.isOn)
            {
                ignoreLoop_ = true;
            }
            else if (PlayerCommand.IsPlaying)
            {
                ignoreLoop_ = false;
            }

            MovieRawImage.texture = GetComponent<FfmpegPlayerVideoTexture>().VideoTexture;

            if (PlayerCommand.IsPlaying)
            {
                PlayButton.GetComponentInChildren<Text>().text = "Pause";
            }
            else
            {
                PlayButton.GetComponentInChildren<Text>().text = "Play";
            }

            if (PlayerCommand.IsPlaying)
            {
                duration_ = PlayerCommand.Duration;
            }

            if (!isSlideTimeSlider_)
            {
                changeTimeSliderOnScript_ = true;
                float duration = PlayerCommand.Duration;
                if (duration > 0f)
                {
                    TimeSlider.maxValue = duration;
                }
                TimeSlider.value = PlayerCommand.Time;
                changeTimeSliderOnScript_ = false;
            }

            string timeFormated(float time)
            {
                string timeText = "";

                int hours = (int)(time / (60f * 60f));
                time -= hours * 60 * 60;
                timeText += string.Format("{0:D2}:", hours);

                int minutes = (int)(time / 60f);
                time -= minutes * 60;
                timeText += string.Format("{0:D2}:", minutes);

                int seconds = (int)time;
                timeText += string.Format("{0:D2}", seconds);

                return timeText;
            }
            TimeText.text = timeFormated(PlayerCommand.Time) + "\n" + timeFormated(duration_);
        }

        public void OnClickPlayButton()
        {
            if (PlayerCommand.IsPlaying)
            {
                ignoreLoop_ = true;
                PlayerCommand.StopFfmpeg();
            }
            else
            {
                ignoreLoop_ = false;
                PlayerCommand.StartFfmpeg();
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
            PlayerCommand.SetTime(TimeSlider.value);
        }
    }
}
