

document.addEventListener('DOMContentLoaded', function () {
    const sectionId = window.location.hash ? window.location.hash.substring(1) : 'home';
    showSection(sectionId);
});

window.addEventListener('hashchange', function () {
    const sectionId = window.location.hash.substring(1);
    showSection(sectionId);
});

//shows the sections in the homepage (contact us, about us....)
function showSection(sectionId) {
    document.querySelectorAll('.content-section').forEach(section => {
        section.style.display = 'none';
    });
    const section = document.getElementById(sectionId);
    if (section) {
        section.style.display = 'block';
    } else {
        console.warn(`Element with ID ${sectionId} not found.`);
    }
}
// if the user is logged in, it takes him to the events page to join an event, if not logged in it directs him to the login page
function applyForEvent() {
    fetch('/Account/IsLoggedIn')
        .then(response => response.json())
        .then(data => {
            if (data.isLoggedIn) {
                window.location.href = '/Event/EIndex';
            } else {
                window.location.href = '/Account/Login';
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}





function isLoggedIn() {
    const profileInfo = document.getElementById('profile-info');
    return profileInfo ? profileInfo.style.display !== 'none' : false;
}

let currentIndex = 0;
const images = document.querySelectorAll('.hero img');
const imageCount = images.length;

function showNextImage() {
    if (images.length === 0) {
        console.warn("No images found in the '.hero img' selector.");
        return;
    }
    if (images[currentIndex]) {
        images[currentIndex].classList.remove('active');
    }
    currentIndex = (currentIndex + 1) % imageCount;
    if (images[currentIndex]) {
        images[currentIndex].classList.add('active');
    }
}

setInterval(showNextImage, 3000);
