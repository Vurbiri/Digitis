var YandexPlugin =
{
    IsInitializeJS: function () { return func.isInitialize(); },
    IsPlayerJS: function () { return func.isPlayer(); },
    IsLogOnJS: function () { return func.isLogOn(); },
    IsLeaderboardJS: function () { return func.isLeaderboard(); },
    IsDesktopJS: function () { return vars.ysdk.deviceInfo.isDesktop(); },
    IsMobileJS: function () { return vars.ysdk.deviceInfo.isMobile() || vars.ysdk.deviceInfo.isTablet(); },

    GetLangJS: function () {
        var lang;
        if (func.isInitialize())
            lang = vars.ysdk.environment.i18n.lang;
        else
            lang = '';

        return func.toUnityString(lang);
    },
    GetPlayerNameJS: function () {
        var name;
        if (func.isLogOn())
            name = vars.player.getName();
        else
            name = '';

        return func.toUnityString(name);
    },
    GetPlayerAvatarURLJS: function (size) {
        var sSize = UTF8ToString(size);
        var url;
        if (func.isLogOn())
            url = vars.player.getPhoto(sSize);
        else
            url = '';

        return func.toUnityString(url);
    },

    InitYsdkJS: function () {

        if (typeof YaGames === 'undefined' || func.isEmpty(YaGames))
        {
            console.log("-- InitYsdk: not YaGames --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndInitYsdk', 0);
            return;
        }

        YaGames
            .init()
            .then((_ysdk) => {
                console.log("++ Yandex SDK initialized ++");
                vars.ysdk = _ysdk;
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitYsdk', 1);
                
            })
            .catch((message) => {
                console.log("-- InitYsdk: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitYsdk', 0);
            });
    },

    ReadyJS: function () {

         if (!func.isInitialize() || !func.isEmpty(vars.ysdk.features.LoadingAPI)) {
             console.log("-- ReadyJS: not LoadingAPI  --");
            return;
         }

         vars.ysdk.features.LoadingAPI.ready(); 
    },

    InitPlayerJS: function () {

        if (!func.isInitialize()) {
            console.log("-- InitPlayer: not ysdk --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndInitPlayer', 0);
            return;
        }

        vars.ysdk
            .getPlayer()
            .then(_player => {
                vars.player = _player;
                console.log("++ InitPlayer ++");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitPlayer', 1);
            })
            .catch(err => {
                vars.player = null;
                console.log("-- InitPlayer: " + err + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitPlayer', 0);
            });
    },

    LogOnJS: function () {
        if (!func.isPlayer()) {
            console.log("-- Authorization: not player --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 0);
            return;
        }

        if (vars.player.getMode() === 'lite') {
            // игрок не авторизован.
            vars.ysdk.auth
                .openAuthDialog()
                .then(() => {
                    // игрок успешно авторизован
                    vars.ysdk
                        .getPlayer()
                        .then((_player) => {
                            vars.player = _player;
                            // успешная инициализации объекта player.
                            console.log("++ Authorization | Player ++");
                            window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 1);
                        })
                        .catch((message) => {
                            // ошибка при инициализации объекта player.
                            vars.player = null;
                            console.log("-- Authorization | Player: " + message + "  --");
                            window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 0);
                        });
                })
                .catch((message) => {
                    // игрок не авторизован.
                    console.log("-- Authorization: " + message + "  --");
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 0);
                });
        }
        else {
            // игрок уже авторизован
            console.log("++ уже Authorization ++");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 1);
        }

    },

    InitLeaderboardsJS: function () {
        if (!func.isInitialize()) {
            console.log("-- InitLeaderboards: not ysdk --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndInitLeaderboards', 0);
            return;
        }

        vars.ysdk
            .getLeaderboards()
            .then((_lb) => {
                console.log("++ Leaderboards initialized ++");
                vars.lb = _lb;
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitLeaderboards', 1);
            })
            .catch((message) => {
                console.log("-- InitLeaderboards: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitLeaderboards', 0);
            });
    },
    SetScoreJS: function (lbName, score) {
        if (!func.isLeaderboard()) {
            console.log("-- SetScore: not lb --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndSetScore', 0);
            return;
        }

        var lbn = UTF8ToString(lbName);
        vars.lb
            .setLeaderboardScore(lbn, score)
            .then(() => {
                console.log("++ SetScore ++");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndSetScore', 1);
            })
            .catch((message) => {
                console.log("-- SetScore: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndSetScore', 0);
            });
    },
    GetPlayerResultJS: function (lbName) {
        if (!func.isLeaderboard())
        {
            console.log("-- GetPlayerResult: not lb --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndGetPlayerResult', '');
        }

        var lbn = UTF8ToString(lbName);
        vars.lb
            .getLeaderboardPlayerEntry(lbn)
            .then((res) => {
                var obj = { rank: res.rank, score: res.score };
                var json = JSON.stringify(obj);
                console.log("++ GetPlayerResult ++");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndGetPlayerResult', json);
            })
            .catch((message) => {
                var code = message.code;
                console.log("-- GetPlayerResult: " + message.code + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndGetPlayerResult', '');
            });
    },
    GetLeaderboardJS: function (lbName, qTop, includeUser, qAround, size) {
        if (!func.isLeaderboard()) {
            console.log("-- Leaderboards: not lb --");
            window.unityInstance.SendMessage('GetLeaderboard', 'OnEndGetLeaderboard', '');
            return;
        }

        var lbn = UTF8ToString(lbName);
        var sSize = UTF8ToString(size);
        var arg;
        if (includeUser)
            arg = { quantityTop: qTop, includeUser: true, quantityAround: qAround };
        else
            arg = { quantityTop: qTop };

        vars.lb
            .getLeaderboardEntries(lbn, arg)
            .then((res) => {
                if (func.isEmpty(res)) {
                    console.log("++ GetLeaderboard: NULL ++");
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndGetLeaderboard', '');
                } else {
                    var obj = {
                        userRank: res.userRank,
                        table: Array.from(res.entries, (e) => { return { rank: e.rank, score: e.score, name: e.player.publicName, avatarURL: e.player.getAvatarSrc(sSize) } })
                    };
                    var json = JSON.stringify(obj);
                    console.log("++ GetLeaderboard ++");
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndGetLeaderboard', json);
                }
            })
            .catch((message) => {
                console.log("-- Leaderboards: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndGetLeaderboard', '');
            });
    },

    CanShortcutJS: function () {
        if (!func.isInitialize()) {
            console.log("-- CanShortcut: not ysdk --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndCanShortcut', 0);
            return;
        }

        vars.ysdk.shortcut
            .canShowPrompt()
            .then((prompt) => {
                console.log('++ CanShortcut?: ', prompt.canShow);
                if (prompt.canShow)
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndCanShortcut', 1);
                else
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndCanShortcut', 0);
            })
            .catch((message) => {
                console.log("-- CanShortcut: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndCanShortcut', 0);
            });
    },
    CreateShortcutJS: function () {
        if (!func.isInitialize()) {
            console.log("-- CreateShortcut: not ysdk --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndCreateShortcut', 0);
            return;
        }

        vars.ysdk.shortcut
            .showPrompt()
            .then((result) => {
                console.log('++ CreateShortcut?: ', result);
                if (result.outcome === 'accepted')
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndCreateShortcut', 1);
                else
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndCreateShortcut', 0);
            })
            .catch((message) => {
                console.log("-- CreateShortcut: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndCreateShortcut', 0);
            });
    },
    CanReviewJS: function () {
        if (!func.isInitialize()) {
            console.log("-- CanReview: not ysdk --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndCanReview', 0);
            return;
        }

        vars.ysdk.feedback
            .canReview()
            .then(({ value, reason }) => {
                if (value)
                    console.log("++ CanReview: " + value + "  ++");
                else
                    console.log("+- CanReview: " + reason + "  -+");

                window.unityInstance.SendMessage('YandexSDK', 'OnEndCanReview', value ? 1 : 0);
            })
            .catch((message) => {
                console.log("-- CanReview: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndCanReview', 0);
            });
    },
    RequestReviewJS: function () {
        if (!func.isInitialize()) {
            console.log("-- RequestReview: not ysdk --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndRequestReview', 0);
            return;
        }

        vars.ysdk.feedback
            .canReview()
            .then(({ value, reason }) => {
                if (value) {
                    vars.ysdk.feedback
                        .requestReview()
                        .then(({ feedbackSent }) => {
                            console.log("++ RequestReview: " + feedbackSent + "  ++");
                            window.unityInstance.SendMessage('YandexSDK', 'OnEndRequestReview', feedbackSent ? 1 : 0);
                        })
                        .catch((message) => {
                            console.log("-- RequestReview: " + message + "  --");
                            window.unityInstance.SendMessage('YandexSDK', 'OnEndRequestReview', 0);
                        });
                }
                else {
                    console.log("-- RequestReview: " + reason + "  --");
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndRequestReview', 0);
                }
            })
            .catch((message) => {
                console.log("-- RequestReview: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndRequestReview', 0);
            });
    },

    SaveJS: function (key, data) {
        if (!func.isPlayer()) {
            console.log("-- SaveYSDK: not player --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndSave', 0);
            return;
        }

        var sKey = UTF8ToString(key);
        var dt = UTF8ToString(data);
        dt = zip(dt);
        var obj = { [sKey]: dt };
        vars.player
            .setData(obj)
            .then(() => {
                console.log("++ SaveYSDK ++");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndSave', 1);
            })
            .catch((message) => {
                console.log("-- SaveYSDK: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndSave', 0);
            });
    },
    LoadJS: function (key) {
        if (!func.isPlayer()) {
            console.log("-- LoadYSDK: not player --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndLoad', '');
            return;
        }

        var sKey = UTF8ToString(key);
        var keys = [sKey];
        vars.player
            .getData(keys)
            .then((data) => {
                if (func.isEmpty(data)) {
                    console.log("++ LoadYSDK: NULL ++");
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndLoad', '');
                }
                else {
                    console.log("++ LoadYSDK ++");
                    var dt = data[sKey];
                    dt = unzip(dt);
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndLoad', dt);
                }

            })
            .catch((message) => {
                console.log("-- LoadYSDK: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndLoad', '');
            });

    },
    
    ShowFullscreenAdvJS: function () {
        if (!func.isInitialize()) {
            console.log("-- ShowFullscreenAdv: not ysdk --");
            window.unityInstance.SendMessage('YMoney', 'OnEndShowFullscreenAdv', 0);
            return;
        }

        vars.ysdk.adv
            .showFullscreenAdv({
                callbacks: {
                    onOpen: () => {
                        console.log('== ShowFullscreenAdv - Open ==');
                    },
                    onClose: (wasShown) => {
                        console.log("++ ShowFullscreenAdv: " + wasShown + "  ++");
                        window.unityInstance.SendMessage('YMoney', 'OnEndShowFullscreenAdv', wasShown ? 1 : 0);
                    },
                    onError: (error) => {
                        console.log("-- ShowFullscreenAdv: " + error + "  --");
                        window.unityInstance.SendMessage('YMoney', 'OnEndShowFullscreenAdv', 0);
                    }
                }
            });
    },
    ShowRewardedVideoJS: function () {
        if (!func.isInitialize()) {
            console.log("-- ShowRewardedVideo: not ysdk --");
            window.unityInstance.SendMessage('YMoney', 'OnRewardRewardedVideo', 0);
            window.unityInstance.SendMessage('YMoney', 'OnCloseRewardedVideo', 0);
            return;
        }

        var reward = 0;
        vars.ysdk.adv
            .showRewardedVideo({
                callbacks: {
                    onOpen: () => {
                        console.log('== ShowRewardedVideo - Video ad open ==');
                    },
                    onRewarded: () => {
                        console.log('++ ShowRewardedVideo - Rewarded! ++');
                        reward = 1;
                        window.unityInstance.SendMessage('YMoney', 'OnRewardRewardedVideo', 1);
                    },
                    onClose: () => {
                        console.log('== ShowRewardedVideo - Video ad closed ==');
                        window.unityInstance.SendMessage('YMoney', 'OnCloseRewardedVideo', reward);
                    },
                    onError: (error) => {
                        console.log("-- ShowRewardedVideo: " + error + "  --");
                        window.unityInstance.SendMessage('YMoney', 'OnRewardRewardedVideo', 0);
                        window.unityInstance.SendMessage('YMoney', 'OnCloseRewardedVideo', 0);
                    }
                }
            })
    },

    ShowBannerAdvJS: function () {
        if (!func.isInitialize()) {
            console.log("-- ShowBannerAdv: not ysdk --");
            return;
        }

        vars.ysdk.adv.showBannerAdv();
    },

    HideBannerAdvJS: function () {
        if (!func.isInitialize()) {
            console.log("-- HideBannerAdv: not ysdk --");
            return;
        }

        vars.ysdk.adv.hideBannerAdv();
    },

    $func: {
        toUnityString: function (returnStr) {
            if (func.isEmpty(returnStr))
                return null;

            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            return buffer;
        },
        isInitialize: function () { return !func.isEmpty(vars.ysdk); },
        isPlayer: function () { return func.isInitialize() && !func.isEmpty(vars.player); },
        isLogOn: function () { return func.isPlayer() && !(vars.player.getMode() === 'lite'); },
        isLeaderboard: function () { return func.isLogOn() && !func.isEmpty(vars.lb); },

        isEmpty: function (obj) {
            if (!obj)
                return true;

            return Object.keys(obj).length === 0;
        },
    },
    $vars:
    {
        ysdk: null,
        player: null,
        lb: null,
    },
}
    
autoAddDeps(YandexPlugin, '$func');
autoAddDeps(YandexPlugin, '$vars');
mergeInto(LibraryManager.library, YandexPlugin);

