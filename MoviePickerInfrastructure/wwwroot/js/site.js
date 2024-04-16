// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    const genrePicker = document.getElementById("genrePicker");
    const addGenreBtn = document.getElementById("addGenreBtn");
    const selectedGenresContainer = document.getElementById("selectedGenres");

    addGenreBtn.addEventListener("click", function () {
        const selectedGenre = genrePicker.value;
        if (selectedGenre !== "") {
            addTag(selectedGenre);
        }
    });

    function addTag(genre) {

        const existingTags = selectedGenresContainer.getElementsByClassName("tag");
        for (let i = 0; i < existingTags.length; i++) {
            if (existingTags[i].textContent === genre) {
                // Genre already exists, do not add again
                return;
            }
        }

        const tag = document.createElement("div");
        tag.classList.add("tag");
        tag.textContent = genre;
        tag.addEventListener("click", function () {
            tag.remove();
        });

        selectedGenresContainer.appendChild(tag);
    }
});




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

    $card.querySelector('.glow').style.backgroundImage = `
    radial-gradient(
      circle at
      ${center.x * 2 + bounds.width / 2}px
      ${center.y * 2 + bounds.height / 2}px,
      #ffffff55,
      #0000000f
    )
  `;
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