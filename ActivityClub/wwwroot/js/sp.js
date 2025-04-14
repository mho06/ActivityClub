document.addEventListener("DOMContentLoaded", function () {
    const signupForm = document.getElementById("signupForm");
    signupForm.addEventListener("submit", function (event) {
        const password = document.getElementById("password").value;
        const confirmPassword = document.getElementById("confirmPassword").value;

        if (password !== confirmPassword) {
            event.preventDefault();
            alert("Passwords do not match. Please try again.");
        }
    });
});

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

