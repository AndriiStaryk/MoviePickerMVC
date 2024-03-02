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
