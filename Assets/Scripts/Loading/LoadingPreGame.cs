using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
	[SerializeField, Scene] private int _nextSceneMobile = 1;
	[SerializeField, Scene] private int _nextSceneDesktop = 1;
	[Space]
	[SerializeField] private Slider _slider;

	private void Start()
	{
		Message.Log("Start LoadingPreGame");

		var settings = SettingsGame.InstanceF;
		settings.SetPlatform();

		var loadScene = new LoadScene(settings.IsDesktop ? _nextSceneDesktop : _nextSceneMobile, _slider, true);

		var localization = Localization.InstanceF;
		if (!localization.Initialize())
			Message.Error("Error loading Localization!");

		Banners.InstanceF.Initialize();

		if (!Storage.StoragesCreate())
			Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7000);

		settings.IsFirstStart = !InitializeStorages();

		Message.Log("End LoadingPreGame");
		loadScene.End();

		#region Local Functions
		static bool InitializeStorages()
		{
			bool isLoad = Storage.Initialize();

			if (isLoad)
				Message.Log("Storage initialize");
			else
				Message.Log("Storage not initialize");

			return SettingsGame.Instance.Initialize(isLoad) | DataGame.InstanceF.Initialize(isLoad);
		}
		#endregion
	}
}
