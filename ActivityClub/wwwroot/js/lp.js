
//filling the form of the login
document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.querySelector("form");
    loginForm.addEventListener("submit", function (event) {
        const emailInput = document.getElementById("email");
        const passwordInput = document.getElementById("password");

        if (!emailInput.value || !passwordInput.value) {
            event.preventDefault();
            alert("Please fill in both fields.");
        }
    });
});

//function for the eye toggle, that hides and shows the password
document.addEventListener('DOMContentLoaded', function () {
    var togglePassword = document.querySelectorAll('.toggle-password');

    togglePassword.forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            var input = document.querySelector(this.getAttribute('toggle'));
            if (input.getAttribute('type') === 'password') {
                input.setAttribute('type', 'text');
                this.classList.add('fa-eye-slash');
                this.classList.remove('fa-eye');
            } else {
                input.setAttribute('type', 'password');
                this.classList.add('fa-eye');
                this.classList.remove('fa-eye-slash');
            }
        });
    });
});
