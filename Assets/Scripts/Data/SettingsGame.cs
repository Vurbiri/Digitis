using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsGame : ASingleton<SettingsGame>
{
    [Space]
    [SerializeField] private int _qualityDesktop = 1;
    [SerializeField] private Profile _profileDesktop = new();
    [Space]
    [SerializeField] private int _qualityMobile = 0;
    [SerializeField] private Profile _profileMobile = new();
    [Space]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private float _audioMinValue = 0.01f;
    [SerializeField] private float _audioMaxValue = 1.5845f;

    private Profile _profileCurrent = null;

    public float SensitivityButtons { get => _profileCurrent.sensitivityButtons; set { _profileCurrent.sensitivityButtons = value; EventChangeSensitivityButtons?.Invoke(value); } }

    public float MinValue => _audioMinValue;
    public float MaxValue => _audioMaxValue;

    public bool IsDesktop { get; private set; } = true;
    public bool IsFirstStart { get; set; } = true;

    public event Action<float> EventChangeSensitivityButtons;

    private YandexSDK _ysdk;
    private Localization _localization;

    public bool Initialize(bool isLoad, bool isDesktop)
    {
        _ysdk = YandexSDK.InstanceF;
        _localization = Localization.InstanceF;

        IsDesktop = isDesktop;
        QualitySettings.SetQualityLevel(isDesktop ? _qualityDesktop : _qualityMobile);

        bool result = false;

        DefaultProfile();
        if (isLoad)
            result = Load();
        Apply();

        return result;
    }

    public void SetVolume(MixerGroup type, float volume)
    {
        _audioMixer.SetFloat(type.ToString(), ConvertToDB(volume));

        static float ConvertToDB(float volume)
        {
            volume = Mathf.Log10(volume) * 40f;
            if (volume > 0) volume *= 2.5f;

            return volume;
        }
    }
    public float GetVolume(MixerGroup type) => _profileCurrent.volumes[type.ToInt()];

    public void Save(bool isSaveHard = true, Action<bool> callback = null)
    {
        _profileCurrent.idLang = _localization.CurrentIdLang;
        foreach (var mixer in Enum<MixerGroup>.GetValues())
        {
            _audioMixer.GetFloat(mixer.ToString(), out float volumeDB);
            _profileCurrent.volumes[mixer.ToInt()] = MathF.Round(ConvertFromDB(volumeDB), 3);
        }

        Storage.Save(_profileCurrent.key, _profileCurrent, isSaveHard, callback);

        static float ConvertFromDB(float dB)
        {
            if (dB > 0) dB /= 2.5f;
            dB = Mathf.Pow(10, dB / 40f);

            return dB;
        }
    }
    private bool Load()
    {
        Return<Profile> data = Storage.Load<Profile>(_profileCurrent.key);
        if (data.Result)
            _profileCurrent.Copy(data.Value);

        return data.Result;
    }

    public void Cancel()
    {
        if (!Load())
            DefaultProfile();

        Apply();
    }

    private void DefaultProfile()
    {
        _profileCurrent = (IsDesktop ? _profileDesktop : _profileMobile).Clone();

        if (_ysdk.IsInitialize)
            if (_localization.TryIdFromCode(_ysdk.Lang, out int id))
                _profileCurrent.idLang = id;
    }

    private void Apply()
    {
        _localization.SwitchLanguage(_profileCurrent.idLang);
        foreach (var mixer in Enum<MixerGroup>.GetValues())
            SetVolume(mixer, _profileCurrent.volumes[mixer.ToInt()]);
    }

    #region Nested Classe
    [System.Serializable]
    private class Profile
    {
        [JsonIgnore]
        public string key = "std";
        [JsonProperty("ilg")]
        public int idLang = 1;
        [JsonProperty("sbt")]
        public float sensitivityButtons = 0.2f;
        [JsonProperty("vls")]
        public float[] volumes = { 0.75f, 0.75f };

        [JsonConstructor]
        public Profile(int idLang, float sensitivityButtons, float[] volumes)
        {
            this.idLang = idLang;
            this.sensitivityButtons = sensitivityButtons;
            this.volumes = (float[])volumes.Clone();
        }

        public Profile() { }

        public void Copy(Profile profile)
        {
            if (profile == null) return;

            idLang = profile.idLang;
            sensitivityButtons = profile.sensitivityButtons;
            profile.volumes.CopyTo(volumes, 0);
        }

        public Profile Clone()
        {
            return new(idLang, sensitivityButtons, volumes) { key = key };
        }

    }
    #endregion
}
