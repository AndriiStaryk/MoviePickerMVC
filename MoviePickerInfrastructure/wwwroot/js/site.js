// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




const $cards = document.querySelectorAll('.card');

function rotateToMouse(e) {
    const $card = e.currentTarget;
    const bounds = $card.getBoundingClientRect();
    const mouseX = e.clientX;
    const mouseY = e.clientY;
    const leftX = mouseX - bounds.x;
    const topY = mouseY - bounds.y;
    const center = {
        x: leftX - bounds.width / 2,
        y: topY - bounds.height / 2
    }
    const distance = Math.sqrt(center.x ** 2 + center.y ** 2);

    $card.style.transform = `
    scale3d(1.07, 1.07, 1.07)
    rotate3d(
      ${center.y / 100},
      ${-center.x / 100},
      0,
      ${Math.log(distance) * 2}deg
    )
  `;

  //  $card.querySelector('.glow').style.backgroundImage = `
  //  radial-gradient(
  //    circle at
  //    ${center.x * 2 + bounds.width / 2}px
  //    ${center.y * 2 + bounds.height / 2}px,
  //    #ffffff55,
  //    #0000000f
  //  )
  //`;
}

$cards.forEach($card => {
    $card.addEventListener('mouseenter', rotateToMouse);
    $card.addEventListener('mouseleave', () => {
        $card.style.transform = '';
        $card.style.background = '';
    });
});






//beatiful animation but not working properly
//const $cards = document.querySelectorAll('.card');

//function rotateToMouse(e) {
//    const $card = e.currentTarget;
//    const bounds = $card.getBoundingClientRect();
//    const mouseX = e.clientX;
//    const mouseY = e.clientY;
//    const leftX = mouseX - bounds.x;
//    const topY = mouseY - bounds.y;
//    const center = {
//        x: leftX - bounds.width / 2,
//        y: topY - bounds.height / 2
//    }
//    const distance = Math.sqrt(center.x ** 2 + center.y ** 2);

//    $card.style.transform = `
//    scale3d(1.07, 1.07, 1.07)
//    rotate3d(
//      ${center.y / 100},
//      ${-center.x / 100},
//      0,
//      ${Math.log(distance) * 2}deg
//    )
//  `;

//    $card.querySelector('.glow').style.backgroundImage = `
//    radial-gradient(
//      circle at
//      ${center.x * 2 + bounds.width / 2}px
//      ${center.y * 2 + bounds.height / 2}px,
//      #ffffff55,
//      #0000000f
//    )
//  `;
//}

//function stopRotation(e) {
//    const $card = e.currentTarget;
//    $card.style.transform = '';
//}

//$cards.forEach($card => {
//    $card.addEventListener('mousemove', rotateToMouse);
//    $card.addEventListener('mouseleave', stopRotation);

//    // Add event listener to each button to stop rotation when mouse enters
//    const $buttons = $card.querySelectorAll('.btn');
//    $buttons.forEach($button => {
//        $button.addEventListener('mouseenter', (e) => {
//            stopRotation({ currentTarget: $card });
//        });
//    });
//});


// Function to retrieve culture value from cookies
//function getCultureFromCookies() {
//    const cookieValue = document.cookie
//        .split('; ')
//        .find(row => row.startsWith('culture='))
//        ?.split('=')[1];
//    return cookieValue || 'en-US'; // Default to 'en-US' if culture cookie is not found
//}

//function updateUrlsWithCulture() {
//    const culture = getCultureFromCookies();

//    // Update all anchor tags with the culture parameter
//    document.querySelectorAll('a').forEach(anchor => {
//        const href = anchor.getAttribute('href');
//        if (href && !href.includes('ui-culture=')) {
//            const separator = href.includes('?') ? '&' : '?';
//            anchor.setAttribute('href', `${href}${separator}ui-culture=${culture}`);
//        }
//    });

//    // Update all form actions with the culture parameter
//    document.querySelectorAll('form').forEach(form => {
//        const action = form.getAttribute('action');
//        if (action && !action.includes('ui-culture=')) {
//            const separator = action.includes('?') ? '&' : '?';
//            form.setAttribute('action', `${action}${separator}ui-culture=${culture}`);
//        }
//    });
//}

//// Call the functions when the page loads
//window.onload = function () {
//    updateUrlsWithCulture();
//};