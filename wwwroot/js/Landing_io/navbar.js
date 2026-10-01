const navbar = document.querySelector(".navbar");
let ticking = false;

function updateNavbar() {
    const y = window.scrollY;

    if (y > 40) {
        navbar.classList.add("is-scrolled");
    } else if (y <= 10) {
        navbar.classList.remove("is-scrolled");
    }
    // Từ 10 đến 40px giữ nguyên trạng thái, tránh bị nhấp nháy

    ticking = false;
}

window.addEventListener("scroll", () => {
    if (!ticking) {
        requestAnimationFrame(updateNavbar);
        ticking = true;
    }
}, { passive: true });

updateNavbar();