using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsGame : Singleton<SettingsGame>
{
    [Space]
    [SerializeField] private Profile _profileDesktop = new();
    [SerializeField] private Profile _profileMobile = new();
    [Space]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private float _audioMinValue = 0.01f;
    [SerializeField] private float _audioMaxValue = 1.5845f;

    private Profile _profileCurrent = null;

    public float MinValue => _audioMinValue;
    public float MaxValue => _audioMaxValue;

    public bool IsDesktop { get; private set; } = true;

    private YandexSDK _ysdk;
    private Localization _localization;

    public bool Initialize(bool isLoad)
    {
        _ysdk = YandexSDK.InstanceF;
        _localization = Localization.InstanceF;

        if (_ysdk.IsPlayer)
            IsDesktop = _ysdk.IsDesktop;
        else
            IsDesktop = !UtilityJS.Instance.IsMobile;

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
        ReturnValue<Profile> data = Storage.Load<Profile>(_profileCurrent.key);
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


    [System.Serializable]
    private class Profile
    {
        [JsonIgnore]
        public string key = "std";
        [JsonProperty("ilg")]
        public int idLang = 1;
        [JsonProperty("vls")]
        public float[] volumes = { 1f, 1f };

        [JsonConstructor]
        public Profile(int idLang, float[] volumes)
        {
            this.idLang = idLang;
            volumes.CopyTo(this.volumes, 0);
        }

        public Profile() { }

        public void Copy(Profile profile)
        {
            if (profile == null) return;

            idLang = profile.idLang;
            profile.volumes.CopyTo(volumes, 0);
        }

        public Profile Clone()
        {
            return new(idLang, volumes) { key = key };
        }

    }
}
