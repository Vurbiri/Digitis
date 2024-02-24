using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [Space]
    [SerializeField] private LeaderboardRecordUI _record;
    [SerializeField] private GameObject _recordSeparator;
    [Space]
    [SerializeField] private ScrollRect _rect;
    [Space]
    [SerializeField] private AvatarSize _avatarSize = AvatarSize.Medium;
    [Range(1, 20), SerializeField] private int _maxTop = 20;
    [Range(1, 10), SerializeField] private int _maxAround = 10;

    private YandexSDK YSDK => YandexSDK.Instance;

    private readonly List<LeaderboardRecordUI> _records = new();
    private GameObject _separator;
    RectTransform _recordTransform = null;

    private const int TOP = 5;

    private void OnEnable()
    {
        ScrollToPlayer();
    }

    private void Start()
    {
        InitializeAsync().Forget();
    }

    public async UniTask TryReward(bool reInitialize = false)
    {
        if (!YSDK.IsLeaderboard) return;

        await UniTask.Delay(150, true);

        var player = await YSDK.GetPlayerResult();
        if (!player.Result)
            return;
        if (player.Value.Rank <= 0)
            return;

        if (reInitialize)
            await ReInitializeAsync(player.Value.Rank);

        if (player.Value.Rank > TOP)
            Message.BannerKey("PersonalRecord");
        else
            Message.BannerKey("Top");
    }

    private async UniTask InitializeAsync(int userRank = 0)
    {
        if (!YSDK.IsLeaderboard) return;

        bool playerInTable = userRank > 0;
        if (!playerInTable)
        {
            var player = await YSDK.GetPlayerResult();
            if (player.Result)
            {
                userRank = player.Value.Rank;
                playerInTable = userRank > 0;
            }
        }
        if (playerInTable)
            if (userRank <= (_maxTop - _maxAround))
                playerInTable = false;

        var leaderboard = await YSDK.GetLeaderboard(_maxTop, playerInTable, _maxAround, _avatarSize);
        if (!leaderboard.Result)
            return;
        if (playerInTable)
            userRank = leaderboard.Value.UserRank;

        CreateTable();
        ScrollToPlayer();

        #region Local Functions
        RectTransform CreateTable()
        {
            int preRank = 0;
            bool isPlayer;
            RectTransform content = _rect.content;
            LeaderboardRecordUI recordUI;
            _recordTransform = null;

            foreach (var record in leaderboard.Value.Table)
            {
                if (record.Rank - preRank > 1)
                    _separator = Instantiate(_recordSeparator, content);
                preRank = record.Rank;
                isPlayer = record.Rank == userRank;

                recordUI = Instantiate(_record, content);
                recordUI.Setup(record, isPlayer);
                _records.Add(recordUI);
                if (isPlayer)
                    _recordTransform = recordUI.GetComponent<RectTransform>();
            }

            return _recordTransform;
        }
        #endregion
    }

    private async UniTask ReInitializeAsync(int userRank)
    {
        if (_rect.content.childCount == 0)
        {
            await InitializeAsync(userRank);
            return;
        }

        Debug.Log("ReInitializeAsync");
        var leaderboard = await YSDK.GetLeaderboard(_maxTop, userRank > (_maxTop - _maxAround), _maxAround, _avatarSize);
        if (!leaderboard.Result)
            return;

        CreateTable();
        ScrollToPlayer();

        #region Local Functions
        void CreateTable()
        {
            int preRank = 0;
            bool isPlayer;
            RectTransform content = _rect.content;
            LeaderboardRecordUI recordUI;
            int childCount = _records.Count;
            int i = 0;
            _recordTransform = null;

            if (_separator != null)
            {
                _separator.SetActive(false);
                _separator.transform.SetAsLastSibling();
            }

            foreach (var record in leaderboard.Value.Table)
            {
                isPlayer = record.Rank == userRank;

                if (i < childCount)
                {
                    recordUI = _records[i++];
                }
                else
                {
                    recordUI = Instantiate(_record, content);
                    _records.Add(recordUI);
                }
                recordUI.Setup(record, isPlayer);
                if (isPlayer)
                    _recordTransform = recordUI.GetComponent<RectTransform>();

                if (record.Rank - preRank > 1)
                {
                    if (_separator == null)
                        _separator = Instantiate(_recordSeparator, content);
                    _separator.transform.SetSiblingIndex(recordUI.transform.GetSiblingIndex());
                    _separator.SetActive(true);
                }
                preRank = record.Rank;

            }

            for (; i < childCount; i++)
                _records[i].gameObject.SetActive(false);
        }
        #endregion
    }

    private void ScrollToPlayer()
    {
        if (_recordTransform == null || !gameObject.activeInHierarchy)
            return;

        Canvas.ForceUpdateCanvases();

        RectTransform content = _rect.content;
        RectTransform viewport = _rect.viewport;
        float maxOffset = content.rect.height - viewport.rect.height;
        float offset = -viewport.rect.height / 2f - _recordTransform.localPosition.y;

        if (offset < 0) offset = 0;
        else if (offset > maxOffset) offset = maxOffset;

        content.localPosition = new Vector2(0, offset);
    }

}
