window.addEventListener('scroll', function() {
    const sections = document.querySelectorAll('section');
    const taskbarLinks = document.querySelectorAll('.taskbar .taskbar-link');
    const scrollPosition = window.scrollY + 100; // Adding offset for better detection
  
    let currentSection = '';
    sections.forEach(section => {
      const sectionTop = section.offsetTop;
      const sectionHeight = section.clientHeight;
      
      if (scrollPosition >= sectionTop && scrollPosition < sectionTop + sectionHeight) {
        currentSection = section.getAttribute('id');
      }
    });
  
    taskbarLinks.forEach(link => {
      link.classList.remove('underline');
      if (link.getAttribute('href') === `#${currentSection}`) {
        link.classList.add('underline');
      }
    });
  });
  
  // Add smooth scroll behavior
  document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function(e) {
      e.preventDefault();
      
      const targetId = this.getAttribute('href');
      const targetElement = document.querySelector(targetId);
      
      if (targetElement) {
        window.scrollTo({
          top: targetElement.offsetTop - 80, // Account for fixed header
          behavior: 'smooth'
        });
        
        // Update URL without jumping
        history.pushState(null, null, targetId);
      }
    });
  });