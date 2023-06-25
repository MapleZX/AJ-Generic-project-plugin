using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AJ.Generic.Tools
{
    [RequireComponent(typeof(Light2D))]
    public class DayAndNightManager : MonoBehaviour
    {
        public enum DayStatus { Early, Morning, Noon, Evening, Night }
        [SerializeField] private DayStatus dayStatus;
        [SerializeField] private float DayTime = 240f;
        [SerializeField] private List<float> clockTimes = new() { 0, 80, 120, 180, 230}; 
        [SerializeField] private List<float> lightness = new() { .1f, 1, 1.25f, .8f, .5f}; 
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool pause = false;
        private Light2D light2D;
        private float currentTime = 0;
        private string dayKey = "Day_Key";
        public float Speed { 
            get => speed;
            set {
                if ((Time.deltaTime * value) <= 0.5f)
                {
                    speed = value;
                } 
            }
        }
        public float CurrentTime {
            get => currentTime;
            set {
                if (value <= DayTime)
                {
                    currentTime = value;
                } 
                if (value < 0)
                {
                    currentTime = 0;
                }
            }
        }
        public bool Pause { get => pause; set => pause = value; }
        void Awake() => AJController.Register<DayAndNightManager>(name, gameObject);
        void OnDestroy() => AJController.UnRegister<DayAndNightManager>(name);
        // Start is called before the first frame update
        void Start()
        {
            light2D = GetComponent<Light2D>();   
            if (PlayerPrefs.HasKey(dayKey))
            {
                dayStatus = (DayStatus)PlayerPrefs.GetInt(dayKey);
            }
            currentTime = clockTimes[(int)dayStatus];
        }

        // Update is called once per frame
        void Update()
        {     
            if (pause) return; 
            if (currentTime < DayTime)
            {
                currentTime += speed * Time.deltaTime;
            } else
            {
                currentTime = 0;
            }
            GameClock(currentTime);
        }
        public void ChangeClock(DayStatus status)
        {
            currentTime = clockTimes[(int)status];
            dayStatus = status;
        }
        private DayStatus ClockStatus(float time)
        {
            var status = dayStatus;
            if (time >= clockTimes[(int)DayStatus.Morning] && time < clockTimes[(int)DayStatus.Noon])
            {
                status = DayStatus.Morning;
            } else if (time >= clockTimes[(int)DayStatus.Noon] && time < clockTimes[(int)DayStatus.Evening])
            {
                status = DayStatus.Noon;
            } else if (time >= clockTimes[(int)DayStatus.Evening] && time < clockTimes[(int)DayStatus.Night])
            {
                status = DayStatus.Evening;
            } else if (time >= clockTimes[(int)DayStatus.Night] && time < DayTime)
            {
                status = DayStatus.Night;
            } else if (time >= clockTimes[(int)DayStatus.Early] && time < clockTimes[(int)DayStatus.Morning])
            {
                status = DayStatus.Early;
            }
            return status;
        }
        private void GameClock(float time)
        {
            var status = ClockStatus(time);
            if (dayStatus != status)
            {
                light2D.intensity = Mathf.Lerp(light2D.intensity, lightness[(int)status], Time.deltaTime);
                var multiple = Mathf.Pow(10, 2);
                var tempValue =  light2D.intensity * multiple + 0.5f;
                tempValue = Mathf.FloorToInt(tempValue);
                var finalValue = tempValue / multiple;
                if (finalValue == lightness[(int)status])
                {
                    dayStatus = status;
                    light2D.intensity = finalValue;
                    PlayerPrefs.SetInt(dayKey, (int)dayStatus);
                }
            }          
        }
    }
}
