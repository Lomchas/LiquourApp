// Funcionalidad para verificación de edad
document.addEventListener('DOMContentLoaded', function () {
    // Verificar si ya se ha verificado la edad anteriormente
    const ageVerified = localStorage.getItem('ageVerified');
    
    // Si no se ha verificado la edad y no estamos en la página de verificación
    if (!ageVerified && !window.location.href.includes('/Account/VerifyAge')) {
        // Redirigir a la página de verificación de edad
        if (!window.location.href.includes('/Account/Login') && 
            !window.location.href.includes('/Account/Register')) {
            window.location.href = '/Account/VerifyAge';
        }
    }

    // Validación de formulario de registro
    const registerForm = document.querySelector('form[action*="Register"]');
    if (registerForm) {
        registerForm.addEventListener('submit', function (e) {
            const dateOfBirthInput = document.getElementById('DateOfBirth');
            if (dateOfBirthInput) {
                const dob = new Date(dateOfBirthInput.value);
                const today = new Date();
                let age = today.getFullYear() - dob.getFullYear();
                const monthDiff = today.getMonth() - dob.getMonth();
                
                if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
                    age--;
                }
                
                if (age < 18) {
                    e.preventDefault();
                    alert('Debes ser mayor de 18 años para registrarte.');
                }
            }
        });
    }

    // Validación de formulario de verificación de edad
    const verifyAgeForm = document.querySelector('form[action*="VerifyAge"]');
    if (verifyAgeForm) {
        verifyAgeForm.addEventListener('submit', function (e) {
            const dateOfBirthInput = document.getElementById('dateOfBirth');
            if (dateOfBirthInput) {
                const dob = new Date(dateOfBirthInput.value);
                const today = new Date();
                let age = today.getFullYear() - dob.getFullYear();
                const monthDiff = today.getMonth() - dob.getMonth();
                
                if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
                    age--;
                }
                
                if (age >= 18) {
                    // Guardar en localStorage que la edad ha sido verificada
                    localStorage.setItem('ageVerified', 'true');
                }
            }
        });
    }
});