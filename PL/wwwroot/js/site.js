let selectedForm = null;

$(document).on("click", ".delete-btn", function () {
    selectedForm = $(this).closest("form");

    $("#overlay").fadeIn(150);
    $("#deleteBox").fadeIn(200);
});

$("#cancelDelete").on("click", function () {
    selectedForm = null;

    $("#deleteBox").fadeOut(150);
    $("#overlay").fadeOut(200);
});

$("#confirmDelete").on("click", function () {
    if (selectedForm) {
        selectedForm.submit();
    }
});
$("#overlay").on("click", function () {
    $("#cancelDelete").click();
});