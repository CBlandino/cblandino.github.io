function locationClicked(location) 
{
    // Handle the click event for the location
    alert("Clicked on " + location +", this location currently does not exist. Apologies for the inconvenience ");

    // Change the color of the filling circle
    var circleId = "circle" + location.split(' ').join('');
    var circle = document.getElementById(circleId);
    if (circle) {
        circle.style.backgroundColor = "#007bff"; // Change to the desired color
    }
}

function enlargeMap() 
{
    var map = document.getElementById('map');
    map.classList.toggle('enlarged');
}
