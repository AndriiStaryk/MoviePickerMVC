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






