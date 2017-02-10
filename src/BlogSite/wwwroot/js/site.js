// Write your Javascript code.
/* Set the width of the side navigation to 250px */
function openNav() {
    var sidebar = document.getElementById("mySidenav");
    if (sidebar != null) {
        sidebar.style.width = "250px";
    }
}

/* Set the width of the side navigation to 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}