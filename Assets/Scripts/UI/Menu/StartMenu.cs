using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MenuNavigation
{
	[Scene]
	[SerializeField] private int _sceneNext = 3;
	[Space]
	[SerializeField] private SettingsGameStart _settingsGame;

	private DataGame _dataGame;

	private void Start()
	{
		_dataGame = DataGame.Instance;
		_settingsGame.Initialize();
	}

	public void OnStart()
	{
		LoadScene loadScene = new(_sceneNext);
		loadScene.Start();

		if (_settingsGame.IsNewGame)
		{
			_dataGame.ResetData();
			_dataGame.ShapeType = _settingsGame.ShapeType;
			_dataGame.MaxDigit = _settingsGame.MaxDigit;
			_dataGame.IsInfinityMode = _settingsGame.IsInfinity;
			_dataGame.Save(true, null);
		}

		MusicSingleton.Instance.Stop();
		loadScene.End();

	}

	#region Nested Classe
	[System.Serializable]
	private class SettingsGameStart
	{
		[SerializeField] private ToggleFullInteractable _toggleContinue;
		[Space]
		[SerializeField] private SliderFullInteractable _sliderMax;
		[SerializeField] private SliderFullInteractable _sliderSize;
		[SerializeField] private ToggleFullInteractable _infinityToggle;
		[Space]
		[SerializeField] private Image _iconButton;
		[SerializeField] private Sprite _spriteEmpty;
		[SerializeField] private Sprite _spriteInfinity;
		[Space]
		[SerializeField] private Image _iconInfinity;

		private DataGame _dataGame;

		private float _tempValueMax;
		private float _tempValueSize;
		private bool _tempValueInfinity;

		private readonly Color _colorOn = Color.white;
		private readonly Color _colorOff = new(1f, 1f, 1f, 0.25f);

		public int MaxDigit => Mathf.RoundToInt(_sliderMax.Value);
		public ShapeSize ShapeType => Mathf.RoundToInt(_sliderSize.Value).ToEnum<ShapeSize>();
		public bool IsInfinity => _infinityToggle.IsOn;

		public bool IsNewGame => !_toggleContinue.IsOn;

		public void Initialize()
		{
			_dataGame = DataGame.Instance;

			_sliderMax.MinValue = _dataGame.MinDigit;
			
			SetStart(!_dataGame.IsNewGame);
			SetIcons(_dataGame.IsInfinityMode);
			_toggleContinue.OnValueChanged.AddListener(ChangeGameStart);
			_infinityToggle.OnValueChanged.AddListener(SetIcons);

			_sliderMax.Value = _tempValueMax = _dataGame.MaxDigit;
			_sliderSize.Value = _tempValueSize = _dataGame.ShapeType.ToInt();
			_infinityToggle.IsOn = _tempValueInfinity = _dataGame.IsInfinityMode || (_dataGame.IsNewGame && _dataGame.MaxScore == 0 && !SettingsGame.Instance.IsFirstStart);

			#region Local Functions
			void SetStart(bool isContinueGame)
			{
				_toggleContinue.Interactable = isContinueGame;
				_toggleContinue.IsOn = isContinueGame;

				SetInteractable(!isContinueGame);
			}
			
			void ChangeGameStart(bool isContinueGame)
			{
				if(isContinueGame)
				{
					_tempValueMax = _sliderMax.Value;
					_tempValueSize = _sliderSize.Value;
					_tempValueInfinity = _infinityToggle.IsOn;

					_sliderMax.Value = _dataGame.MaxDigit;
					_sliderSize.Value = _dataGame.ShapeType.ToInt();
					_infinityToggle.IsOn = _dataGame.IsInfinityMode;
				}
				else 
				{
					_sliderMax.Value = _tempValueMax;
					_sliderSize.Value = _tempValueSize;
					_infinityToggle.IsOn = _tempValueInfinity;
				}

				SetInteractable(!isContinueGame);
			}
			void SetInteractable(bool isNewGame)
			{
				_sliderSize.Interactable = isNewGame;
				_sliderMax.Interactable = isNewGame;
				_infinityToggle.Interactable = isNewGame;
			}
			void SetIcons(bool isInfinity)
			{
				_iconButton.sprite = isInfinity ? _spriteInfinity : _spriteEmpty;
				_iconInfinity.color = isInfinity ? _colorOn : _colorOff;
			}
			#endregion
		}
	}
	#endregion
}
