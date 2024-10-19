// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

setTimeout(function () {
    let alertElement = document.getElementById("alertMessage");
    if (alertElement) {
        alertElement.style.display = 'none';
    }
}, 5000);
// 5 seconds