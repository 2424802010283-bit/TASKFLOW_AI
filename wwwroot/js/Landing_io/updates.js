document.addEventListener("DOMContentLoaded", function () {

    const grid = document.getElementById("updatesGrid");
    const next = document.getElementById("updatesNext");
    const prev = document.getElementById("updatesPrev");

    if (!grid) return;

    next.addEventListener("click", function () {

        grid.scrollBy({
            left: 430,
            behavior: "smooth"
        });

    });

    prev.addEventListener("click", function () {

        grid.scrollBy({
            left: -430,
            behavior: "smooth"
        });

    });

});
</script >