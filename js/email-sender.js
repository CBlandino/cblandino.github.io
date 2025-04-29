document.addEventListener('DOMContentLoaded', function() {
  const contactForm = document.getElementById('contact-form');
  const status = document.getElementById('form-status');

  if (contactForm) {
    contactForm.addEventListener('submit', function(e) {
      e.preventDefault();
      
      const submitBtn = contactForm.querySelector('button[type="submit"]');
      const originalText = submitBtn.textContent;
      submitBtn.textContent = 'Sending...';
      submitBtn.disabled = true;

      // Send the email
      emailjs.sendForm('Portfolio_Contact', 'portfolio_contact_form', contactForm)
        .then(function(response) {
          status.innerHTML = 'Message sent successfully! I\'ll get back to you soon.';
          status.className = 'form-status success';
          contactForm.reset();
        }, function(error) {
          status.innerHTML = 'Failed to send message. Please email me directly at christopherblandino0@gmail.com';
          status.className = 'form-status error';
        })
        .finally(function() {
          submitBtn.textContent = originalText;
          submitBtn.disabled = false;
          
          // Show status message
          status.style.display = 'block';
          status.scrollIntoView({ behavior: 'smooth' });
          
          // Hide after 5 seconds
          setTimeout(() => {
            status.style.display = 'none';
          }, 5000);
        });
    });
  }
});