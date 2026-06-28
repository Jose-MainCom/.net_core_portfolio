const menuButton = document.querySelector(".site-menu-button");
const navigationLinks = document.querySelector("#site-navigation-links");

if (menuButton && navigationLinks) {
    menuButton.addEventListener("click", () => {
        const isExpanded =
            menuButton.getAttribute("aria-expanded") === "true";

        menuButton.setAttribute(
            "aria-expanded",
            (!isExpanded).toString()
        );

        navigationLinks.classList.toggle("is-open");
    });
}