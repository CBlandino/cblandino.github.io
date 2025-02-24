window.addEventListener('scroll', function () {
    const sections = document.querySelectorAll('section');
    const taskbarLinks = document.querySelectorAll('.taskbar .taskbar-link');

    let currentSection = '';
    sections.forEach(section => {
        const sectionTop = section.offsetTop;
        const sectionHeight = section.clientHeight;
        if (window.scrollY >= sectionTop - sectionHeight / 3) {
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