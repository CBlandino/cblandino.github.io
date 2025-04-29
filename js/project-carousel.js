// Updated project-carousel.js
document.addEventListener('DOMContentLoaded', function() {
    const projects = [
      {
        title: "Python-P5",
        description: "Java virtual processor with memory management and file tokenization",
        background: "linear-gradient(135deg, #2c3e50, #4a6491)",
        textColor: "#ffffff",
        links: [
          { text: "Live Demo", url: "https://cblandino.github.io/Python-P5/" },
          { text: "GitHub", url: "https://github.com/cblandino/Python-P5" }
        ]
      },
      {
        title: "ALPHA (WIP)",
        description: "Modular virtual processor for digital systems",
        background: "linear-gradient(135deg, #2c3e50, #4a6491)",
        textColor: "#ffffff",
        links: [
          { text: "Live Demo", url: "https://cblandino.github.io/ALPHA" },
          { text: "GitHub", url: "https://github.com/cblandino/ALPHA(revamped)" }
        ]
      },
      {
        title: "Text-Based Compiler",
        description: "Java compiler demonstrating software development principles",
        background: "linear-gradient(135deg, #2c3e50, #4a6491)",
        textColor: "#ffffff",
        links: [
          { text: "GitHub", url: "https://github.com/cblandino/Scheme" }
        ]
      },
      {
        title: "Virtual OS",
        description: "Multi-threaded ToDo application with task management",
        background: "linear-gradient(135deg, #2c3e50, #4a6491)",
        textColor: "#ffffff",
        links: [
          { text: "GitHub", url: "https://github.com/cblandino/Virtual-MultiThreaded-OperatingSystem" }
        ]
      },
      {
        title: "Virtual Processor",
        description: "Weather forecasting application using public APIs",
        background: "linear-gradient(135deg, #2c3e50, #4a6491)",
        textColor: "#ffffff",
        links: [
          { text: "GitHub", url: "https://github.com/cblandino/VirtualProcessor" }
        ]
      },
      {
        title: "DamienSweetJar",
        description: "Weather application with real-time data visualization",
        background: "linear-gradient(135deg, #2c3e50, #4a6491)",
        textColor: "#ffffff",
        links: [
          { text: "GitHub", url: "https://github.com/cblandino/ASP---DamienSweetJar" }
        ]
      }
    ];

    const carousel = document.querySelector('.project-carousel');
    const dotsContainer = document.querySelector('.project-dots');
    let currentSlide = 0;

    // Create slides
    projects.forEach((project, index) => {
      const slide = document.createElement('div');
      slide.className = 'project-slide';
      slide.style.background = project.background;
      slide.style.color = project.textColor;
      slide.style.transform = `translateX(${index * 100}%)`;
      
      const content = document.createElement('div');
      content.className = 'project-slide-content';
      
      // Create links HTML
      let linksHTML = '';
      project.links.forEach(link => {
        const btnClass = link.text === "Live Demo" ? "project-btn-primary" : "project-btn-secondary";
        linksHTML += `<a href="${link.url}" target="_blank" class="project-btn ${btnClass}" style="color: ${project.textColor}; border-color: ${project.textColor}">${link.text}</a>`;
      });
      
      content.innerHTML = `
        <h3 style="color: ${project.textColor}">${project.title}</h3>
        <p>${project.description}</p>
        <div class="project-links">
          ${linksHTML}
        </div>
      `;
      
      slide.appendChild(content);
      carousel.appendChild(slide);

      // Create dots
      const dot = document.createElement('div');
      dot.className = 'dot';
      if(index === 0) dot.classList.add('active');
      dot.addEventListener('click', () => goToSlide(index));
      dotsContainer.appendChild(dot);
    });

    const slides = document.querySelectorAll('.project-slide');
    const dots = document.querySelectorAll('.dot');

    function goToSlide(slideIndex) {
      slides.forEach((slide, index) => {
        slide.style.transform = `translateX(${100 * (index - slideIndex)}%)`;
      });
      
      dots.forEach(dot => dot.classList.remove('active'));
      dots[slideIndex].classList.add('active');
      currentSlide = slideIndex;
    }

    // Auto-rotate
    let slideInterval = setInterval(() => {
      currentSlide = (currentSlide + 1) % projects.length;
      goToSlide(currentSlide);
    }, 5000);

    // Pause on hover
    carousel.addEventListener('mouseenter', () => {
      clearInterval(slideInterval);
    });

    carousel.addEventListener('mouseleave', () => {
      slideInterval = setInterval(() => {
        currentSlide = (currentSlide + 1) % projects.length;
        goToSlide(currentSlide);
      }, 5000);
    });

    // Touch support for mobile
    let touchStartX = 0;
    let touchEndX = 0;

    carousel.addEventListener('touchstart', (e) => {
      touchStartX = e.changedTouches[0].screenX;
    }, false);

    carousel.addEventListener('touchend', (e) => {
      touchEndX = e.changedTouches[0].screenX;
      handleSwipe();
    }, false);

    function handleSwipe() {
      if (touchEndX < touchStartX) {
        // Swipe left - next slide
        currentSlide = (currentSlide + 1) % projects.length;
      }
      if (touchEndX > touchStartX) {
        // Swipe right - previous slide
        currentSlide = (currentSlide - 1 + projects.length) % projects.length;
      }
      goToSlide(currentSlide);
    }
});