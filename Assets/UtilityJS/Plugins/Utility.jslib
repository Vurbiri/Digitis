var UtilityPlugin =
{
    IsMobileUnityJS: function () {
        return Module.SystemInfo.mobile;
    },
    LogJS: function (msg) {
        console.log(UTF8ToString(msg));
    },
    ErrorJS: function (msg) {
        console.error(UTF8ToString(msg));
    },

    SetStorageJS: function (key, data) {
        try {
            var sKey = UTF8ToString(key);
            var dt = UTF8ToString(data);
            window.localStorage.setItem(sKey, btoa(dt));
            console.log("++ SetStorage  ++");
            return true;
        }
        catch (e) {
            console.log("-- SetStorage: " + e + " --");
            return false;
        }
    },
    GetStorageJS: function (key) {

        if (window.localStorage.length === 0) {
            console.log("-- GetStorage: Storage Empty --");
            return '';
        }

        var value;
        try {
            var sKey = UTF8ToString(key);
            value = window.localStorage.getItem(sKey);
        }
        catch (e) {
            console.log("-- GetStorages: " + e +" --");
            return '';
        }
        if (func.isEmpty(value)) {
            console.log("-- GetStorages: NULL --");
            return '';
        }
        else {
            console.log("++ GetStorages ++");
            return func.toUnityString(atob(value));
        }
    },
    IsStorageJS: function () {
        var test = 'teststorage';
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            console.log("++ IsStorage: true ++");
            return true;
        } catch (e) {
            console.log("-- IsStorage: " + e + " --");
            return false;
        }
    },


    SetCookiesJS: function (key, data) {
        var sKey = UTF8ToString(key);
        var dt = UTF8ToString(data);

        var expires = new Date();
        try {
            expires.setFullYear(expires.getFullYear() + 1);
            document.cookie = sKey + "=" + (dt ? btoa(dt) : "") + ";expires=" + expires.toGMTString();
            console.log("++ SetCookies  ++");
            return true;
        }
        catch (e) {
            console.log("-- SetCookies: " + e + " --");
            return false;
        }
    },
    GetCookiesJS: function (key) {
        var value;
        try {
            var sKey = UTF8ToString(key);
            var cookies = {};
            var items = document.cookie.split("; ");
            for (var ii = 0; ii < items.length; ii++) {
                var keyValuePair = items[ii].split("=");
                cookies[keyValuePair[0]] = keyValuePair[1];
            }
            value = cookies[sKey];
        }
        catch (e) {
            console.log("-- GetCookies: " + e + " --");
            return '';
        }
        if (func.isEmpty(value)) {
            console.log("-- GetCookies NULL --");
            return '';
        }
        else {
            
            console.log("++ GetCookies  ++");
            return func.toUnityString(atob(value));
        }
    },
    IsCookiesJS: function () {
        let isCookieEnabled = (window.navigator.cookieEnabled) ? true : false;
        if (typeof window.navigator.cookieEnabled == "undefined" && !isCookieEnabled) {
            document.cookie = "testcookie";
            isCookieEnabled = (document.cookie.indexOf("testcookie") != -1) ? true : false;
        }
        console.log("== GetCookies: " + isCookieEnabled + " ++");
        return isCookieEnabled;
    },
	
	$func: {
        isEmpty: function (obj) {
            if (!obj) return true;

            return Object.keys(obj).length === 0;
        },
		
		toUnityString: function (returnStr) {
            if (func.isEmpty(returnStr))
                return null;

            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            return buffer;
        },
    },

}

autoAddDeps(UtilityPlugin, '$func');
mergeInto(LibraryManager.library, UtilityPlugin);

