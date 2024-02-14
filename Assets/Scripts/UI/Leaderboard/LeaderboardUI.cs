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
    [Range(1, 20), SerializeField] private int _maxTop = 20;
    [Range(1, 10), SerializeField] private int _maxAround = 10;
    [SerializeField] private string _lbName = "lbDigitis";

    private YandexSDK _ysdk;
    private readonly List<LeaderboardRecordUI> _records = new();
    private GameObject _separator;

    private const int TOP = 5;

    public void Start()
    {
        _ysdk = YandexSDK.InstanceF;
        InitializeAsync().Forget();
    }

    public async UniTaskVoid InitializeAsync()
    {
        if (!_ysdk.IsLeaderboard) return;

        int UserRank = 0;
        bool playerInTable = false;
        var player = await _ysdk.GetPlayerResult(_lbName);
        if (player.Result)
        {
            UserRank = player.Value.Rank;
            playerInTable = UserRank > 0;
        }
        if (playerInTable)
            if (UserRank <= (_maxTop - _maxAround))
                playerInTable = false;

        var leaderboard = await _ysdk.GetLeaderboard(_lbName, _maxTop, playerInTable, _maxAround);
        if (!leaderboard.Result)
            return;
        if (playerInTable)
            UserRank = leaderboard.Value.UserRank;

        RectTransform content = _rect.content;
        ScrollToPlayer(CreateTable());

        #region Local Functions
        RectTransform CreateTable()
        {
            int preRank = 0;
            bool isPlayer;
            RectTransform recordTransform = null;
            LeaderboardRecordUI recordUI;

            foreach (var record in leaderboard.Value.Table)
            {
                if (record.Rank - preRank > 1)
                    _separator = Instantiate(_recordSeparator, content);
                preRank = record.Rank;
                isPlayer = record.Rank == UserRank;

                recordUI = Instantiate(_record, content);
                recordUI.Setup(record, isPlayer);
                _records.Add(recordUI);
                if (isPlayer)
                    recordTransform = recordUI.GetComponent<RectTransform>();
            }

            return recordTransform;
        }

        void ScrollToPlayer(RectTransform recordTransform)
        {
            gameObject.SetActive(true);

            if (recordTransform != null)
            {
                RectTransform viewport = _rect.viewport;
                Canvas.ForceUpdateCanvases();

                float maxOffset = content.rect.height - viewport.rect.height;
                float offset = -viewport.rect.height / 2f - recordTransform.localPosition.y;

                if (offset < 0) offset = 0;
                else if (offset > maxOffset) offset = maxOffset;

                content.localPosition = new Vector2(0, offset);
            }
        }

        #endregion
    }

    public async UniTask<bool> TrySetScoreAndReward(int points)
    {
        if(points <= 0) 
            return false;
        if (!_ysdk.IsLeaderboard) 
            return false;

        var player = await _ysdk.GetPlayerResult(_lbName);
        if (player.Result)
            if (player.Value.Score >= points) 
                return false;

        if (!await _ysdk.SetScore(_lbName, points)) 
            return false;

        await Reward();

        return true;

        async UniTask Reward()
        {
            await UniTask.Delay(250, true);

            player = await _ysdk.GetPlayerResult(_lbName);
            if (!player.Result) 
                return;
            if (player.Value.Rank <= 0)
                return;

            if (player.Value.Rank > TOP)
                Message.BannerKey("PersonalRecord");
            else
                Message.BannerKey("Top");
        }
    }

    /*
    public async UniTaskVoid ReInitializeAsync()
    {
        if (!_ysdk.IsLeaderboard) return;

        int UserRank = 0;
        bool playerInTable = false;
        var player = await _ysdk.GetPlayerResult(_lbName);
        if (player.Result)
        {
            UserRank = player.Value.Rank;
            playerInTable = UserRank > 0;
        }
        if (playerInTable)
            if (UserRank <= (_maxTop - _maxAround))
                playerInTable = false;

        var leaderboard = await _ysdk.GetLeaderboard(_lbName, _maxTop, playerInTable, _maxAround);
        if (!leaderboard.Result)
            return;
        if (playerInTable)
            UserRank = leaderboard.Value.UserRank;

        RectTransform content = _rect.content;
        ScrollToPlayer(CreateTable());

        #region Local Functions
        RectTransform CreateTable()
        {
            int preRank = 0;
            bool isPlayer;
            RectTransform recordTransform = null;
            LeaderboardRecordUI recordUI;
            int childCount = _records.Count;
            int i = 0;

            if (_separator != null)
            {
                _separator.SetActive(false);
                _separator.transform.SetAsLastSibling();
            }

            foreach (var record in leaderboard.Value.Table)
            {
                isPlayer = record.Rank == UserRank;

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
                    recordTransform = recordUI.GetComponent<RectTransform>();

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

            return recordTransform;
        }

        void ScrollToPlayer(RectTransform recordTransform)
        {
            gameObject.SetActive(true);

            if (recordTransform != null)
            {
                RectTransform viewport = _rect.viewport;
                Canvas.ForceUpdateCanvases();

                float maxOffset = content.rect.height - viewport.rect.height;
                float offset = -viewport.rect.height / 2f - recordTransform.localPosition.y;

                if (offset < 0) offset = 0;
                else if (offset > maxOffset) offset = maxOffset;

                content.localPosition = new Vector2(0, offset);
            }
        }

        #endregion
    }
    */
}
