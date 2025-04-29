document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('.contact-form');
    const status = document.getElementById('form-status');
  
    if (form) {
      form.addEventListener('submit', function(e) {
        e.preventDefault();
        
        // Change button text
        const submitBtn = form.querySelector('button[type="submit"]');
        const originalBtnText = submitBtn.textContent;
        submitBtn.textContent = 'Sending...';
        submitBtn.disabled = true;
  
        // Prepare form data
        const formData = new FormData(form);
        
        // Send form data
        fetch(form.action, {
          method: 'POST',
          body: formData,
          headers: {
            'Accept': 'application/json'
          }
        })
        .then(response => {
          if (response.ok) {
            // Success message
            status.innerHTML = 'Thank you! Your message has been sent.';
            status.className = 'form-status success';
            form.reset();
          } else {
            throw new Error('Network response was not ok');
          }
        })
        .catch(error => {
          // Error message
          status.innerHTML = 'Oops! There was a problem sending your message. Please try again later.';
          status.className = 'form-status error';
          console.error('Error:', error);
        })
        .finally(() => {
          submitBtn.textContent = originalBtnText;
          submitBtn.disabled = false;
          
          // Scroll to status message
          status.scrollIntoView({ behavior: 'smooth', block: 'center' });
          
          // Hide message after 5 seconds
          setTimeout(() => {
            status.style.display = 'none';
          }, 5000);
        });
      });
    }
  });