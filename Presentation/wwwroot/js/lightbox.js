const lightboxLinks = Array.from(document.querySelectorAll("[data-lightbox]"));

const lightboxOverlay = document.createElement("div");
lightboxOverlay.classList.add("lightbox-overlay");

const lightboxPreviousButton = document.createElement("button");
lightboxPreviousButton.type = "button";
lightboxPreviousButton.classList.add("lightbox-button", "lightbox-button-previous");
lightboxPreviousButton.textContent = "‹";

const lightboxNextButton = document.createElement("button");
lightboxNextButton.type = "button";
lightboxNextButton.classList.add("lightbox-button", "lightbox-button-next");
lightboxNextButton.textContent = "›";

const lightboxImage = document.createElement("img");
lightboxImage.classList.add("lightbox-image");

const lightboxCaption = document.createElement("div");
lightboxCaption.classList.add("lightbox-caption");

let currentImageIndex = 0;

function showLightboxImage(imageIndex) {
    currentImageIndex = imageIndex;

    const selectedLink = lightboxLinks[currentImageIndex];
    const selectedImage = selectedLink.querySelector("img");
    const selectedAltText = selectedImage?.alt ?? "";

    lightboxImage.src = selectedLink.href;
    lightboxImage.alt = selectedAltText;

    lightboxCaption.textContent =
        `${currentImageIndex + 1} of ${lightboxLinks.length} — ${selectedAltText}`;
}

function showPreviousImage(event) {
    event.stopPropagation();

    const previousImageIndex =
        currentImageIndex === 0
            ? lightboxLinks.length - 1
            : currentImageIndex - 1;

    showLightboxImage(previousImageIndex);
}

function showNextImage(event) {
    event.stopPropagation();

    const nextImageIndex =
        currentImageIndex === lightboxLinks.length - 1
            ? 0
            : currentImageIndex + 1;

    showLightboxImage(nextImageIndex);
}

if (lightboxLinks.length > 0) {
    lightboxOverlay.appendChild(lightboxPreviousButton);
    lightboxOverlay.appendChild(lightboxImage);
    lightboxOverlay.appendChild(lightboxNextButton);
    lightboxOverlay.appendChild(lightboxCaption);

    document.body.appendChild(lightboxOverlay);
}

lightboxLinks.forEach((link, index) => {
    link.addEventListener("click", (event) => {
        event.preventDefault();

        showLightboxImage(index);
        lightboxOverlay.classList.add("is-open");
    });
});

lightboxPreviousButton.addEventListener("click", showPreviousImage);
lightboxNextButton.addEventListener("click", showNextImage);

lightboxImage.addEventListener("click", (event) => {
    event.stopPropagation();
});

lightboxOverlay.addEventListener("click", () => {
    lightboxOverlay.classList.remove("is-open");
});

document.addEventListener("keydown", (event) => {
    if (!lightboxOverlay.classList.contains("is-open")) {
        return;
    }

    if (event.key === "Escape") {
        lightboxOverlay.classList.remove("is-open");
    }

    if (event.key === "ArrowLeft") {
        showPreviousImage(event);
    }

    if (event.key === "ArrowRight") {
        showNextImage(event);
    }
});