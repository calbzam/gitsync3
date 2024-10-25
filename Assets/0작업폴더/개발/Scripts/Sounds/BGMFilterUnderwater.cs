using UnityEngine;

public class BGMFilterUnderwater : MonoBehaviour
{
    [SerializeField] private AudioSource _stage1_2Bgm;
    [SerializeField] private float _filterMoveSpeed = 1;

    private AudioLowPassFilter _underwaterLowpassFilter;
    private AudioEchoFilter _underwaterEchoFilter;
    
    private float _underwaterCutoffFreq, _abovewaterCutoffFreq, _maxCutoffFreq;
    private float _underwaterWetMix, _abovewaterWetMix;
    
    private float _underwaterPercent;

    private void Awake()
    {
        _underwaterLowpassFilter = GetComponent<AudioLowPassFilter>();
        _underwaterEchoFilter = GetComponent<AudioEchoFilter>();

        _maxCutoffFreq = 12000;
        _abovewaterCutoffFreq = 4400;
        _abovewaterWetMix = 0;
    }

    private void Start()
    {
        _underwaterCutoffFreq = _underwaterLowpassFilter.cutoffFrequency;
        _underwaterWetMix = _underwaterEchoFilter.wetMix;

        SetUnderwaterFilterValues();
    }

    private void SetUnderwaterFilterValues()
    {
        if (PlayerLogic.Player.IsInWater)
        {
            _underwaterLowpassFilter.cutoffFrequency = _underwaterCutoffFreq;
            _underwaterEchoFilter.wetMix = _underwaterWetMix;
            _underwaterPercent = 1;
        }
        else
        {
            _underwaterLowpassFilter.cutoffFrequency = _abovewaterCutoffFreq;
            _underwaterEchoFilter.wetMix = _abovewaterWetMix;
            _underwaterPercent = 0;
        }
    }

    private void Update()
    {
        if (!_stage1_2Bgm.isPlaying) return;

        if (PlayerLogic.Player.IsInWater)
        {
            if (_underwaterPercent < 0)
            {
                if (_underwaterLowpassFilter.cutoffFrequency - _abovewaterCutoffFreq < 5) _underwaterPercent = -0.001f; // quickly transition to underwater ready regardless of percent
                _underwaterPercent = Mathf.MoveTowards(_underwaterPercent, 0, _filterMoveSpeed * Time.deltaTime);

                _underwaterLowpassFilter.cutoffFrequency = Mathf.Lerp(_underwaterLowpassFilter.cutoffFrequency, _abovewaterCutoffFreq, -_underwaterPercent); // from _maxCutoffFreq
            }
            else if (_underwaterPercent < 1f)
            {
                _underwaterPercent = Mathf.MoveTowards(_underwaterPercent, 1f, _filterMoveSpeed * Time.deltaTime);

                _underwaterLowpassFilter.cutoffFrequency = Mathf.Lerp(_underwaterLowpassFilter.cutoffFrequency, _underwaterCutoffFreq, _underwaterPercent);
                _underwaterEchoFilter.wetMix = _underwaterPercent; // to 1
            }
        }

        else
        {
            if (_underwaterPercent > 0f)
            {
                if (_abovewaterCutoffFreq - _underwaterLowpassFilter.cutoffFrequency < 5) _underwaterPercent = 0.001f; // quickly transition to maxfreq ready regardless of percent
                _underwaterPercent = Mathf.MoveTowards(_underwaterPercent, 0f, _filterMoveSpeed * Time.deltaTime);

                _underwaterLowpassFilter.cutoffFrequency = Mathf.Lerp(_underwaterLowpassFilter.cutoffFrequency, _abovewaterCutoffFreq, 1 - _underwaterPercent);
                _underwaterEchoFilter.wetMix = _underwaterPercent; // to 0
            }
            else if (_underwaterPercent > -1)
            {
                _underwaterPercent = Mathf.MoveTowards(_underwaterPercent, -1, _filterMoveSpeed * Time.deltaTime);

                _underwaterLowpassFilter.cutoffFrequency = Mathf.Lerp(_underwaterLowpassFilter.cutoffFrequency, _maxCutoffFreq, -_underwaterPercent); // to _maxCutoffFreq
            }
        }
    }
}
