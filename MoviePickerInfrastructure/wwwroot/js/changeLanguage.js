//function changeLanguage(languageCode) {
//    var currentUrl = window.location.href;
//    var newUrl;

//    // Check if the current URL already contains a query string
//    if (currentUrl.indexOf('?') !== -1) {
//        // URL contains query string
//        newUrl = currentUrl.replace(/(\?|&)ui-culture=[^&]*|$/, '$1ui-culture=' + languageCode);
//    } else {
//        // URL doesn't contain query string
//        newUrl = currentUrl + '?ui-culture=' + languageCode;
//    }

//    // Redirect to the new URL
//    window.location.href = newUrl;
//}




//function changeLanguage(languageCode) {
//    var currentUrl = window.location.href;
//    var newUrl;

//    // Check if the current URL already contains a query string
//    if (currentUrl.indexOf('?') !== -1) {
//        // URL contains query string
//        // Check if ui-culture parameter already exists
//        if (currentUrl.match(/[?&]ui-culture=[^&]*/)) {
//            // Replace the value of the existing ui-culture parameter
//            newUrl = currentUrl.replace(/(ui-culture=)[^&]*/, '$1' + languageCode);
//        } else {
//            // Add ui-culture parameter to existing query string
//            newUrl = currentUrl + '&ui-culture=' + languageCode;
//        }
//    } else {
//        // URL doesn't contain query string
//        newUrl = currentUrl + '?ui-culture=' + languageCode;
//    }

//    // Redirect to the new URL
//    window.location.href = newUrl;
//}


//function changeLanguage(languageCode) {
//    var currentUrl = window.location.href;
//    var newUrl;

//    // Check if the current URL already contains a query string
//    if (currentUrl.indexOf('?') !== -1) {
//        // URL contains query string
//        // Check if ui-culture parameter already exists
//        if (currentUrl.match(/[?&]ui-culture=[^&]*/)) {
//            // Replace the value of the existing ui-culture parameter
//            newUrl = currentUrl.replace(/(ui-culture=)[^&]*/, '$1' + languageCode);
//        } else {
//            // Add ui-culture parameter to existing query string
//            newUrl = currentUrl + '&ui-culture=' + languageCode;
//        }
//    } else {
//        // URL doesn't contain query string
//        newUrl = currentUrl + '?ui-culture=' + languageCode;
//    }

//    // Redirect to the new URL
//    window.location.href = newUrl;
//}


//function changeLanguage(languageCode) {
//    var currentUrl = window.location.href;
//    var newUrl;

//    // Check if the current URL already contains a query string
//    if (currentUrl.indexOf('?') !== -1) {
//        // URL contains query string
//        // Check if ui-culture parameter already exists
//        if (currentUrl.match(/[?&]ui-culture=[^&]*/)) {
//            // Replace the value of the existing ui-culture parameter
//            newUrl = currentUrl.replace(/(ui-culture=)[^&]*/, '$1' + languageCode);
//        } else {
//            // Add ui-culture parameter to existing query string
//            newUrl = currentUrl + '&ui-culture=' + languageCode;
//        }
//    } else {
//        // URL doesn't contain query string
//        newUrl = currentUrl + '?ui-culture=' + languageCode;
//    }

//    // Redirect to the new URL
//    window.location.href = newUrl;
//}

//function changeLanguage(languageCode) {
//    var currentUrl = window.location.href;
//    var newUrl;

//    // Check if the current URL already contains a query string
//    if (currentUrl.indexOf('?') !== -1) {
//        // URL contains query string
//        // Check if ui-culture parameter already exists
//        if (currentUrl.match(/[?&]ui-culture=[^&]*/)) {
//            // Replace the value of the existing ui-culture parameter
//            newUrl = currentUrl.replace(/([?&]ui-culture=)[^&]*/, '$1' + languageCode);
//        } else {
//            // Add ui-culture parameter to existing query string
//            newUrl = currentUrl + '&ui-culture=' + languageCode;
//        }
//    } else {
//        // URL doesn't contain query string
//        newUrl = currentUrl + '?ui-culture=' + languageCode;
//    }

//    // Redirect to the new URL
//    window.location.href = newUrl;
//}





function changeLanguage(languageCode) {
    var currentUrl = window.location.href;
    var newUrl;

    // Check if the current URL already contains a query string
    if (currentUrl.indexOf('?') !== -1) {

        // URL contains query string
        // Check if ui-culture parameter already exists
        if (currentUrl.match(/[?&]ui-culture=[^&]*/)) {
            // Replace the value of the existing ui-culture parameter
            newUrl = currentUrl.replace(/([?&]ui-culture=)[^&]*/, '$1' + languageCode);
        } else {
            // Add ui-culture parameter to existing query string
            newUrl = currentUrl + '&ui-culture=' + languageCode;
        }
    } else {
        // URL doesn't contain query string
        newUrl = currentUrl + '?ui-culture=' + languageCode;
    }

    // Redirect to the new URL
    window.location.href = newUrl;
}





//    function changeLanguage(languageCode) {
//        // Set selected language preference in a cookie
//        document.cookie = "language=" + languageCode + ";path=/";
//    // Redirect to the same page
//    window.location.reload();
//}



//function appendLanguageQueryParam() {
//    var lang = getLanguagePreference();
//    if (lang) {
//        var currentUrl = window.location.href;
//        var separator = currentUrl.includes("?") ? "&" : "?";
//        window.history.replaceState({}, '', currentUrl + separator + 'ui-culture=' + lang);
//    }
//}

//function getLanguagePreference() {
//    var name = "language=";
//    var decodedCookie = decodeURIComponent(document.cookie);
//    var cookieArray = decodedCookie.split(';');
//    for (var i = 0; i < cookieArray.length; i++) {
//        var cookie = cookieArray[i];
//        while (cookie.charAt(0) == ' ') {
//            cookie = cookie.substring(1);
//        }
//        if (cookie.indexOf(name) == 0) {
//            return cookie.substring(name.length, cookie.length);
//        }
//    }
//    return "";
//}

//// Call the function to append language query parameter when the page loads
//window.onload = appendLanguageQueryParam;
