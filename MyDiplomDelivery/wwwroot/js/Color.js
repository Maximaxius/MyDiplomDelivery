let selector = document.getElementsByClassName("status")
for (el of selector) {
    switch (el.innerText) {
        case "Todo":
            el.style.backgroundColor = "yellow";
            break;
        case "InProgress":
            el.style.backgroundColor = "#3ed9f3";
            break;
        case "Completed":
            el.style.backgroundColor = "green";
            break;
        case "Cancelled":
            el.style.backgroundColor = "red";
            break;
        case "Closed":
            el.style.backgroundColor = "#ff5cd6";
            break;
    }
}