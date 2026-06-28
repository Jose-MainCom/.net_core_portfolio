const statusMessage = document.getElementById("status-message");

if (statusMessage) {

    requestAnimationFrame(() => {
        statusMessage.classList.add("is-visible");
    });

    setTimeout(() => {
        statusMessage.classList.remove("is-visible");
    }, 3500);
}